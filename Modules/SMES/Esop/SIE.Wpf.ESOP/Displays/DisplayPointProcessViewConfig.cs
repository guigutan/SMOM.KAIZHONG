using SIE.ESop.Displays;
using SIE.Wpf.Command;
using SIE.Wpf.ESOP.Displays.Command;

namespace SIE.Wpf.ESop.Displays
{
    /// <summary>
    /// 工位显示点关系视图配置
    /// </summary>
    internal class DisplayPointProcessViewConfig : WPFViewConfig<DisplayPointProcess>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.ClearCommands().UseCommands(typeof(DisplayPointLookupCommand), typeof(ListDeleteCommand));
            View.Property(p => p.Process);
        }
    }
}