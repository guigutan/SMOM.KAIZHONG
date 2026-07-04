using SIE.EMS.SpareParts;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts
{
    /// <summary>
    /// 序列号明细查询视图配置
    /// </summary>
    public class StoreSummaryDetailViewConfig : WebViewConfig<StoreSummaryDetail>
    {
        /// <summary>
        /// 备件清单添加视图
        /// </summary>
        public const string AddStoreSummaryDetailViewGroup = "AddStoreSummaryDetailViewGroup";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { AddStoreSummaryDetailViewGroup });

            if (ViewGroup == AddStoreSummaryDetailViewGroup)
            {
                ConfigAddStoreSummaryDetailView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.OrderNumberCode);
                View.Property(p => p.WarehouseId).UsePagingLookUpEditor((m, r) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.WarehouseName), nameof(r.Warehouse.Name));
                    m.DicLinkField = keyValues;
                });
                View.Property(p => p.WarehouseName);
                View.Property(p => p.StorageLocationId).UsePagingLookUpEditor((m, r) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.StorageLocationName), nameof(r.StorageLocation.Name));
                    m.DicLinkField = keyValues;
                });
                View.Property(p => p.StorageLocationName);
                View.Property(p => p.OdNbStatus);
                View.Property(p => p.StoreStatus);
                View.Property(p => p.FixedAssetsAccountCode);
                View.Property(p => p.FixedAssetsAccountName);
                View.Property(p => p.StoreCode);
                View.Property(p => p.InboundDate).UseDateEditor();
                View.Property(p => p.SupplierId).UsePagingLookUpEditor((m, r) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(r.SupplierName), nameof(r.Supplier.Name));
                    m.DicLinkField = keyValues;
                });
                View.Property(p => p.SupplierName);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected void ConfigAddStoreSummaryDetailView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.OrderNumberCode).Show();
                View.Property(p => p.SparePartCode).Show();
                View.Property(p => p.SparePartName).Show();
                View.Property(p => p.State).Show();
                View.Property(p => p.Specification).Show();
                View.Property(p => p.SpartType).Show();
                View.Property(p => p.UnitName).Show();
                View.Property(p => p.WarehouseName).Show();
                View.Property(p => p.StorageLocationName).Show();
                View.Property(p => p.SupplierCode).Show();
                View.Property(p => p.SupplierName).Show();
                View.Property(p => p.CreateByName).Show();
                View.Property(p => p.CreateDate).Show();
                View.Property(p => p.UpdateByName).Show();
                View.Property(p => p.UpdateDate).Show();
            }
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.OrderNumberCode);
        }
    }
}