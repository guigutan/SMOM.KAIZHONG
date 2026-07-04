using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.EMS.Purchases.EquipmentReceives;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;

namespace SIE.Web.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 设备接收查询界面
    /// </summary>
    internal class EquipmentReceiveCriteriaViewConfig : WebViewConfig<EquipmentReceiveCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.PurchaseOrderNo);
            View.Property(p => p.ReceiveType);
            View.Property(p => p.SupplierId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
            });
            View.Property(p => p.CustomerId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CustomerController>().GetCustomers(pagingInfo, keyword);
            });
            View.Property(p => p.ReceiveBillStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
