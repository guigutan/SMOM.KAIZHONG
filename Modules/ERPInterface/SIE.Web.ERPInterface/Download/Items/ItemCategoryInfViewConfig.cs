using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.Items
{
    /// <summary>
    /// 分类中间表视图配置
    /// </summary>
    internal class ItemCategoryInfViewConfig : WebViewConfig<ItemCategoryInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Level);
            View.Property(p => p.ParentCode);
            View.Property(p => p.CategoryLevelNum);
            View.Property(p => p.ItemType);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}