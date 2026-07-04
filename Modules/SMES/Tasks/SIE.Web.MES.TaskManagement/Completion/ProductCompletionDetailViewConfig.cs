using SIE.MES.TaskManagement.Completion;
using SIE.MetaModel.View;

namespace SIE.Web.MES.TaskManagement.Completion
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class ProductCompletionDetailViewConfig : WebViewConfig<ProductCompletionDetail>
    {
        protected override void ConfigListView()
        {
            View.DisableEditing().UseClientOrder();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);

            using (View.OrderProperties())
            {
                View.Property(p => p.TaskNo).ShowInList(150).Readonly();
                View.Property(p => p.TaskQty).ShowInList().Readonly();
                View.Property(p => p.ProductCode).ShowInList(150).Readonly();
                View.Property(p => p.ProductName).ShowInList(150).Readonly();
                View.Property(p => p.WorkOrderNo).ShowInList(150).Readonly();
                View.Property(p => p.ProcessCode).ShowInList(150).Readonly();
                View.Property(p => p.ProcessName).ShowInList(150).Readonly();
                View.Property(p => p.ReportedQty).ShowInList().Readonly();
                View.Property(p => p.OkQty).ShowInList().Readonly();
                View.Property(p => p.NgQty).ShowInList().Readonly();
                View.Property(p => p.ReworkQty).ShowInList().Readonly();
                View.Property(p => p.SuspectQty).ShowInList().Readonly();
                View.Property(p => p.ResourceCode).ShowInList(150).Readonly();
                View.Property(p => p.ResourceName).ShowInList(150).Readonly();
                View.Property(p => p.Classes).ShowInList().Readonly();
                //View.Property(p => p.ShiftCount).ShowInList().Readonly();
                View.Property(p => p.ScheduleStartTime).ShowInList(150).Readonly();
                View.Property(p => p.ScheduleEndTime).ShowInList(150).Readonly();
                View.Property(p => p.MrpController).ShowInList().Readonly();
            }
        }
    }
}
