using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Web.Common;
using SIE.Web.EMS.Extensions;
using SIE.Web.EMS.Purchases._Extensions_;
using SIE.Web.Resources;

namespace SIE.Web.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 采购订单查询实体界面
    /// </summary>
    internal class PurchaseOrderCriteriaViewConfig : WebViewConfig<PurchaseOrderCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.No);
            View.Property(p => p.PurchaseCategroy).UseCatalogEditor(e => { e.CatalogType = PurchaseOrder.PurchaseClassify; e.CatalogReloadData = true; });
            View.Property(p => p.PurchaseObjectType).UsePurchaseObjectEditor();
            View.Property(p => p.SupplierId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
            });
            View.Property(p => p.PurchaseOrderStatus);
            View.Property(p => p.ApprovalStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
