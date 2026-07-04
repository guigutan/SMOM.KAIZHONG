using SIE.MES.WIP.Configs;
using SIE.Wpf.Common.Editors;

namespace SIE.WPF.MES.WIP.Configs
{
    /// <summary>
    /// 是否打印标签配置值视图配置
    /// </summary>
    internal class PrintLabelConfigValueViewConfig : WPFViewConfig<PrintLabelConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.IsPrint).Show(ShowInWhere.All);
                View.Property(p => p.Printer).Show(ShowInWhere.All)
                    .UseEditor(PrinterEditor.EditorName);
            }
        }
    }
}
