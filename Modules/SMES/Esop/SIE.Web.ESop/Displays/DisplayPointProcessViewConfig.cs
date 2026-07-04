using SIE.ESop.Displays;
using SIE.MetaModel.View;
using SIE.Web.ESop.Displays.Commands;

namespace SIE.Web.ESop.Displays
{
    /// <summary>
    /// 工位显示点关系视图配置
    /// </summary>
    internal class DisplayPointProcessViewConfig : WebViewConfig<DisplayPointProcess>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.ClearCommands();
            View.UseCommands(typeof(SelectDisplayPointProcessCommand).FullName, WebCommandNames.Delete);
            View.Property(p => p.Process);
        }
    }
}
