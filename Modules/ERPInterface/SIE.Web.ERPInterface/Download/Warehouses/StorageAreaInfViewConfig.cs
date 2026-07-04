using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.Warehouses
{
    /// <summary>
    /// 库区中间表视图配置
    /// </summary>
    internal class StorageAreaInfViewConfig : WebViewConfig<StorageAreaInf>
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
            View.Property(p => p.WarehouseCode);
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