using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;

namespace SIE.Web.EMS.Purchases.FixtureAcceptances
{
    /// <summary>
    /// 备件验收查询界面
    /// </summary>
    internal class FixtureAcceptanceCriteriaViewConfig : WebViewConfig<FixtureAcceptanceCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.No);
            View.Property(p => p.FixtureEncodeCode);
            View.Property(p => p.SupplierId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
            });
            View.Property(p => p.CustomerId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CustomerController>().GetCustomers(pagingInfo, keyword);
            });
            View.Property(p => p.ApprovalStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
