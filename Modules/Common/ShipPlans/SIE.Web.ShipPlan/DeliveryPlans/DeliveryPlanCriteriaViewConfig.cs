using SIE.Core.Enums;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.ShipPlan;
using SIE.Warehouses;
using SIE.Web.Inventory;

namespace SIE.Web.ShipPlan
{
    /// <summary>
    /// 发货计划查询视图配置
    /// </summary>
    internal class DeliveryPlanCriteriaViewConfig : WebViewConfig<DeliveryPlanCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show();
                View.Property(p => p.State).UseEnumMutilEditor(p => p.EnumType = typeof(DeliveryState)).Show();
                View.Property(p => p.OrderType).UseSelectEnumEditor(p =>
                {
                    p.AllowBlank = true;
                    p.ValuesList.Add((int)OrderType.SaleOut);
                    p.ValuesList.Add((int)OrderType.WorkFeed);
                    p.ValuesList.Add((int)OrderType.OutWorkFeed);
                    p.ValuesList.Add((int)OrderType.OutWorkFeedUse);
                    p.ValuesList.Add((int)OrderType.OutAllotReturn);
                    p.ValuesList.Add((int)OrderType.OtherOut);
                    p.ValuesList.Add((int)OrderType.SupplierReturn);
                    p.ValuesList.Add((int)OrderType.DirectAllocate);
                    p.ValuesList.Add((int)OrderType.TwoAllocate);
                    p.ValuesList.Add((int)OrderType.WhTransferOut);
                }).Show();
                View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
                }).Show();
                View.Property(p => p.EnterpriseId).Show();
                View.Property(p => p.CustomerId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<CustomerController>().GetCustomers(pagingInfo, keyword);
                }).Show();
                View.Property(p => p.SupplierId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
                }).Show();
                View.Property(p => p.TargetWarehouseId).UseDataSource((o, e, r) =>
                {
                    var plan = o as DeliveryPlanCriteria;
                    if (plan == null)
                    {
                        return new EntityList<Warehouse>();
                    }
                    return RT.Service.Resolve<WarehouseController>().GetWarehouseByAllInvOrg(e, r, State.Enable);
                }).Show();
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();
                View.Property(p => p.OrderNo).Show();
                View.Property(p => p.DeliveryDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.LastMonth;
                }).Show();
                View.Property(p => p.IsFilter).DefaultValue(1).Show();
            }
        }
    }
}
