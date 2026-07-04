using SIE.Warehouses;
using SIE.Web.Common;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 库位基本资料 视图配置
    /// </summary>
    internal class StorageLocationInfoViewConfig : WebViewConfig<StorageLocationInfo>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.Warehouses.ViewBehaviors.StorageLocationInfoBehavior");
            View.HasDetailColumnsCount(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.Length);
                View.Property(p => p.Width);
                View.Property(p => p.Height);
                View.Property(p => p.StartPointX);
                View.Property(p => p.StartPointY);
                View.Property(p => p.StartPointZ);
                View.Property(p => p.LayerCount).DefaultValue(1);
                View.Property(p => p.Form).UseCatalogEditor(p => { p.CatalogType = StorageLocationInfo.LOCATIONFORM; p.CatalogReloadData = true; })
                    .UseListSetting(e => { e.HelpInfo = "库位形式快码类型(LOCATION_FORM)"; });
                View.Property(p => p.IsBonded);
            }
        }
    }
}
