using SIE.EMS.SpareParts;

namespace SIE.Web.EMS.SpareParts
{
    /// <summary>
    /// 仓库明细查询视图配置
    /// </summary>
    internal class StoreSummaryWarehouseViewConfig : WebViewConfig<StoreSummaryWarehouse>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.WithoutPaging();
            using (View.OrderProperties())
            {
                View.Property(p => p.WarehouseCode).DisableSort();
                View.Property(p => p.WarehouseName).DisableSort();
                View.Property(p => p.LibraryType).DisableSort();
                View.Property(p => p.IsZeroCost).Readonly().DisableSort();
                View.Property(p => p.RotNumber).DisableSort();
                View.Property(p => p.GoodNumber).DisableSort();
                View.Property(p => p.SumNumber).DisableSort();
            }

        }
    }
}
