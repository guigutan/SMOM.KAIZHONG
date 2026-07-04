using SIE.MES.Workbench.AlertLights;

namespace SIE.WPF.MES.Workbench.AlertLights
{
    /// <summary>
    /// 员工呼叫通知视图配置
    /// </summary>
    internal class EmpCallInformViewConfig : WPFViewConfig<EmpCallInform>
    {
        /// <summary>
        /// 员工呼叫通知视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.DomainName("员工呼叫通知");
        }

        /// <summary>
        /// 员工呼叫通知列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.UseDefaultCommands();
            View.RemoveCommands(WPFCommandNames.ListSave, WPFCommandNames.Export);
            View.RemoveCommands(WPFCommandNames.Redo, WPFCommandNames.Undo, WPFCommandNames.CustomizeUI);

            using (View.OrderProperties())
            {
                View.Property(p => p.Employee);
            }
        }
    }
}
