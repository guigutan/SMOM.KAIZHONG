using SIE.MES.Workbench.AlertLights;
using SIE.Wpf.MES.Workbench.Editors;

namespace SIE.WPF.MES.Workbench.AlertLights
{
    /// <summary>
    /// 员工呼叫设置视图配置
    /// </summary>
    internal class EmpCallSettingViewConfig : WPFViewConfig<EmpCallSetting>
    {
        /// <summary>
        /// 员工呼叫设置视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.DomainName("员工呼叫设置");
        }

        /// <summary>
        /// 员工呼叫设置列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.UseDefaultCommands();
            View.RemoveCommands(WPFCommandNames.ListSave, WPFCommandNames.Export);
            View.RemoveCommands(WPFCommandNames.Redo, WPFCommandNames.Undo, WPFCommandNames.CustomizeUI);
            View.UseChildrenAsHorizontal();

            using (View.OrderProperties())
            {
                View.Property(p => p.ExceptionType).UseEditor(AbnormalTypeCatalogEditor.EditorName);
                View.Property(p => p.ExceptionType.Name).HasLabel("异常类型名称");
                ////View.Property(p => p.WorkGroup);             
                View.Property(p => p.AlertType);
                View.ChildrenProperty(p => p.InformList).HasOrderNo(10);
            }
        }
    }
}
