using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.EMS.Maintains.Controller;
using SIE.Equipments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.EMS.Maintains.Plans.ImportMaintainPlans
{
    /// <summary>
    /// 导入类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportMaintainPlanHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportMaintainPlanHandle : IBusinessImport
    {
        private const string PLAN_BEGIN_DATE = "计划开始日期";
        private const string PLAN_END_DATE = "计划结束日期";
        private const string PLAN_MAITAIN_TIME_LENGTH = "计划保养时长(H)";
        private const string SPECIAL_PLAN_BEGIN_DATE_TIME = "指定计划开始时间";
        private const string SPECIAL_PLAN_END_DATE_TIME = "指定计划结束时间";

        #region 私有属性        

        /// <summary>
        /// 备件资料--已存在则会被记录
        /// </summary>
        private Dictionary<string, MaintainPlan> MaintainPlanDic { get; } = new Dictionary<string, MaintainPlan>();
        #endregion

        /// <summary>
        /// 导入模板的列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
             "设备编码", PLAN_BEGIN_DATE, PLAN_END_DATE, "保养类型", SPECIAL_PLAN_BEGIN_DATE_TIME,
            SPECIAL_PLAN_END_DATE_TIME, PLAN_MAITAIN_TIME_LENGTH
        };

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建导入备件基础数据标准对象
        /// </summary>
        /// <returns>返回料号检验标准对象</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("设备编码", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add(PLAN_BEGIN_DATE, new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add(PLAN_END_DATE, new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("保养类型", new ValidColumn(ImportDataType._Enum, true, true));
            this.ColumnValidList.Add(SPECIAL_PLAN_BEGIN_DATE_TIME, new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add(SPECIAL_PLAN_END_DATE_TIME, new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add(PLAN_MAITAIN_TIME_LENGTH, new ValidColumn(ImportDataType._Int, false, true));
            return this;
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs == null || drs.Length <= 0)
            {
                return;
            }

            //查询所有设备
            var equipAccountCodes = drs.Select(x => x["设备编码"].ToString()).Distinct().ToList();

            var equipAccounts = RT.Service.Resolve<CoreEquipController>().GetEquipAccountBaseInfos(equipAccountCodes);

            var maintainController = RT.Service.Resolve<MaintainController>();

            // 设备保养计划
            var maintainPlanInfos = maintainController.GetMaintainPlanInfos(equipAccounts.Select(p => p.Id).ToList());

            //循环检验每一行主数据
            foreach (var dr in drs)
            {
                var eqiupAccountCode = dr["设备编码"].ToString();
                var equip = equipAccounts.FirstOrDefault(p => p.Code == eqiupAccountCode);

                MaintainPlan plan = null;
                if (MaintainPlanDic.Count > 0 && MaintainPlanDic.ContainsKey(eqiupAccountCode + "-" + dr[PLAN_BEGIN_DATE]))
                {
                    continue;
                }

                if (plan == null)
                {
                    if (equip == null)
                    {
                        ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName,
                            "设备编码【{0}】不存在或无该设备的权限".L10nFormat(eqiupAccountCode));
                        continue;
                    }

                    plan = new MaintainPlan();

                    var equipAccountId = equip.Id;
                    plan.EquipAccountId = equipAccountId;

                    DateTime beginDate, endDate;

                    bool planBRight = DateTime.TryParse(dr[PLAN_BEGIN_DATE].ToString(), out DateTime planBtime);
                    bool planERight = DateTime.TryParse(dr[PLAN_END_DATE].ToString(), out DateTime planEtime);
                    if (!planBRight)
                    {
                        ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName,
                            "{0}格式不正确".L10nFormat(PLAN_BEGIN_DATE));
                        continue;
                    }
                    else
                    {
                        beginDate = planBtime;
                    }
                    if (!planERight)
                    {
                        ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName,
                            "{0}格式不正确".L10nFormat(PLAN_END_DATE));
                        continue;
                    }
                    else
                    {
                        endDate = planEtime;
                    }

                    var existsExcelPlan = drs.Where(p => p["设备编码"].ToString() == eqiupAccountCode && p[PLAN_BEGIN_DATE].ToString() == dr[PLAN_BEGIN_DATE].ToString() && p[PLAN_END_DATE].ToString() == dr[PLAN_END_DATE].ToString());
                    if (existsExcelPlan.Count() > 1)
                    {
                        ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName,
                            "设备在计划开始日期【{0}】-计划结束日期【{1}】已存在保养计划".L10nFormat(dr[PLAN_BEGIN_DATE].ToString(), dr[PLAN_END_DATE].ToString()));
                        continue;

                    }

                    var existsDBPlan = maintainPlanInfos.FirstOrDefault(p => p.EquipId == equip.Id && p.PlanBeginDateValue.Date == beginDate.Date && p.PlanEndDateValue.Date == endDate.Date);
                    if (existsDBPlan != null)
                    {
                        ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName,
                            "设备在计划开始日期【{0}】-计划结束日期【{1}】已存在保养计划".L10nFormat(beginDate.Date, endDate.Date));
                        continue;

                    }

                    //验证计划开始时间和计划结束时间是周一和周日且只跨度一周，通过这个计算【年/月、周期】
                    var tupleWeekInfo = maintainController.GetWeekInfoOfDateTime(beginDate);

                    var year = tupleWeekInfo.Item1;
                    var week = tupleWeekInfo.Item2;
                    var firstDayOfWeek = tupleWeekInfo.Item3;
                    var lastDayOfWeek = tupleWeekInfo.Item4;
                    if (beginDate != firstDayOfWeek)
                    {
                        ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName,
                            "计划开始日期【{0}】不是星期一".L10nFormat(beginDate));
                        continue;
                    }

                    
                    if ((endDate - beginDate).TotalDays != 6)
                    {
                        ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName,
                            "计划开始日期【{0}】跟 计划结束日期【{1}】跨度不是一周".L10nFormat(beginDate, endDate));
                        continue;
                    }

                    if (endDate != lastDayOfWeek)
                    {
                        ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName,
                            "计划结束日期【{0}】不是星期日".L10nFormat(endDate));
                        continue;
                    }

                    plan.YearAndMonth = new DateTime(year, lastDayOfWeek.Month, 1);
                    plan.Cycle = week;
                    plan.PlanBeginDate = beginDate;
                    plan.PlanEndDate = endDate;

                    //验证【指定计划开始时间和指定计划结束时间】在【计划开始时间和计划结束时间】之内
                    if (dr[SPECIAL_PLAN_BEGIN_DATE_TIME] != null && dr[SPECIAL_PLAN_BEGIN_DATE_TIME].ToString() != "")
                    {
                        bool timeRight = DateTime.TryParse(dr[SPECIAL_PLAN_BEGIN_DATE_TIME].ToString(), out DateTime beginTime);
                        if (!timeRight)
                        {
                            ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName,
                                "{0}格式错误".L10nFormat(SPECIAL_PLAN_BEGIN_DATE_TIME));
                            continue;
                        }
                        else
                        {
                            plan.PrecisePlanBeginDate = beginTime;
                        }
                        if (plan.PrecisePlanBeginDate < beginDate || plan.PrecisePlanBeginDate > endDate)
                        {
                            ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName,
                                "指定计划开始时间【{0}】，不在【计划开始时间和计划结束时间】范围之内"
                                .L10nFormat(plan.PrecisePlanBeginDate));
                            continue;
                        }
                    }

                    if (dr[SPECIAL_PLAN_END_DATE_TIME] != null && dr[SPECIAL_PLAN_END_DATE_TIME].ToString() != "")
                    {
                        bool timeRight = DateTime.TryParse(dr[SPECIAL_PLAN_END_DATE_TIME].ToString(), out DateTime endTime);
                        if (!timeRight)
                        {
                            ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName,
                                "{0}格式错误".L10nFormat(SPECIAL_PLAN_END_DATE_TIME));
                            continue;
                        }
                        else
                        {
                            plan.PrecisePlanEndDate = endTime;
                        }
                        if (plan.PrecisePlanEndDate < beginDate || plan.PrecisePlanEndDate > endDate)
                        {
                            ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName,
                                "指定计划结束时间【{0}】，不在【计划开始时间和计划结束时间】范围之内"
                                .L10nFormat(plan.PrecisePlanEndDate));
                            continue;
                        }
                    }

                    if (dr[PLAN_MAITAIN_TIME_LENGTH] != null && dr[PLAN_MAITAIN_TIME_LENGTH].ToString() != "")
                    {
                        plan.MaintainTime = Convert.ToInt32(dr[PLAN_MAITAIN_TIME_LENGTH].ToString());
                    }

                    GetMaintainType(dr, plan);

                    plan.EquipMaintainType = Checks.Plans.EquipCheckType.Equip;

                    try
                    {
                        RT.Service.Resolve<MaintainController>().AddMaintainPlans(new EntityList<MaintainPlan>() { plan }, new List<double>() { equipAccountId });
                    }
                    catch (Exception ex)
                    {
                        string strMsg = ex.GetBaseException()?.Message;
                        ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName, strMsg);
                    }
                }
            }
        }

        private void GetMaintainType(DataRow dr, MaintainPlan plan)
        {
            switch (dr["保养类型"].ToString())
            {
                case "周":
                    plan.MaintainType = MaintainType.Week;
                    break;
                case "双周":
                    plan.MaintainType = MaintainType.DbWeek;
                    break;
                case "月":
                    plan.MaintainType = MaintainType.Month;
                    break;
                case "双月":
                    plan.MaintainType = MaintainType.DbMonth;
                    break;
                case "季":
                    plan.MaintainType = MaintainType.Season;
                    break;
                case "半年":
                    plan.MaintainType = MaintainType.HalfYear;
                    break;
                case "年":
                    plan.MaintainType = MaintainType.Year;
                    break;
                default:
                    break;
            }
        }
    }
}
