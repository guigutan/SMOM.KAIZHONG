using DocumentFormat.OpenXml.Wordprocessing;
using NPOI.SS.Formula.Functions;
using SIE.Api;
using SIE.Common.InvOrg;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Capacitys;
using SIE.MES.DashBoard.DashBoards.WorkShop.Datas;
using SIE.MES.ProcessProperty;
using SIE.MES.TaskManagement.DailyOutputReports;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.SchedulingInfs;
using SIE.Rbac.InvOrgs;
using SIE.Security;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Community.CsharpSqlite.Sqlite3;
using static IronPython.Modules._ast;

namespace SIE.MES.DashBoard.DashBoards.WorkShop
{
    public partial class WorkShopController : DomainController
    {
        #region 车间产量看板

        /// <summary>
        /// 车间产量看板
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("车间产量看板")]
        public virtual WorkshopOutputData GetWorkshopOutputData(double processId, int resourceId)
        {
            WorkshopOutputData data = new WorkshopOutputData();

            List<WorkshopOutputBottomData> grid1 = new List<WorkshopOutputBottomData>();

            List<WorkshopOutputRightData> grid2 = new List<WorkshopOutputRightData>();

            #region 时间获取规则
            // 获取当前时间
            System.DateTime now = Convert.ToDateTime(System.DateTime.Now.ToString("yyyyy-MM-dd HH:mm:ss"));
            System.DateTime timeMinus8Hours = now.AddHours(-8);

            System.DateTime previousTime = new System.DateTime(
                                    timeMinus8Hours.Year,      // 年（当前年）
                                    timeMinus8Hours.Month,     // 月（当前月）
                                    timeMinus8Hours.Day,       // 日（当前日）
                                    8,  // 小时（指定为小时）
                                    0,             // 分钟（00）
                                    0              // 秒（00）
                                    );

            System.DateTime currentDateTime = new System.DateTime(
                                        timeMinus8Hours.Year,      // 年（当前年）
                                        timeMinus8Hours.Month,     // 月（当前月）
                                        timeMinus8Hours.Day,       // 日（当前日）
                                        0,  // 小时（指定为小时）
                                        0,             // 分钟（00）
                                        0              // 秒（00）
                                        );

            TimeSpan timeDiff = now - previousTime;
            double hoursDiff = Math.Ceiling(timeDiff.TotalHours * 100) / 100;

            int currentHour = now.Hour;
            string currentTimeSlot = $"{currentHour:D2}:00-{currentHour + 1:D2}:00";

            // 计算上一个小时段
            int previousHour = currentHour - 1;
            // 处理0点的特殊情况（0点的上一个时段是23:00-00:00）
            if (previousHour < 0)
                previousHour = 23;
            string previousTimeSlot = $"{previousHour:D2}:00-{currentHour:D2}:00";

            #endregion
            RT.InvOrg = resourceId;
            var dailyReportsList = GetDailyOutputReports(now,processId);
            
            //当班计划
            data.ShiftPlanQty = dailyReportsList.Sum(p => p.TaskQty);
            //累计计划
            if (data.ShiftPlanQty != 0)
            {
                data.AccPlannedQty = Math.Ceiling(data.ShiftPlanQty / 24 * (decimal)hoursDiff * 100) / 100;
            }
            else
                data.AccPlannedQty = 0;
            //累计实际
            data.AccActualQty = dailyReportsList.Sum(p => p.ReportedQty);
            //差异
            data.DiffQty = data.AccPlannedQty - data.AccActualQty;

            foreach (var item in dailyReportsList)
            {
                WorkshopOutputRightData rightData = new WorkshopOutputRightData();
                rightData.Resource = item.ResourceName;
                rightData.Product = item.ShortDescription;
                rightData.PlanQty = item.TaskQty;
                rightData.ActualQty = item.ReportedQty;
                grid2.Add(rightData);
            }


            WorkshopOutputBottomData bottomData = new WorkshopOutputBottomData();
            bottomData.Time = previousTimeSlot;
            bottomData.PlanQty = Math.Ceiling(data.ShiftPlanQty / 24*100)/100;
            bottomData.ActualQty = HourActualQty(previousTimeSlot, dailyReportsList);
            bottomData.DiffQty = bottomData.PlanQty - bottomData.ActualQty;
            grid1.Add(bottomData);
            WorkshopOutputBottomData bottomData1 = new WorkshopOutputBottomData();
            bottomData1.Time = currentTimeSlot;
            bottomData1.PlanQty = Math.Ceiling(data.ShiftPlanQty / 24 * 100) / 100;
            bottomData1.ActualQty = HourActualQty(currentTimeSlot, dailyReportsList);
            bottomData1.DiffQty = bottomData1.PlanQty - bottomData1.ActualQty;
            grid1.Add(bottomData1);
            data.grid1 = grid1;
            data.grid2 = grid2;

            var workSafety = Query<WorkSafety>().ToList();
            if (workSafety.Count>0)
            {
                var InitialTime = workSafety.FirstOrDefault().SafetyDate;
                TimeSpan timeDay = (TimeSpan)(now - InitialTime);
                int daysDiff = timeDay.Days;
                data.SafetyDateNum = (int)Math.Floor((decimal)daysDiff);
            }
            else
            {
                data.SafetyDateNum = 1;
            }

                return data;
        }

