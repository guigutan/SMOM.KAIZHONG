using SIE.MES.TaskManagement.Dispatchs.ViewModels;

namespace SIE.Web.MES.TaskManagement.Dispatchs.ViewModels
{
    /// <summary>
    /// 任务单打印 视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class DispatchTaskPrintViewModelViewConfig : WebViewConfig<DispatchTaskPrintViewModel>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.BillTemplate).Show(ShowInWhere.Detail).UseDispatchTaskBillEditor().HasLabel("打印模板");
            }
        }
    }
}
