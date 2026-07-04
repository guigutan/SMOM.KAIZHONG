using SIE.MES.TaskManagement.WipProgress;
using SIE.MetaModel.View;

namespace SIE.Web.MES.TaskManagement.WipProgress
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class WipProgressViewModelViewConfig : WebViewConfig<WipProgressViewModel>
    {
        protected override void ConfigListView()
        {
            View.DisableEditing().UseClientOrder();
            View.UseCommands(WebCommandNames.ExportXlsAll);
            View.UseCommand("SIE.Web.MES.TaskManagement.WipProgress.Commands.WipProgressWipBatchViewCommand");

            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).ShowInList(150);
                View.Property(p => p.ProcessSeq);
                View.Property(p => p.ProcessCode);
                View.Property(p => p.ProcessName).ShowInList(150);
                View.Property(p => p.OldItem).ShowInList(150);
                View.Property(p => p.ParentOldItem).ShowInList(150);
                View.Property(p => p.ProcessStatus);
                View.Property(p => p.PlanQty);
                View.Property(p => p.OkQty);
                View.Property(p => p.InProcessQty);
                View.Property(p => p.FinishQty);
                //View.Property(p => p.PreFinishQty);
                //View.Property(p => p.PreOkQty);
                View.Property(p => p.BatchNo).ShowInList(200);
            }
        }
    }
}
