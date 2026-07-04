using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.EMS.Purchases.FixtureReceives;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 工治具接受查询视图
    /// </summary>
    public class FixtureReceiveCriteriaViewConfig:WebViewConfig<FixtureReceiveCriteria>
    {
        /// <summary>
        /// 配置查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.DepartmentId).UseUserBussinessDepartmentEditor();
            View.Property(p => p.PurchaseOrderNo).Show();
            View.Property(p => p.ReceiveType).Show();
            View.Property(p => p.Supplier).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<SupplierController>().GetSupplierList(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.Customer).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CustomerController>().GetCustomers(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.ReceiveBillStatus).Show();
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
