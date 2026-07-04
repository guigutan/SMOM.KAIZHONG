
using DocumentFormat.OpenXml.Wordprocessing;
using SIE.MES.TaskManagement.ProcessTaskLists;

namespace SIE.Web.MES.TaskManagement.ProcessTaskLists
{
    /// <summary>
    /// 拆分任务ViewModel主视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class SplitTaskViewModelViewConfig : WebViewConfig<SplitTaskViewModel>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProcessTaskListViewModel));
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(2);
            View.Property(p => p.ProcessName).ShowInDetail().Readonly();
            View.Property(p => p.DispatchQty).ShowInDetail().Readonly();
            View.Property(p => p.DispatchedTaskQty).ShowInDetail().Readonly();
            View.Property(p => p.Qty).ShowInDetail();
            View.Property(p => p.RemainQty).ShowInDetail().Readonly();

            View.Property(p => p.Copies).ShowInDetail().UseSpinEditor(e => { e.MinValue = 1;e.AllowDecimals = false; });
            View.Property(p => p.Qty).UseSpinEditor(e => e.MinValue = 0);
        }
    }
}
