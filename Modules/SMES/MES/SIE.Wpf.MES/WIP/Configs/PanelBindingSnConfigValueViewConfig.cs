using SIE.MES.WIP.Configs;
using SIE.Wpf.Common.Editors;

namespace SIE.Wpf.MES.WIP.Configs
{
    /// <summary>
    /// 拼板码绑定的SN条码来源视图配置
    /// </summary>
    internal class PanelBindingSnConfigValueViewConfig : WPFViewConfig<PanelBindingSnConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsGenerateSn);
            View.Property(p => p.Printer).Show(ShowInWhere.All).UseEditor(PrinterEditor.EditorName);
        }
    }
}
