using SIE.Warehouses;
using SIE.Wpf.Common;

namespace SIE.Wpf.Warehouses
{
    /// <summary>
    /// 库区基本资料 视图配置
    /// </summary>
    internal class StorageLocationInfoViewConfig : WPFViewConfig<StorageLocationInfo>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(StorageLocationItemList.IdProperty);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.Length);
                View.Property(p => p.Width);
                View.Property(p => p.Height);
                View.Property(p => p.StartPointX);
                View.Property(p => p.StartPointY);
                View.Property(p => p.StartPointZ);
                View.Property(p => p.LayerCount);
                View.Property(p => p.Form).UseCatalogEditor(p => p.CatalogType = StorageLocationInfo.LOCATIONFORM);
                View.Property(p => p.IsBonded);
            }
        }
    }
}
