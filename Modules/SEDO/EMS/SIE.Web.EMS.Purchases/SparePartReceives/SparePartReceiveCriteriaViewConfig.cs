using SIE.CSM.Suppliers;
using SIE.EMS.Purchases.SparePartReceives;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;

namespace SIE.Web.EMS.Purchases.SparePartReceives
{
    /// <summary>
    /// 备件接收查询界面
    /// </summary>
    internal class SparePartReceiveCriteriaViewConfig : WebViewConfig<SparePartReceiveCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.PurchaseOrderNo);
            View.Property(p => p.ReceiveType).UseEnumEditor(p => p.FilterCategoery = "SparePartReceive");
            View.Property(p => p.SupplierId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
            });
            View.Property(p => p.ReceiveBillStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
