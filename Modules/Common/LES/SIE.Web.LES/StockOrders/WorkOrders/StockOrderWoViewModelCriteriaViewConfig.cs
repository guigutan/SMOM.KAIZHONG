using SIE.Domain;
using SIE.LES.StockOrders.WorkOrders;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.StockOrders.WorkOrders
{
    /// <summary>
    /// 备料单查询工单视图配置
    /// </summary>
    public class StockOrderWoViewModelCriteriaViewConfig : WebViewConfig<StockOrderWoViewModelCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WoNo).Show();
                View.Property(p => p.Factory).UseDataSource((e, p, k) =>
                {
                    var list = RT.Service.Resolve<EnterpriseController>().GetEmployeeFactoriesList(p, k);
                    foreach (var factory in list)
                    {
                        factory.TreePId = 0;
                    }
                    return list;
                }).Cascade(p => p.WorkshopId, null).Cascade(p => p.WipResourceId, null).Show();
                View.Property(p => p.Workshop).UseDataSource((e, p, k) =>
                {
                    var entity = e as StockOrderWoViewModelCriteria;
                    var list = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(p, k, entity.FactoryId);
                    foreach (var workshop in list)
                    {
                        workshop.TreePId = 0;
                    }
                    return list;
                }).Cascade(p => p.WipResourceId, null).Show();
                View.Property(p => p.WipResource).UseDataSource((e, p, k) =>
                {
                    var entity = e as StockOrderWoViewModelCriteria;
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
                    if (entity.WorkshopId.HasValue)
                    {
                        return RT.Service.Resolve<WipResourceController>()
                            .GetWipResourcesByWorkShopId(stateList, new List<double?> { entity.WorkshopId.Value },
                            srcTypeList, p, k);
                    }

                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, entity.WorkshopId, srcTypeList, p, k);
                }).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
            }
        }
    }
}
