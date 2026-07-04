using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.EMS.Purchases.EquipmentInbounds;
using SIE.Equipments.EquipModels;

namespace SIE.Web.EMS.Purchases.EquipmentInbounds
{
    /// <summary>
    /// 设备入库查询实体界面
    /// </summary>
    internal class EquipmentInboundCriteriaViewConfig : WebViewConfig<EquipmentInboundCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
            View.Property(p => p.InboundType);
            View.Property(p => p.EquipmentCode);
            View.Property(p => p.EquipModelId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EquipModelController>().GetEquipModels(pagingInfo, keyword);
            });
            View.Property(p => p.PurchaseOrderNo);
            View.Property(p => p.AcceptanceNo);
            View.Property(p => p.SupplierId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
            });
            View.Property(p => p.CustomerId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CustomerController>().GetCustomers(pagingInfo, keyword);
            });
            View.Property(p => p.InboundStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
