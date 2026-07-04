using SIE.Resources.Enterprises;
using SIE.SO.SaleOrders;
using System.Linq;

namespace SIE.Web.SO.SaleOrders
{
    /// <summary>
    /// 销售订单查询视图
    /// </summary>
    internal class SaleOrderCriteriaViewConfig : WebViewConfig<SaleOrderCriteria>
    {
        /// <summary>
        /// 配置显示字段
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
               // View.Property(p => p.TargetOrderCode).Show(ShowInWhere.All);
                View.Property(p => p.CustomerId).Show(ShowInWhere.All).HasLabel("客户编号");
                View.Property(p => p.LineState).Show(ShowInWhere.All).HasLabel("行状态");
                View.Property(p => p.ItemCode).Show(ShowInWhere.All).HasLabel("物料编码");
                View.Property(p => p.ItemName).Show(ShowInWhere.All).HasLabel("物料名称");
                View.Property(p => p.Employee).Show(ShowInWhere.All).HasLabel("销售人员");
                //View.Property(p => p.Enterprise).Show(ShowInWhere.All).HasLabel("库存组织");
                View.Property(p => p.EnterpriseId).HasLabel("库存组织").UseDataSource((e, p, r) =>
                {
                    var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Plant, p, r);
                    enterprises.ForEach(enterprise => { enterprise.TreePId = null; });
                    return enterprises;
                }).Show(ShowInWhere.All);
                View.Property(p => p.RegisterDateTime).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);
                View.Property(p => p.RequireDelivery).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);
            }
        }
    }
}
