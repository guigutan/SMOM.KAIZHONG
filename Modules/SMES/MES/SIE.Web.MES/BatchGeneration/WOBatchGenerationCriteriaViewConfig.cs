using SIE.Barcodes.WipBatchs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MES.BatchGeneration;
using SIE.MES.WorkOrderArchives;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System.Collections.Generic;
using System.Linq;
using SIE.Web.Resources;

namespace SIE.Web.MES.BatchGeneration
{
    /// <summary>
    /// 实体页面配置
    /// </summary>
    internal class WOBatchGenerationCriteriaViewConfig : WebViewConfig<WOBatchGenerationCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No);
                View.Property(p => p.ProCode);
                View.Property(p => p.ProName);
                View.Property(p => p.State).UseEnumMutilEditor(p => p.EnumType = typeof(WorkOrderState));
                View.Property(p => p.BatchNo);
                View.Property(p => p.Factory).UseFactoryEditor().UseListSetting(p => p.HelpInfo = "更改工厂时清空车间").Cascade(p => p.WorkShopId, null);
                View.Property(p => p.WorkShop)
                    .UseDataSource((source, pagingInfo, keyword) =>
                    {
                        var entity = source as WOBatchGenerationCriteria;
                        double? factoryId = entity?.FactoryId;
                        var workshop = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword, factoryId);
                        if (workshop == null || workshop.Count <= 0)
                        {
                            return new EntityList<Enterprise>();
                        }
                        workshop.ForEach(p => p.TreePId = null);
                        return workshop;
                    })
                    .UseListSetting(p => p.HelpInfo = "更改车间时清空资源").Cascade(p => p.ResourceId, null);
                View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                {
                    var workOrder = e as WOBatchGenerationCriteria;
                    if (workOrder == null || workOrder.WorkShop == null)
                        return new EntityList<WipResource>();
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var sourceType = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, workOrder.WorkShopId.Value, sourceType, c, r);
                }).UseListSetting(p => p.HelpInfo = "显示启用状态不失效且企业类型为企业模型的生产资源");
                View.Property(p => p.PlanBeginDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
            }
        }
    }
}