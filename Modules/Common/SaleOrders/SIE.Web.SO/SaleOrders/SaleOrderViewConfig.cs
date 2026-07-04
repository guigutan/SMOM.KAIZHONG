using SIE.SO.SaleOrders;
using SIE.Web.SO.SaleOrders.Commands;
using System.Collections.Generic;

namespace SIE.Web.SO.SaleOrders
{
    /// <summary>
    /// 销售订单视图
    /// </summary>
    internal class SaleOrderViewConfig : WebViewConfig<SaleOrder>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(
                typeof(SaleOrderAddCommand).FullName,
                "SIE.Web.SO.SaleOrders.Commands.SaleOrderEditCommand",
                "SIE.Web.SO.SaleOrders.Commands.SaleOrderDeleteCommand",
                typeof(SaleOrderSaveCommand).FullName,
                //WebCommandNames.Save,
                typeof(ImportSaleOrderCommand).FullName,
                "SIE.Web.SO.SaleOrders.Commands.ExportSaleOrderCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly();
                View.Property(p => p.OrderRowsQty).Readonly().DisableSort();
                View.Property(p => p.TotalQty).Readonly().DisableSort();
                View.Property(p => p.RegisterDateTime).UseDateEditor().Readonly(p => p.DetailSum > 0);
                View.Property(p => p.OrderSourceType).Readonly();
                View.Property(p => p.CustomerId).HasLabel("客户编号").UseSalesOrderCustomerEditor()
                        .UsePagingLookUpEditor((m, e) =>
                        {
                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            dic.Add(nameof(e.CustomerName), nameof(e.Customer.ShortName));
                            m.DicLinkField = dic;
                        }).Readonly(p => p.DetailSum > 0);
                View.Property(p => p.CustomerName).HasLabel("客户名称").Readonly(p => p.DetailSum > 0);
                View.Property(p => p.Employee).HasLabel("销售人员").Readonly(p => p.DetailSum > 0);
                #region SGC
                View.Property(p => p.SalesOrganization).Readonly();
                View.Property(p => p.SalesTeam).Readonly();
                View.Property(p => p.CustomerPo).Readonly();
                View.Property(p => p.CustomerPoDate).UseDateEditor().HasLabel("客户下单日期");
                #endregion
                View.Property(p => p.Remark);
            }
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.OrderRowsQty);
            View.Property(p => p.TotalQty);
            View.Property(p => p.RegisterDateTime);
            View.Property(p => p.OrderSourceType);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.OrderRowsQty).Readonly();
            View.Property(p => p.TotalQty).Readonly();
            View.Property(p => p.RegisterDateTime).Readonly();
            View.Property(p => p.OrderSourceType).Readonly();
        }
    }
}
