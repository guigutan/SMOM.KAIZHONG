using SIE.Inventory.Task.ViewModels;

namespace SIE.Web.Inventory.Task.ViewModels
{
    /// <summary>
    /// 发货记录打印 视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class TaskManagementPrintViewModelViewConfig : WebViewConfig<TaskManagementPrintViewModel>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.BillTemplate).Show(ShowInWhere.Detail)
                    .UseBillPrintEditor().HasLabel("打印模板");
            }
        }
    }
}
