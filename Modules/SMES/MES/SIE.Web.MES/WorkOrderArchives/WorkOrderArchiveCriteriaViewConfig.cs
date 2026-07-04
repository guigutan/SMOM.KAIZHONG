using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MES.WorkOrderArchives;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案查询实体视图配置
    /// </summary>
    public class WorkOrderArchiveCriteriaViewConfig : WebViewConfig<WorkOrderArchiveCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No);
                View.Property(p => p.ProCode);
                View.Property(p => p.ProName);
                View.Property(p => p.State).UseEnumMutilEditor(p => p.EnumType = typeof(WorkOrderState));
                View.Property(p => p.BarCode);
                View.Property(p => p.BatchNo);
                View.Property(p => p.Factory).UseFactoryEditor().UseListSetting(p => p.HelpInfo = "更改工厂时清空车间").Cascade(p => p.WorkShopId, null);
                View.Property(p => p.WorkShop)
                    .UseDataSource((source, pagingInfo, keyword) =>
                    {
                        var entity = source as WorkOrderArchiveCriteria;
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
                    var workOrder = e as WorkOrderArchiveCriteria;
                    if (workOrder == null || workOrder.WorkShop == null)
                        return new EntityList<WipResource>();
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var sourceType = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, workOrder.WorkShopId.Value, sourceType, c, r);
                }).UseListSetting(p => p.HelpInfo = "显示启用状态不失效且企业类型为企业模型的生产资源");
                View.Property(p => p.PlanBeginDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
                View.Property(p => p.ActuFinishDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
            }
        }
    }
}
