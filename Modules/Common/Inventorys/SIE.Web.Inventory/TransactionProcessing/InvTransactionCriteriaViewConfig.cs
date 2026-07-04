using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.Inventory.TransactionProcessing;
using SIE.Inventory.Transactions;
using SIE.Items;
using SIE.Warehouses;

namespace SIE.Web.Inventory.TransactionProcessing
{
    /// <summary>
    /// 库位库存查询 视图配置
    /// </summary>
    internal class InvTransactionCriteriaViewConfig : WebViewConfig<InvTransactionCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.DomainName("事务交易查询");
                View.Property(p => p.BillNo).Show(ShowInWhere.All);
                View.Property(p => p.ItemId).UseDataSource((e, p, s) =>
                {
                    var invTransactionCriteria = e as InvTransactionCriteria;
                    if (invTransactionCriteria == null)
                        return new EntityList<Item>();
                    return RT.Service.Resolve<ItemController>().GetItemDatas(p, s);
                }).UsePagingLookUpEditor().HasLabel("物料编码").Show(ShowInWhere.All);
                View.Property(p => p.Warehouse).UsePagingLookUpEditor().HasLabel("仓库").Show(ShowInWhere.All);
                View.Property(p => p.StorageLocation).UseDataSource((e, p, s) =>
                {
                    var warehouse = (e as InvTransactionCriteria).Warehouse;
                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocations(null, warehouse?.Id, s, p);
                }).UsePagingLookUpEditor().HasLabel("库位").Show(ShowInWhere.All);
                
                View.Property(p => p.LotId).UseDataSource((e, p, s) =>
                {
                    var invTransactionCriteria = e as InvTransactionCriteria;
                    if (invTransactionCriteria == null)
                        return new EntityList<Lot>();
                    return RT.Service.Resolve<LotController>().GetLotDatas(p, s);
                }).UsePagingLookUpEditor().Show(ShowInWhere.All);
              
                View.Property(p => p.Lpn).Show(ShowInWhere.All);
                View.Property(p => p.Customer).HasLabel("货主").Show(ShowInWhere.All);
                View.Property(p => p.ProjectNo).Show(ShowInWhere.All);
                View.Property(p => p.Supplier).HasLabel("相关方").Show(ShowInWhere.All);
                View.Property(p => p.TransactionType).UseEnumEditor(p => p.AllowBlank = true).Show(ShowInWhere.All);
                View.Property(p => p.OrderType).UseEnumEditor(e => e.AllowBlank = true).Show(ShowInWhere.All);
                View.Property(p => p.Transaction).UseDataSource((e, p, s) =>
                {
                    var type = (e as InvTransactionCriteria).OrderType;
                    if (type == null)
                        return new EntityList<Transaction>();
                    return RT.Service.Resolve<TransactionController>().GetTransactions(p, s, type.Value);
                }).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true).Show(ShowInWhere.All);
                View.Property(p => p.CreateBy).Show(ShowInWhere.All);
                View.Property(p => p.TransactionDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.Week;
                }).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).HasLabel("创建时间").UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);
                View.Property(p => p.Id).UseTextEditor().Show(ShowInWhere.All);
            }
        }
    }
}
