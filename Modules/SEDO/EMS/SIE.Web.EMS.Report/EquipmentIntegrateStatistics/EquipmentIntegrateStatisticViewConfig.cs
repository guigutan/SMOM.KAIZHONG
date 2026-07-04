using SIE.EMS.Report.EquipmentIntegrateStatistics;
using SIE.Web.EMS.Report.EquipmentIntegrateStatistics.Commands;
using SIE.Web.Extensions;

namespace SIE.Web.EMS.Report.EquipmentIntegrateStatistics
{
    /// <summary>
    /// 不通过明细
    /// </summary>
    public class EquipmentIntegrateStatisticViewConfig : WebViewConfig<EquipmentIntegrateStatistic>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            //View.UseCommand(typeof(ExportCommand).FullName);
        }
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
           // View.RequireChartsResource();
            View.WithoutPaging();
            View.UseClientOrder(); 
        }
    }
}
