using SIE.EMS.SpareParts;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts
{
    /// <summary>
    /// 库位明细查询视图配置
    /// </summary>
    internal class StoreSummaryLocationViewConfig : WebViewConfig<StoreSummaryLocation>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.WarehouseId).UsePagingLookUpEditor((m, r) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.WarehouseName), nameof(r.Warehouse.Name));
                    keyValues.Add(nameof(r.LibraryType), nameof(r.Warehouse.LibraryType));
                    m.DicLinkField = keyValues;
                });
                View.Property(p => p.WarehouseName);
                View.Property(p => p.LibraryType);
                View.Property(p => p.IsZeroCost).Readonly().DisableSort();
                View.Property(p => p.StorageLocationId).UsePagingLookUpEditor((m, r) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.StorageLocationName), nameof(r.StorageLocation.Name));
                    m.DicLinkField = keyValues;
                });
                View.Property(p => p.StorageLocationName);
                View.Property(p => p.RotNumber);
                View.Property(p => p.GoodNumber);
                View.Property(p => p.SumNumber);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

            }

        }
    }
}
