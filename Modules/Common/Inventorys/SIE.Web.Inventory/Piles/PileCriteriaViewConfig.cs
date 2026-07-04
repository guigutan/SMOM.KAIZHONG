using SIE.Inventory.Piles;
using SIE.Warehouses;

namespace SIE.Web.Inventory.Pile
{
    /// <summary>
    /// 垛表查询视图
    /// </summary>
    public class PileCriteriaViewConfig : WebViewConfig<PileCriteria>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Model).Show();
                View.Property(p => p.PileState).Show();
                View.Property(p => p.TurnoverContainer).UseCheckDropDownEditor().Show();
                View.Property(p => p.BillNo).Show();
                View.Property(p => p.CurLocation).Show();
                View.Property(p => p.Warehouse).UsePagingLookUpEditor(p =>
                {
                    p.SearchFieldList.Add(Warehouse.CodeProperty.Name);
                    p.SearchFieldList.Add(Warehouse.NameProperty.Name);
                }).Show();
                View.Property(p => p.StorageArea).UsePagingLookUpEditor(p =>
                {
                    p.SearchFieldList.Add(StorageArea.CodeProperty.Name);
                    p.SearchFieldList.Add(StorageArea.NameProperty.Name);
                }).Show();
                View.Property(p => p.StorageLocation).UsePagingLookUpEditor(p =>
                {
                    p.SearchFieldList.Add(StorageLocation.CodeProperty.Name);
                    p.SearchFieldList.Add(StorageLocation.NameProperty.Name);
                }).Show();
                View.Property(p => p.ItemState).Show();
                View.Property(p => p.Item).Show();
                View.Property(p => p.Lot).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
            }
        }
    }
}
