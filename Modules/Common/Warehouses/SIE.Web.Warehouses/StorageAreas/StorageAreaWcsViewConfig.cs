using SIE.Warehouses;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 库区操作管理视图配置
    /// </summary>
    internal class StorageAreaWcsViewConfig : WebViewConfig<StorageAreaWcs>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(StorageArea));
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(4);
            using (View.DeclareGroup("深浅库位库存属性要求", 4, false))
            {
                View.Property(p => p.LotRequire1);
                View.Property(p => p.LotRequire2);
                View.Property(p => p.LotRequire3);
                View.Property(p => p.LotRequire4);
            }

        }
    }
}
