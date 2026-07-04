using SIE.Domain;
using SIE.EMS.InventoryPlans;
using SIE.EMS.InventoryTasks;
using SIE.Equipments.DeviceIOTParas.Controllers;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.Common;
using SIE.Web.EMS.DevicePurs;
using SIE.Web.EMS.InventoryPlans.Commands;
using SIE.Web.EMS.InventoryTasks;
using SIE.Web.EMS.WorkFlows;
using SIE.Web.Equipments.DeviceIOTParas.Details;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.InventoryPlans
{
    /// <summary>
    /// 盘点计划视图配置
    /// </summary>
    public class InventoryPlanViewConfig : WebViewConfig<InventoryPlan>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.InventoryPlans.InventoryPlanListBehavior");
            View.UseCommands(WebCommandNames.Add, "SIE.Web.EMS.InventoryPlans.Commands.EditInventoryPlanCommand", typeof(DeleteInventoryPlanCommand).FullName,
                typeof(SubmitInventoryPlanCommand).FullName, typeof(CancelInventoryPlanCommand).FullName, typeof(ExamineInventoryPlanCommand).FullName,
                typeof(ShutdownCommand).FullName);
            //typeof(SeePhotoCommand).FullName
            View.UseClientOrder();
            View.Property(p => p.PlanNo).ShowInList(130);
            View.Property(p => p.FactoryId).ShowInList(120);
            View.Property(p => p.ApprovalStatus).ShowInList(80);
            View.Property(p => p.PercentageString).ShowInList(80);
            View.Property(p => p.InventoryAssetObject).ShowInList(80);
            View.Property(p => p.InventoryType).UseCatalogEditor(e => e.CatalogType = InventoryPlan.InventoryTypeCatalog).ShowInList(80);
            View.Property(p => p.Remark).ShowInList(400);
            View.Property(p => p.PlanEndDate).UseDateEditor().ShowInList(150);
            View.Property(p => p.ResponsibleId).ShowInList(120);
            View.Property(p => p.InventoryExecuteType).ShowInList(80);
            //View.Property(p => p.NeedPhoto).ShowInList(80);
          //  View.Property(p => p.PhotoFilePath).ShowInList(80);
            View.Property(p => p.CloseRemark).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
        /*    View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);*/
            View.AssociateChildrenProperty(InventoryPlanExtEquip.InventoryEquipmentProperty, (e) =>
            {
                var plan = e.Parent as InventoryPlan;
                if (plan == null)
                {
                    return new InventoryPlanEquipment() { InventoryPlan = plan };
                }
                var entity = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlanEquipment(plan.Id);
                if (entity == null)
                {
                    entity = new InventoryPlanEquipment() { InventoryPlan = plan };
                }
                return entity;
            }, DetailsView).HasLabel("盘点范围").OrderNo = 1;

            View.AssociateChildrenProperty(InventoryPlanExtEquip.InventoryPlanSparePartProperty, (e) =>
            {
                var plan = e.Parent as InventoryPlan;
                if (plan == null)
                {
                    return new InventoryPlanSparePart() { InventoryPlan = plan };
                }

                var entity = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlanSparePart(plan.Id);

                if (entity == null)
                {
                    entity = new InventoryPlanSparePart() { InventoryPlan = plan };
                }

                return entity;
            }, DetailsView).HasLabel("盘点范围").OrderNo = 2;

            View.AssociateChildrenProperty(InventoryPlanExtEquip.InventoryPlanFixtureProperty, (e) =>
            {
                var plan = e.Parent as InventoryPlan;
                if (plan == null)
                {
                    return new InventoryPlanFixture() { InventoryPlan = plan };
                }
                var entity = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlanFixture(plan.Id);
                if (entity == null)
                {
                    entity = new InventoryPlanFixture() { InventoryPlan = plan };
                }
                return entity;
            }, DetailsView).HasLabel("盘点范围").OrderNo = 3;

            View.AttachChildrenProperty(typeof(InventoryTask), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<InventoryPlan>();
                if (parent == null)
                {
                    return new EntityList<InventoryTask>();
                }
                return RT.Service.Resolve<InventoryTaskController>().GetInventoryTaskByPlanId(parent.Id, args.SortInfo, args.PagingInfo);
            }, InventoryTaskViewConfig.PlanView).HasLabel("盘点任务").OrderNo = 4;
            View.ChildrenProperty(p => p.InventoryCounterList).HasLabel("盘点人").OrderNo = 5;
            View.ChildrenProperty(p => p.InventoryFixtureCounterList).HasLabel("盘点人").OrderNo = 6;
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<InventoryPlan>();
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(InventoryPlan).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.EmsSeeView).HasLabel("审核记录").OrderNo = 7;
            View.AttachChildrenProperty(typeof(EquipmentList), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<InventoryPlan>();
                if (parent == null)
                    return new EntityList<EquipmentList>();
                var equipList = RT.Service.Resolve<InventoryPlanController>().GetEquipmentLists(parent.Id, args.PagingInfo);
                return equipList;
            }, EquipmentListViewConfig.ListView).HasLabel("设备清单").HasOrderNo(8).Show(ChildShowInWhere.All);

            View.AttachChildrenProperty(typeof(SparePartList), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<InventoryPlan>();
                if (parent == null)
                    return new EntityList<SparePartList>();
                var equipList = RT.Service.Resolve<InventoryPlanController>().GetSpartLists(parent.Id, args.PagingInfo);
                return equipList;
            }, SparePartListViewConfig.ListView).HasLabel("备件清单").HasOrderNo(9).Show(ChildShowInWhere.All);
            //View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            //{
            //    var args = w as ChildPagingDataArgs;
            //    var parent = args.Parent.CastTo<InventoryPlan>();
            //    if (parent == null)
            //    {
            //        return new EntityList<WorkFlowRecord>();
            //    }
            //    return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(InventoryPlan).FullName, args.SortInfo, args.PagingInfo);
            //}, WorkFlowRecordViewConfig.EmsSeeView).HasLabel("审核记录").HasOrderNo(10).Show(ChildShowInWhere.All);

        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.InventoryPlans.InventoryPlanBehavior");
            View.ClearCommands();
            View.UseCommand(typeof(SaveInventoryPlanCommand).FullName);
            View.UseDetail(8);
            View.Property(p => p.PlanNo).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.ApprovalStatus).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.FactoryId).Readonly(p => p.PersistenceStatus != PersistenceStatus.New).UseFactoryEditor().ShowInDetail(columnSpan: 2);
            View.Property(p => p.InventoryAssetObject).ShowInDetail(columnSpan: 2);
            View.Property(p => p.InventoryType).UseCatalogEditor(e => { e.CatalogType = InventoryPlan.InventoryTypeCatalog; e.CatalogReloadData = true; }).ShowInDetail(columnSpan: 2);
            View.Property(p => p.ResponsibleId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            }).ShowInDetail(columnSpan: 2);
            View.Property(p => p.InventoryExecuteType).ShowInDetail(columnSpan: 2);
            //View.Property(p => p.NeedPhoto).ShowInDetail(columnSpan: 2);
            View.Property(p => p.PlanEndDate).UseDateEditor().ShowInDetail(columnSpan: 2);
            View.Property(p => p.Remark).ShowInDetail(columnSpan: 8);
            View.Property(p => p.PhotoFilePath).UseConfigValueEditor(p =>
            {
                p.XType = "uploadinventoryplanphotoeditor";
                p.AllowBlank = true;
                p.Editable = false;
            }).Show(ShowInWhere.Hide);
            View.AssociateChildrenProperty(InventoryPlanExtEquip.InventoryEquipmentProperty, (e) =>
            {
                var plan = e.Parent as InventoryPlan;
                if (plan == null)
                {
                    return new InventoryPlanEquipment() { InventoryPlan = plan };
                }
                var entity = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlanEquipment(plan.Id);
                if (entity == null)
                {
                    entity = new InventoryPlanEquipment() { InventoryPlan = plan };
                }
                return entity;
            }, InventoryPlanEquipmentViewConfig.EditView).Show(ChildShowInWhere.Detail).HasLabel("盘点范围").OrderNo = 1;

            View.AssociateChildrenProperty(InventoryPlanExtEquip.InventoryPlanSparePartProperty, (e) =>
            {
                var plan = e.Parent as InventoryPlan;
                if (plan == null)
                {
                    return new InventoryPlanSparePart() { InventoryPlan = plan };
                }

                var entity = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlanSparePart(plan.Id);

                if (entity == null)
                {
                    entity = new InventoryPlanSparePart() { InventoryPlan = plan };
                }
                return entity;
            }, InventoryPlanSparePartViewConfig.EditView).Show(ChildShowInWhere.Detail).HasLabel("盘点范围").OrderNo = 2;

            View.AssociateChildrenProperty(InventoryPlanExtEquip.InventoryPlanFixtureProperty, (e) =>
            {
                var plan = e.Parent as InventoryPlan;
                if (plan == null)
                {
                    return new InventoryPlanFixture() { InventoryPlan = plan };
                }
                var entity = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlanFixture(plan.Id);
                if (entity == null)
                {
                    entity = new InventoryPlanFixture() { InventoryPlan = plan };
                }
                return entity;
            }, InventoryPlanFixtureViewConfig.EditView).Show(ChildShowInWhere.Detail).HasLabel("盘点范围").OrderNo = 3;


            View.ChildrenProperty(p => p.InventoryCounterList).UseViewGroup(InventoryCounterViewConfig.EditView).HasLabel("盘点人").OrderNo = 4;
            View.ChildrenProperty(p => p.InventoryFixtureCounterList).HasLabel("盘点人").OrderNo = 5;
            View.AttachChildrenProperty(typeof(EquipmentList), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<InventoryPlan>();
                if (parent == null)
                    return new EntityList<EquipmentList>();
                var equipList = RT.Service.Resolve<InventoryPlanController>().GetEquipmentLists(parent.Id, args.PagingInfo);
                return equipList;
            }, EquipmentListViewConfig.ListView).HasLabel("设备清单").HasOrderNo(6).Show(ChildShowInWhere.All);

            View.AttachChildrenProperty(typeof(SparePartList), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<InventoryPlan>();
                if (parent == null)
                    return new EntityList<SparePartList>();
                var equipList = RT.Service.Resolve<InventoryPlanController>().GetSpartLists(parent.Id, args.PagingInfo);
                return equipList;
            }, SparePartListViewConfig.ListView).HasLabel("备件清单").HasOrderNo(7).Show(ChildShowInWhere.All);

        }
    }
}