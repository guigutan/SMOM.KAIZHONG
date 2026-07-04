using SIE.Kit.APS.EngineerPlans;
using SIE.Resources.Enterprises;
using SIE.SO.SaleOrders;
using SIE.Web.Common;
using SIE.Web.Resources;
using System.Linq;

namespace SIE.Web.Kit.APS.EngineerPlans
{
    /// <summary>
    /// 查询 视图
    /// </summary>
    public class EngineerPlanCriteriaViewConfig : WebViewConfig<EngineerPlanCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryId).UseFactoryEditor().Show(ShowInWhere.All);
                View.Property(p => p.SaleOrderNo).Show(ShowInWhere.All);
                View.Property(p => p.LineNo).Show(ShowInWhere.All);
                View.Property(p => p.ItemId).HasLabel("生产型号").Show(ShowInWhere.All);
                View.Property(p => p.ItemName).HasLabel("物料名称").Show(ShowInWhere.All);
                View.Property(p => p.CustomerCode).Show(ShowInWhere.All);
                View.Property(p => p.CustomerName).Show(ShowInWhere.All);

                View.Property(p => p.OrderType).UseListSetting(e => { e.HelpInfo = "订单类型快码类型(ORDER_TYPE)"; })
                        .UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.ORDERTYPE).Show(ShowInWhere.List);
                View.Property(p => p.PlanState).Show(ShowInWhere.All);

                View.Property(p => p.ScheduleDay).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.Week;
                    p.DateFormat = "Y年m月d日";
                }).Show(ShowInWhere.All);
                View.Property(p => p.SortDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.Week;
                    p.DateFormat = "Y年m月d日";
                }).Show(ShowInWhere.All);
                View.Property(p => p.RequireDelivery).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                    p.DateFormat = "Y年m月d日";
                }).Show(ShowInWhere.All);


            }
        }
    }
}
