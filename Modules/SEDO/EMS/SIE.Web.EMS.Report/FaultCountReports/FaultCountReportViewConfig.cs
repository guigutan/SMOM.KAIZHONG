using SIE.EMS.Report.FaultCountReports;
using SIE.MetaModel.View;

namespace SIE.Web.EMS.Report.FaultCountReports
{
    /// <summary>
    /// 故障统计报表查询视图
    /// </summary>
    public class FaultCountReportViewConfig : WebViewConfig<FaultCountReport>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.RepairNo).Show();
                View.Property(p => p.DeviceAbnormalRemark).HasLabel("故障说明").Show();
                View.Property(p => p.RepairState).Show();
                View.Property(p => p.EquipAccountCode).Show();
                View.Property(p => p.EquipAccountName).Show();
                View.Property(p => p.UseDepartment).Show();
                View.Property(p => p.InstallationLocation).HasLabel("位置").Show();
                View.Property(p => p.ApplyRepairEmployeeId).Show();
                View.Property(p => p.ApplyRepairDate).Show();
                View.Property(p => p.RepairMasterId).Show();
                View.Property(p => p.ReceiveOrderDate).Show();
                View.Property(p => p.RepairBeginDate).Show();
                View.Property(p => p.RepairFinishDate).Show();
                View.Property(p => p.FactoryName).Show();
            }
        }
    }
}
