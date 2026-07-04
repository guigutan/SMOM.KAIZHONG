using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items.Items;
using SIE.MES.Capacitys;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.Rbac.InvOrgs;
using System;
using System.Linq;

namespace SIE.MES.TaskManagement.DailyOutputReports
{
    /// <summary>
    /// 控制器
    /// </summary>
    public class DailyOutputReportController : DomainController
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<DailyOutputReport> GetDailyOutputReports(DailyOutputReportCriteria criteria)
        {
            if (criteria.PlanBeginTime.BeginValue == null || criteria.PlanBeginTime.EndValue == null)
                throw new ValidationException("日期不能为空");

            var startTime = criteria.PlanBeginTime.BeginValue;
            var endTime = criteria.PlanBeginTime.EndValue;
            var date1 = Convert.ToDateTime(startTime.Value.ToString("yyyy-MM-dd") + " 08:00:00");   //报工开始时间
            var date2 = Convert.ToDateTime(endTime.Value.AddDays(1).ToString("yyyy-MM-dd") + " 08:00:00");  //报工结束时间

            var datas = new EntityList<DailyOutputReport>();
            var q = Query<ReportRecord>();
            q.Where(p => p.ReportTime >= date1);
            q.Where(p => p.ReportTime <= date2);

            if (criteria.ResourceCode.IsNotEmpty())
                q.Where(p => p.DispatchTask.Resource.Code.Contains(criteria.ResourceCode));
            if (criteria.ResourceName.IsNotEmpty())
                q.Where(p => p.DispatchTask.Resource.Name.Contains(criteria.ResourceName));
            if (criteria.ProductCode.IsNotEmpty())
                q.Where(p => p.DispatchTask.Product.Code.Contains(criteria.ProductCode));
            if (criteria.ProductName.IsNotEmpty())
                q.Where(p => p.DispatchTask.Product.Name.Contains(criteria.ProductName));
            if (criteria.ShortDescription.IsNotEmpty())
                q.Where(p => p.DispatchTask.Product.ShortDescription.Contains(criteria.ShortDescription));
            if (criteria.MrpController.IsNotEmpty())
                q.Where(p => p.DispatchTask.Product.MrpController.Contains(criteria.MrpController));

            if (criteria.ProcessId > 0)
                q.Where(p => p.ProcessId == criteria.ProcessId);
            if (criteria.WorkOrderId > 0)
                q.Where(p => p.WorkOrderId == criteria.WorkOrderId);

            var records = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (records.Count == 0)
                return datas;
            var tasks = Query<DispatchTask>().Where(p => p.PlanBeginTime >= startTime && p.PlanBeginTime < endTime && (p.IsSchedulingInfReturn == YesNo.No || p.IsSchedulingInfReturn == null)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var productCodes = records.Select(p => p.ProductCode).Distinct().ToList();
            var parentItems = productCodes.SplitContains(temp =>
            {
                return Query<ParentItem>().Where(p => temp.Contains(p.Item.Code)).ToList();
            });

            var stdCapacitys = productCodes.SplitContains(temp =>
            {
                return Query<StandardCapacity>().Where(p => temp.Contains(p.Item.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
            records.OrderBy(p => p.ReportTime).GroupBy(p => new
            {
                Date = GetSummaryDate(p.ReportTime.Value),
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
                    ParentOldItem = parentItems.FirstOrDefault(p => p.Item.Code == dtl.Key.ProductCode)?.Bismt,
                    Capacity = stdCapacity?.Capacity ?? 0,
                    TaskQty = taskList.Sum(x => x.DispatchQty),
                };
                report.OuputQty_08to10 = GetTimeSlotReportQty(records, date, "08:00:00", "10:00:00", productCode, resourceCode, processCode);
                report.OuputQty_10to12 = GetTimeSlotReportQty(records, date, "10:00:00", "12:00:00", productCode, resourceCode, processCode);
                report.OuputQty_12to15 = GetTimeSlotReportQty(records, date, "12:00:00", "15:00:00", productCode, resourceCode, processCode);
                report.OuputQty_15to17 = GetTimeSlotReportQty(records, date, "15:00:00", "17:00:00", productCode, resourceCode, processCode);
                report.OuputQty_17to20 = GetTimeSlotReportQty(records, date, "17:00:00", "20:00:00", productCode, resourceCode, processCode);
                report.OuputQty_20to22 = GetTimeSlotReportQty(records, date, "20:00:00", "22:00:00", productCode, resourceCode, processCode);
                report.OuputQty_22to00 = GetTimeSlotReportQty(records, date, "22:00:00", "00:00:00", productCode, resourceCode, processCode);
                report.OuputQty_00to03 = GetTimeSlotReportQty(records, date.AddDays(1), "00:00:00", "03:00:00", productCode, resourceCode, processCode);
                report.OuputQty_03to05 = GetTimeSlotReportQty(records, date.AddDays(1), "03:00:00", "05:00:00", productCode, resourceCode, processCode);
                report.OuputQty_05to08 = GetTimeSlotReportQty(records, date.AddDays(1), "05:00:00", "08:00:00", productCode, resourceCode, processCode);

                report.DayShiftReportedQty = report.OuputQty_08to10 + report.OuputQty_10to12 + report.OuputQty_12to15 + report.OuputQty_15to17 + report.OuputQty_17to20;
                report.NightShiftReportedQty = report.OuputQty_20to22 + report.OuputQty_22to00 + report.OuputQty_00to03 + report.OuputQty_03to05 + report.OuputQty_05to08;
                report.ReportedQty = report.DayShiftReportedQty + report.NightShiftReportedQty;

                datas.Add(report);
            });
            datas.SetTotalCount(datas.Count);
            return datas;
        }

        /// <summary>
        /// 计算统计日期
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        DateTime GetSummaryDate(DateTime dateTime)
        {
            var date = dateTime.Date;
            if (dateTime.Hour < 8)
            {
                return date.AddDays(-1);
            }
            return date;
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
        decimal GetTimeSlotReportQty(EntityList<ReportRecord> records, DateTime date, string time1, string time2, string productCode, string resourceCode, string processCode)
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
    }
}
