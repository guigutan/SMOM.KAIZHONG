using SIE.CSM.Suppliers;
using SIE.EMS.Purchases.PaymentPlans;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;

namespace SIE.Web.EMS.Purchases.PaymentPlans
{
    /// <summary>
    /// 付款计划查询界面
    /// </summary>
    internal class PaymentPlanCriteriaViewConfig : WebViewConfig<PaymentPlanCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.No).HasLabel("付款计划单号");
            View.Property(p => p.PurchaseOrderNo);
            View.Property(p => p.SupplierId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
            });
            View.Property(p => p.ApprovalStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