        /// <summary>
        /// 车间库存组织下拉看板
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("车间库存组织下拉看板")]
        public virtual List<InvOrgData> GetInvOrgDatas()
        {
            List<InvOrgData> invOrgs = new List<InvOrgData>();
            var ing = Query<Rbac.InvOrgs.InvOrg>().ToList();
            foreach (var item in ing)
            {
                InvOrgData invOrgData = new InvOrgData();
                invOrgData.InvName = item.Name;
                invOrgData.InvCode = item.Code;
                invOrgs.Add(invOrgData);
            }

            return invOrgs;
        }


        /// <summary>
        /// 车间工序下拉看板
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("车间工序下拉看板")]
        public virtual List<ProcessData> GetProcessDatas(int invOrgId)
        {
            List<ProcessData> processDatas = new List<ProcessData>();
            //int a = GetInvOrgId();
            RT.InvOrg = invOrgId;
            var ProcessDataList = Query<ProcessPty>().ToList();
            foreach (var item in ProcessDataList)
            {
                ProcessData processData = new ProcessData();
                processData.ProcessName = item.Process.Name;
                processData.ProcessCode = item.Process.Code;
                processData.ProcessId = item.ProcessId;
                processDatas.Add(processData);
            }
            return processDatas;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<DailyOutputReport> GetDailyOutputReports(System.DateTime? PlanBeginTime,double processId)
        {
            if (PlanBeginTime == null)
                throw new ValidationException("日期不能为空");

            var startTime = Convert.ToDateTime(PlanBeginTime.Value.ToString("yyyy-MM-dd") + " 00:00:00");
            var endTime = Convert.ToDateTime(PlanBeginTime.Value.ToString("yyyy-MM-dd") + " 23:59:59");
            var date1 = Convert.ToDateTime(PlanBeginTime.Value.ToString("yyyy-MM-dd") + " 08:00:00");   //报工开始时间
            var date2 = Convert.ToDateTime(PlanBeginTime.Value.AddDays(1).ToString("yyyy-MM-dd") + " 08:00:00");  //报工结束时间

            var datas = new EntityList<DailyOutputReport>();
            var q = Query<ReportRecord>();
            q.Where(p => p.ReportTime >= date1);
            q.Where(p => p.ReportTime <= date2);
            if (processId > 0)
                q.Where(p => p.ProcessId == processId);

            var records = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (records.Count == 0)
                return datas;
            var tasks = Query<DispatchTask>().Where(p => p.PlanBeginTime >= startTime && p.PlanBeginTime < endTime && p.TaskStatus != DispatchTaskStatus.Closed).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var productCodes = records.Select(p => p.ProductCode).Distinct().ToList();
            var stdCapacitys = Query<StandardCapacity>().Where(p => productCodes.Contains(p.Item.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
            records.OrderBy(p => p.ReportTime).GroupBy(p => new
            {
                p.ReportTime.Value.Date,
                p.ProductCode,
                p.ProductName,
                p.ResourceCode,
                p.ResourceName,
                p.ProcessCode,
                p.ProcessName,
                p.MrpController,
                p.ShortDescription,

            }).ForEach(dtl =>
            {
                var date = dtl.Key.Date;
                var productCode = dtl.Key.ProductCode;
                var resourceCode = dtl.Key.ResourceCode;
                var processCode = dtl.Key.ProcessCode;
                var taskList = tasks.Where(p => p.PlanBeginTime.Date == date && p.ProductCode == productCode && p.ResourceCode == resourceCode && p.ProcessCode == processCode).ToList();
                //var dayShiftTasks = records.Where(p => p.ReportTime.Value.Date == date && p.Classes == ClassesType.Day).ToList();  //白班任务
                //var nightShiftTasks = records.Where(p => p.ReportTime.Value.Date == date && p.Classes == ClassesType.Night).ToList();  //夜班任务

                //班次产能；取标准产能维护基础表的值（取值逻辑：按优先级从高到低的维度获取一个值，优先级高-资源+物料编码+工序，优先级中-物料编码+工序，优先级低-物料编码）
                var stdCapacity = stdCapacitys.FirstOrDefault(p => p.ItemCode == dtl.Key.ProductCode && p.ProcessCode == dtl.Key.ProcessCode && p.ResourceCode == dtl.Key.ResourceCode);
                if (stdCapacity == null)
                    stdCapacity = stdCapacitys.FirstOrDefault(p => p.ItemCode == dtl.Key.ProductCode && p.ProcessCode == dtl.Key.ProcessCode && p.ResourceId == null);
                if (stdCapacity == null)
                    stdCapacity = stdCapacitys.FirstOrDefault(p => p.ItemCode == dtl.Key.ProductCode && p.ProcessId == null && p.ResourceId == null);
                var report = new DailyOutputReport
                {
                    Date = date,
                    Division = "",
                    Department = "",
                    Factory = invOrg?.ExternalId,
                    ProductCode = dtl.Key.ProductCode,
                    ProductName = dtl.Key.ProductName,
                    ResourceCode = dtl.Key.ResourceCode,
                    ResourceName = dtl.Key.ResourceName,
                    ProcessCode = dtl.Key.ProcessCode,
                    ProcessName = dtl.Key.ProcessName,
                    MrpController = dtl.Key.MrpController,
                    ShortDescription = dtl.Key.ShortDescription,
                    Capacity = stdCapacity?.Capacity ?? 0,
                    TaskQty = taskList.Sum(x => x.DispatchQty),
                    //ReportedQty = dtl.Sum(x => x.ReportQty),
                    //DayShiftTaskQty = dayShiftTasks.Sum(x => x.DispatchQty),
                    //DayShiftReportedQty = dayShiftTasks.Sum(x => x.ReportQty),
                    //NightShiftTaskQty = nightShiftTasks.Sum(x => x.DispatchQty),
                    //NightShiftReportedQty = nightShiftTasks.Sum(x => x.ReportQty),
                };
                report.OuputQty_08to09 = GetTimeSlotReportQty(records, date, "08:00:00", "09:00:00", productCode, resourceCode, processCode);
                report.OuputQty_09to10 = GetTimeSlotReportQty(records, date, "09:00:00", "10:00:00", productCode, resourceCode, processCode);
                report.OuputQty_10to11 = GetTimeSlotReportQty(records, date, "10:00:00", "11:00:00", productCode, resourceCode, processCode);
                report.OuputQty_11to12 = GetTimeSlotReportQty(records, date, "11:00:00", "12:00:00", productCode, resourceCode, processCode);
                report.OuputQty_12to13 = GetTimeSlotReportQty(records, date, "12:00:00", "13:00:00", productCode, resourceCode, processCode);
                report.OuputQty_13to14 = GetTimeSlotReportQty(records, date, "13:00:00", "14:00:00", productCode, resourceCode, processCode);
                report.OuputQty_14to15 = GetTimeSlotReportQty(records, date, "14:00:00", "15:00:00", productCode, resourceCode, processCode);
                report.OuputQty_15to16 = GetTimeSlotReportQty(records, date, "15:00:00", "16:00:00", productCode, resourceCode, processCode);
                report.OuputQty_16to17 = GetTimeSlotReportQty(records, date, "16:00:00", "17:00:00", productCode, resourceCode, processCode);
                report.OuputQty_17to18 = GetTimeSlotReportQty(records, date, "17:00:00", "18:00:00", productCode, resourceCode, processCode);
                report.OuputQty_18to19 = GetTimeSlotReportQty(records, date, "18:00:00", "19:00:00", productCode, resourceCode, processCode);
                report.OuputQty_19to20 = GetTimeSlotReportQty(records, date, "19:00:00", "20:00:00", productCode, resourceCode, processCode);
                report.OuputQty_00to01 = GetTimeSlotReportQty(records, date.AddDays(1), "00:00:00", "01:00:00", productCode, resourceCode, processCode);
                report.OuputQty_01to02 = GetTimeSlotReportQty(records, date.AddDays(1), "01:00:00", "02:00:00", productCode, resourceCode, processCode);
                report.OuputQty_02to03 = GetTimeSlotReportQty(records, date.AddDays(1), "02:00:00", "03:00:00", productCode, resourceCode, processCode);
                report.OuputQty_03to04 = GetTimeSlotReportQty(records, date.AddDays(1), "03:00:00", "04:00:00", productCode, resourceCode, processCode);
                report.OuputQty_04to05 = GetTimeSlotReportQty(records, date.AddDays(1), "04:00:00", "05:00:00", productCode, resourceCode, processCode);
                report.OuputQty_05to06 = GetTimeSlotReportQty(records, date.AddDays(1), "05:00:00", "06:00:00", productCode, resourceCode, processCode);
                report.OuputQty_06to07 = GetTimeSlotReportQty(records, date.AddDays(1), "06:00:00", "07:00:00", productCode, resourceCode, processCode);
                report.OuputQty_07to08 = GetTimeSlotReportQty(records, date.AddDays(1), "07:00:00", "08:00:00", productCode, resourceCode, processCode);
                report.OuputQty_20to21 = GetTimeSlotReportQty(records, date.AddDays(1), "20:00:00", "21:00:00", productCode, resourceCode, processCode);
                report.OuputQty_21to22 = GetTimeSlotReportQty(records, date.AddDays(1), "21:00:00", "22:00:00", productCode, resourceCode, processCode);
                report.OuputQty_22to23 = GetTimeSlotReportQty(records, date.AddDays(1), "22:00:00", "23:00:00", productCode, resourceCode, processCode);
                report.OuputQty_23to00 = GetTimeSlotReportQty(records, date.AddDays(1), "23:00:00", "00:00:00", productCode, resourceCode, processCode);


                report.DayShiftReportedQty = report.OuputQty_08to09 + report.OuputQty_09to10 + report.OuputQty_10to11 + report.OuputQty_11to12 + report.OuputQty_12to13 + report.OuputQty_13to14
                + report.OuputQty_14to15 + report.OuputQty_15to16 + report.OuputQty_16to17 + report.OuputQty_17to18 + report.OuputQty_18to19 + report.OuputQty_19to20;
                report.NightShiftReportedQty = report.OuputQty_00to01 + report.OuputQty_01to02 + report.OuputQty_02to03 + report.OuputQty_03to04 + report.OuputQty_04to05 + report.OuputQty_05to06
                + report.OuputQty_06to07 + report.OuputQty_07to08 + report.OuputQty_20to21 + report.OuputQty_21to22 + report.OuputQty_22to23 + report.OuputQty_23to00;
                report.ReportedQty = report.DayShiftReportedQty + report.NightShiftReportedQty;

                datas.Add(report);
            });
            datas.SetTotalCount(datas.Count);
            return datas;
        }

        /// <summary>
        /// 获取时间段内报工数量
        /// </summary>
        /// <param name="records"></param>
        /// <param name="date"></param>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <param name="productCode"></param>
        /// <param name="resourceCode"></param>
        /// <param name="processCode"></param>
        /// <returns></returns>
        decimal GetTimeSlotReportQty(EntityList<ReportRecord> records, System.DateTime date, string time1, string time2, string productCode, string resourceCode, string processCode)
        {
            var date1 = Convert.ToDateTime(date.ToString("yyyy-MM-dd") + " " + time1);
            var date2 = Convert.ToDateTime(date.ToString("yyyy-MM-dd") + " " + time2);
            if (time2 == "00:00:00")
                date2 = date2.AddDays(1);
            var reportQty = records.Where(p => p.ReportTime >= date1 && p.ReportTime < date2
                                        && p.ProductCode == productCode
                                        && p.ResourceCode == resourceCode
                                        && p.ProcessCode == processCode
                                        ).Sum(p => p.ReportQty);
            return reportQty;
        }


        /// <summary>
        /// 根据时间获取计划数
        /// </summary>
        /// <param name="currentTime"></param>
        /// <param name="dailies"></param>
        /// <returns></returns>
        public virtual decimal HourActualQty(string currentTime,EntityList<DailyOutputReport> dailies)
        {
            decimal actualQty = 0;
            switch (currentTime)
            {
                case "00:00-01:00":
                    actualQty= dailies.Sum(p => p.OuputQty_00to01);
                    break;
                case "01:00-02:00":
                    actualQty = dailies.Sum(p => p.OuputQty_01to02);
                    break;
                case "02:00-03:00":
                    actualQty = dailies.Sum(p => p.OuputQty_02to03);
                    break;
                case "03:00-04:00":
                    actualQty = dailies.Sum(p => p.OuputQty_03to04);
                    break;
                case "04:00-05:00":
                    actualQty = dailies.Sum(p => p.OuputQty_04to05);
                    break;
                case "05:00-06:00":
                    actualQty = dailies.Sum(p => p.OuputQty_05to06);
                    break;
                case "06:00-07:00":
                    actualQty = dailies.Sum(p => p.OuputQty_06to07);
                    break;
                case "07:00-08:00":
                    actualQty = dailies.Sum(p => p.OuputQty_07to08);
                    break;
                case "08:00-09:00":
                    actualQty = dailies.Sum(p => p.OuputQty_08to09);
                    break;
                case "09:00-10:00":
                    actualQty = dailies.Sum(p => p.OuputQty_09to10);
                    break;
                case "10:00-11:00":
                    actualQty = dailies.Sum(p => p.OuputQty_10to11);
                    break;
                case "11:00-12:00":
                    actualQty = dailies.Sum(p => p.OuputQty_11to12);
                    break;
                case "12:00-13:00":
                    actualQty = dailies.Sum(p => p.OuputQty_12to13);
                    break;
                case "13:00-14:00":
                    actualQty = dailies.Sum(p => p.OuputQty_13to14);
                    break;
                case "14:00-15:00":
                    actualQty = dailies.Sum(p => p.OuputQty_14to15);
                    break;
                case "15:00-16:00":
                    actualQty = dailies.Sum(p => p.OuputQty_15to16);
                    break;
                case "16:00-17:00":
                    actualQty = dailies.Sum(p => p.OuputQty_16to17);
                    break;
                case "17:00-18:00":
                    actualQty = dailies.Sum(p => p.OuputQty_17to18);
                    break;
                case "18:00-19:00":
                    actualQty = dailies.Sum(p => p.OuputQty_18to19);
                    break;
                case "19:00-20:00":
                    actualQty = dailies.Sum(p => p.OuputQty_19to20);
                    break;
                case "20:00-21:00":
                    actualQty = dailies.Sum(p => p.OuputQty_20to21);
                    break;
                case "21:00-22:00":
                    actualQty = dailies.Sum(p => p.OuputQty_21to22);
                    break;
                case "22:00-23:00":
                    actualQty = dailies.Sum(p => p.OuputQty_22to23);
                    break;
                case "23:00-00:00":
                    actualQty = dailies.Sum(p => p.OuputQty_23to00);
                    break;

            }
            return actualQty;
        }

        #endregion
    }
}
