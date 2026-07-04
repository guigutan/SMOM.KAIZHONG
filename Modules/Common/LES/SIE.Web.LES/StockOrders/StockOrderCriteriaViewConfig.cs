using SIE.Domain;
using SIE.LES.StockOrders;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System.Linq;

namespace SIE.Web.LES.StockOrders
{
    /// <summary>
    /// 备料单查询实体配置视图
    /// </summary>
    public class StockOrderCriteriaViewConfig : WebViewConfig<StockOrderCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show();
                View.Property(p => p.WorkOrderNo).Show();
                View.Property(p => p.PlanBeginTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.WorkShopId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var workshop = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword);
                    if (workshop == null || workshop.Count <= 0)
                        return new EntityList<Enterprise>();
                    workshop.ForEach(p => p.TreePId = null);
                    return workshop;
                }).Show();
                View.Property(p => p.ResourceId).UsePagingLookUpEditor((m, r) => 
                {
                    m.DisplayField = WipResource.NameProperty.Name;
                    m.BindDisplayField = WipResource.NameProperty.Name;
                }).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(RT.IdentityId, c, r);
                }).Show();
                View.Property(p => p.StockState).UseEnumMutilEditor(p => p.EnumType = typeof(StockState)).Show();
                View.Property(p => p.BillSource).Show();
                View.Property(p => p.StockType).Show();
                View.Property(p => p.TriggerMode).Show();
                View.Property(p => p.DemandMode).Show();
                View.Property(p => p.ItemCode).Show(); 
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
            }
        }
    }
}