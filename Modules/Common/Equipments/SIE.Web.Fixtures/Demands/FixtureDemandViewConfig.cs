using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Equipments.WorkFlows;
using SIE.EventMessages.MES.WorkOrders;
using SIE.Fixtures;
using SIE.Fixtures.FixtureDemands;
using SIE.Resources.ProcessSegments;
using SIE.Resources.WipResources;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.Equipments.Extensions;
using SIE.Web.Fixtures.Demands.Commands;
using SIE.Web.Fixtures.WorkFlows;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Demands
{
    /// <summary>
	/// 工治具需求清单视图配置
	/// </summary>
	internal class FixtureDemandViewConfig : WebViewConfig<FixtureDemand>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            var fixtureDemandsConfigValue = RT.Service.Resolve<ElecFixtureController>().GetFixtureDemandsConfigValue();//获取配置项
            View.ClearCommands().UseCommands(ConfigCommands.ModuleConfigCommand);
            View.UseCommands("SIE.Web.Fixtures.Demands.Commands.AddDemandCommand", "SIE.Web.Fixtures.Demands.Commands.EditDemandCommand", "SIE.Web.Fixtures.Demands.Commands.UnLoadCommand",
                typeof(SubmitDemandsCommand).FullName, typeof(ExamineDemandCommand).FullName, typeof(ForcedShutdownCommand).FullName);
            View.AddBehavior("SIE.Web.Fixtures.Demands.Scripts.FixtureDemandBehavior");
            View.Property(p => p.No).Readonly().ShowInList(120);
            View.Property(p => p.WorkShopId).Readonly().HasLabel("车间").ShowInList(100);
            View.Property(p => p.ResourceId).Readonly().HasLabel("产线").ShowInList(80);
            View.Property(p => p.WorkOrderNo).Readonly().HasLabel("工单").ShowInList(120);
            View.Property(p => p.WorkOrderProductCode).Readonly().HasLabel("产品").ShowInList(120);
            View.Property(p => p.ProcessStegment).HasLabel("工段").Readonly().ShowInList(80);
            View.Property(p => p.ProcessSurface).HasLabel("工艺面").Readonly().ShowInList(60);
            View.Property(p => p.DemandState).UseEnumEditor().Readonly().ShowInList(80);
            View.Property(p => p.ReceiveState).UseEnumEditor().Readonly().ShowInList(80);
            View.Property(p => p.Billsource).UseEnumEditor().Readonly().ShowInList(80); 
            View.Property(p => p.Close).Readonly().ShowInList(80);
            View.Property(p => p.DemandTime).Readonly().ShowInList(140);
            if (fixtureDemandsConfigValue != null && fixtureDemandsConfigValue.SwitchApproval)
                View.Property(p => p.ApprovalStatus).UseEnumEditor().Readonly().HasLabel("需求单状态").ShowInList(100);
            View.Property(p => p.CloseRemark).Readonly().ShowInList(120);
            View.ChildrenProperty(p => p.DetailList).Show(ChildShowInWhere.All);
            View.ChildrenProperty(p => p.UnloadList).Show(ChildShowInWhere.All);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as FixtureDemand;
                if (parent == null)
                    return new EntityList<WorkFlowRecord>();
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(FixtureDemand).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.SeeView).HasLabel("审核记录");
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.FormEdit();
            View.UseCommands(typeof(SaveDemandCommand).FullName);
            View.AddBehavior("SIE.Web.Fixtures.Demands.Scripts.AddDemandBehavior");
            View.HasDetailColumnsCount(4);
            View.Property(p => p.No).Readonly();
            View.Property(p => p.WorkShop)
                .UseResourceWorkShopEditor()
                .Cascade(p => p.Resource, null).Cascade(p => p.WorkOrder, null)
                .Cascade(p => p.ProcessSurface, null)
                .Cascade(p => p.WorkOrderProductCode, null)
                .UseListSetting(e => { e.HelpInfo = "更改车间清空产线、工单、产品、工艺面"; }).HasLabel("车间")
                .Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified);
            View.Property(p => p.Resource).UseDataSource((e, c, r) =>
            {
                var demand = e as FixtureDemand;
                if (demand == null || demand.WorkShop == null)
                    return new EntityList<WipResource>();
                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
                return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, demand.WorkShopId, srcTypeList, c, r);
            }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true).Cascade(p => p.WorkOrder, null).Cascade(p => p.WorkOrderProductCode, null)
                .UseListSetting(e => { e.HelpInfo = "显示状态为启用且企业类型为企业模型的生产资源"; }).HasLabel("产线")
                .Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified);
            View.Property(p => p.WorkOrder).UseDataSource((e, c, r) =>
            {
                var demand = e as FixtureDemand;
                if (demand == null || demand.WorkShop == null || demand.Resource == null)
                    return new EntityList<WorkOrder>();
                return RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderList(demand.WorkShopId, demand.ResourceId, c, r,true);
            }).UsePagingLookUpEditor((m, r) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(r.WorkOrderProductCode), nameof(r.WorkOrder.WorkOrderProductCode));
                keyValues.Add(nameof(r.DemandTime), nameof(r.WorkOrder.PlanBeginDate));
                m.DicLinkField = keyValues;
            }).HasLabel("工单")
            .UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true)
            .Cascade(p => p.ProcessSurface, null)
            .Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified);
            View.Property(p => p.WorkOrderProductCode).HasLabel("产品").Readonly();
            View.Property(p => p.ProcessSurface).HasLabel("工艺面").Readonly();
            View.Property(p => p.ProcessStegment).UseDataSource((e, c, r) =>
            {
                var demand = e as FixtureDemand;
                if (demand == null || demand.WorkOrder==null)
                    return new EntityList<ProcessSegment>();
                return RT.Service.Resolve<ElecFixtureController>().GetWoProcessSegment(demand.WorkOrderId,c, r);
            }).HasLabel("工段").Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified);

            View.Property(p => p.DemandTime).UseDateTimeEditor().Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified);
            View.ChildrenProperty(p => p.UnloadList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.DetailList).UseViewGroup(FixtureDemandDetailViewConfig.EditDemandView);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as FixtureDemand;
                if (parent == null)
                    return new EntityList<WorkFlowRecord>();
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(FixtureDemand).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.SeeView).HasLabel("审核记录");
        }
    }
}
