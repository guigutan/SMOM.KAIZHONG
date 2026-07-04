using SIE.Items;
using SIE.Web.ERPInterface.DownloadManual.Items.Commands;

namespace SIE.Web.ERPInterface.DownloadManual.Items
{
    /// <summary>
    /// 单位转换扩展视图
    /// </summary>
    public class ItemUnitExtViewConfig : WebViewConfig<ItemUnit>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(DlUnitChangeCommand).FullName);
        }
    }
}
