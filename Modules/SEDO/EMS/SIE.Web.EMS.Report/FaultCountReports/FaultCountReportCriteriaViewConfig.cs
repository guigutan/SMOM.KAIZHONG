using SIE.EMS.Report.FaultCountReports;
using SIE.Web.Equipments.Extensions;
using SIE.Web.Resources;

namespace SIE.Web.EMS.Report.FaultCountReports
{
    /// <summary>
    /// 故障统计报表查询视图
    /// </summary>
    public  class FaultCountReportCriteriaViewConfig : WebViewConfig<FaultCountReportCriteria>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryId).UseFactoryEditor().Show();
                View.Property(p => p.DepartmentId).UseFactoryDepartmentsEditor().Show();
                View.Property(p => p.Code).Show();
                View.Property(p => p.RepairMasterId).Show();
                View.Property(p => p.ApplyRepairDate).UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.Today; }).Show();
            }
        }
    }
}
