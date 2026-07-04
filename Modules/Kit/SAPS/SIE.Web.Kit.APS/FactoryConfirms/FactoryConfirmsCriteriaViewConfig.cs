using SIE.Kit.APS.FactoryConfirms;
using SIE.SO.SaleOrders;
using SIE.Resources.Enterprises;
using SIE.Web.Common;
using System.Linq;

namespace SIE.Web.Pcb.APS.FactoryConfirms
{
    /// <summary>
    /// 查询视图
    /// </summary>
    internal class FactoryConfirmsCriteriaViewConfig : WebViewConfig<FactoryConfirmsViewModelCriteria>
    {
        /// <summary>
        /// 显示
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.EnterpriseId).HasLabel("工厂").UseDataSource((e, p, r) =>
                {
                    var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Plant, p, r);
                    enterprises.ForEach(enterprise => { enterprise.TreePId = null; });
                    return enterprises;
                }).Show(ShowInWhere.All);
                View.Property(p => p.SalesOrderCode).Show(ShowInWhere.All);
                View.Property(p => p.ItemCode).Show(ShowInWhere.All);
                View.Property(p => p.ItemName).Show(ShowInWhere.All);
                View.Property(p => p.IndustryType).UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.INDUSTRYTYPE).Show(ShowInWhere.All);
                View.Property(p => p.OrderType).UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.ORDERTYPE).Show(ShowInWhere.All);
                View.Property(p => p.ProductType).UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.PRODUCTTYPE).Show(ShowInWhere.All);
                View.Property(p => p.IsNew).UseCheckDropDownEditor(p => p.AllowBlank = true).HasLabel("是否新单").Show(ShowInWhere.All);
                View.Property(p => p.LineState).Show(ShowInWhere.All).HasLabel("行状态").DefaultValue((int)LineState.NEW);
                View.Property(p => p.RequireDelivery).UseDateRangeEditor(p =>
                    {
                        p.DateRangeType = ObjectModel.DateRangeType.Month;
                    }).Show(ShowInWhere.All);
            }
        }
    }
}
