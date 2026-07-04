using SIE.Domain;
using SIE.EMS.Common.Utils;
using SIE.EMS.Enums;
using SIE.EMS.InspectionRules;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.Equipments.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.MeteringEquipment.Calibrations.Handle
{
    /// <summary>
    /// 自动生产计量设备定检计划
    /// </summary>
    [Services.Service(FallbackType = typeof(CalibrationAtuoScheduleHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]

    public class CalibrationAtuoScheduleHandle
    {
        /// <summary>
        /// 检验规程配置池
        /// </summary>
        readonly InspectionRulePool ilpool;

        /// <summary>
        /// 快速生成关键事件进度ID对象
        /// </summary>
        protected QuickGenerateIdHelper GenerateIdHelper { get; set; }

        /// <summary>
        /// 计量设备台账控制器
        /// </summary>
        private CalibrationController CalConter { get; set; }

        #region 定义

        /// <summary>
        /// 计量设备台账与设备定检规程关联关系字典集合
        /// </summary>
        private Dictionary<double, List<EquipAccountCalibration>> DicCalibration { get; set; }

        /// <summary>
        /// 检验规程字典集合    key：检验规程id value:检验规程
        /// </summary>
        private Dictionary<double, List<InspectionProjectItem>> DicInspectionPro { get; set; }

        /// <summary>
        /// 保存的数据实体
        /// </summary>
        private EntityList<Calibration> CalibrationList { get; set; }

        private EntityList<EquipAccountCalibration> EquipAccountCalibrationList { get; set; }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public CalibrationAtuoScheduleHandle()
        {
            GenerateIdHelper = new QuickGenerateIdHelper();
            ilpool = new InspectionRulePool();
            CalConter = RT.Service.Resolve<CalibrationController>();
            CalibrationList = new EntityList<Calibration>();
            DicCalibration = new Dictionary<double, List<EquipAccountCalibration>>();
            DicInspectionPro = new Dictionary<double, List<InspectionProjectItem>>();
            EquipAccountCalibrationList = new EntityList<EquipAccountCalibration>();
        }

        /// <summary>
        ///  入口
        /// </summary>
        public virtual void DoSchedule()
        {
            //加载基础数据
            LoadBase();
            //生成实体数据
            InitDate();
            //保存
            SaveDate();
        }

        /// <summary>
        /// 加载基础数据
        /// </summary>
        public virtual void LoadBase()
        {
            //初始化配置池数据
            ilpool.Load();

            //获取 设备定检规程(计量设备台账)（并且首次定检日期和下次检验日期不可同时为null）
            List<EquipAccountCalibration> calibrationList = CalConter.GetEquipAccountCalibration().ToList();
            DicCalibration = calibrationList.GroupBy(p => p.MeteringEquipmentAccountId).ToDictionary(p => p.Key, p => p.ToList());
            //根据计量设备台账与检验定检规程
            List<double> InspectionRuleIds = calibrationList.Select(p => p.InspectionRuleId).Distinct().ToList();

            //根据检验规程找 检验规程与点检项目的关系数据转字典
            DicInspectionPro = RT.Service.Resolve<InspectionRuleController>().GetInspectionProjectItemList(InspectionRuleIds).GroupBy(p => p.InspectionRuleId).ToDictionary(p => p.Key, p => p.ToList());
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void InitDate()
        {
            //获取数据库时间
            var now = RF.Find<EquipAccountCalibration>().GetDbTime();
            List<EquipAccountCalibration> RegularInsList = null;
            Calibration reg = null;
            foreach (var Specialid in DicCalibration.Keys)
            {
                if (DicCalibration.TryGetValue(Specialid, out RegularInsList))
                {
                    foreach (var RegularIns in RegularInsList)
                    {
                        //当前系统时间加预警期大于等于下次检验日期
                        if (now.AddDays(RegularIns.WarningPeriod) >= RegularIns.NextInspectionDate)
                        {
                            //一条关系创建一个计量设备定检
                            reg = CreateCalibration(RegularIns);
                            CalibrationList.Add(reg);
                            //创建的此数据存在未提交的情况,不能重复生成此条数据
                            RegularIns.NotSubmit = false;
                            EquipAccountCalibrationList.Add(RegularIns);
                        }
                    }
                }
            }
            InitDetail();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void InitDetail()
        {
            List<InspectionProjectItem> InspecProjectItemList = null;
            //为计量设备定检生成id和code
            if (CalibrationList.Any())
            {
                List<double> CalibrationIds = BatchDataEntity.GetBatchEntityId<Calibration>(CalibrationList.Count);
                List<string> InspectionNoList = CalConter.GetCalibrationNo(CalibrationList.Count);
                for (int i = 0; i < CalibrationList.Count; i++)
                {
                    CalibrationList[i].Id = CalibrationIds[i];
                    CalibrationList[i].InspectionNo = InspectionNoList[i];
                    CalibrationList[i].PlanName = "自动计划_" + InspectionNoList[i];
                }
            }
            CalibrationResume resume = null;
            CalibrationItem calItem = null;
            List<CalibrationItem> calItemList = null;
            List<CalibrationResume> resumeList = null;
            //根据计量设备定检的检验规程所关联的检验项目自动生成 计量设备定检子页签数据
            foreach (var calibration in CalibrationList)
            {
                calItemList = new List<CalibrationItem>();
                resumeList = new List<CalibrationResume>();
                if (DicInspectionPro.TryGetValue(calibration.InspectionRuleId, out InspecProjectItemList))
                {
                    foreach (var inspecproitem in InspecProjectItemList)
                    {
                        calItem = CreateCalibrationItem(calibration.Id, inspecproitem);
                        calItemList.Add(calItem);
                    }
                }
                if (calItemList.Any())
                {
                    calibration.CalibrationItemList.AddRange(calItemList);
                }
                //操作记录值创建一次
                resume = CreateCalibrationResume(calibration.Id);
                resumeList.Add(resume);
                if (resumeList.Any())
                {
                    calibration.CalibrationResumeList.AddRange(resumeList);
                }
            }
        }

        /// <summary>
        /// 创建操作记录对象
        /// </summary>
        /// <param name="CalibrationId">设备定检对象id</param>
        /// <returns></returns>
        public virtual CalibrationResume CreateCalibrationResume(double CalibrationId)
        {
            CalibrationResume resume = new CalibrationResume();
            resume.GenerateId();
            resume.CalibrationId = CalibrationId;
            resume.OperationType = OperationType.CREATE;
            resume.OperatorId = RT.IdentityId;
            resume.OperationDateTime = DateTime.Now;
            resume.CreateBy = RT.IdentityId;
            resume.CreateDate = DateTime.Now;
            return resume;
        }

        /// <summary>
        /// 创建计量设备定检
        /// </summary>
        /// <param name="RegularIns"></param>
        /// <returns></returns>
        public virtual Calibration CreateCalibration(EquipAccountCalibration RegularIns)
        {
            Calibration cal = new Calibration();
            if (RegularIns != null)
            {
                //创建计量设备定检
                cal.InspectionRuleId = RegularIns.InspectionRuleId;
                cal.PlanInspectionDate = DateTime.Today;
                cal.InspectionStatus = InspectionStatus.Pending;
                cal.ApprovalStatus = ApprovalStatus.Draft;
                cal.BillSourceType = BillSourceType.Automatically;

                //生成设备明细
                CalibrationEquipment calequ = new CalibrationEquipment();
                calequ.MeteringEquipmentAccountId = RegularIns.MeteringEquipmentAccountId;
                calequ.PrecisionClass = RegularIns.PrecisionClass;
                calequ.IsDowngrade = (RegularIns.MeteringEquipmentAccount.Downgrade != null) && (bool)RegularIns.MeteringEquipmentAccount.Downgrade;
                calequ.PrecisionClass = RegularIns.MeteringEquipmentAccount.PrecisionClass;

                calequ.InspectionDate = null;
                calequ.InspectionResult = null;

                cal.CalibrationEquipmentList.Add(calequ);
            }

            //创建操作记录对象(生成后统一再创建履历)
            return cal;
        }

        /// <summary>
        /// 创建计量设备定检检验项目
        /// </summary>
        /// <param name="CalibrationId">计量设备定检单Id</param>
        /// <param name="insproitem"></param>
        /// <returns></returns>
        public virtual CalibrationItem CreateCalibrationItem(double CalibrationId, InspectionProjectItem insproitem)
        {
            CalibrationItem calItem = new CalibrationItem();
            calItem.Id = GenerateIdHelper.GetNewId<CalibrationItem>();
            calItem.CalibrationId = CalibrationId;
            if (insproitem != null)
            {
                calItem.ProjectDetailId = insproitem.ProjectDetailId;
                calItem.Name = insproitem.ProjectName;
                calItem.Part = insproitem.Part;
                calItem.Consumable = insproitem.Consumable;
                calItem.Method = insproitem.Method;
                calItem.Standard = insproitem.Standard;
                calItem.MinValue = insproitem.MinValue;
                calItem.MaxValue = insproitem.MaxValue;
                calItem.UseTime = insproitem.UseTime;
                calItem.Unit = insproitem.Unit;
            }
            return calItem;
        }

        /// <summary>
        /// 保存数据d
        /// </summary>
        public virtual void SaveDate()
        {
            using (var tran = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                if (CalibrationList.Any())
                {
                    RF.Save(CalibrationList);
                }
                if (EquipAccountCalibrationList.Any())
                {
                    RF.Save(EquipAccountCalibrationList);
                }
                tran.Complete();
            }
        }
    }
}
