using SIE.Domain;
using SIE.EMS.AssetRequisitions;
using SIE.EMS.AssetReturns;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.Resources;
using System.Collections.Generic;

namespace SIE.Web.EMS.AssetReturns
{
    /// <summary>
    /// 资产归还视图配置
    /// </summary>
    public class AssetReturnViewConfig : WebViewConfig<AssetReturn>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
            View.AddBehavior("SIE.Web.EMS.AssetReturns.Behaviors.AssetReturnBehavior");
			View.UseCommand(WebCommandNames.Add);
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.EditAssetRequisitionCommand");
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.DeleteAssetRequisitionCommand");
			View.UseCommand("SIE.Web.EMS.AssetReturns.Commands.SubmitAssetReturnCommand");
            View.UseCommand("SIE.Web.EMS.AssetReturns.Commands.CancelAssetReturnCommand");
            View.UseCommand("SIE.Web.EMS.AssetReturns.Commands.ApprovalAssetReturnCommand");
            View.FormEdit();
			View.Property(p => p.ReturnNo).ShowInList(width: 120);
			View.Property(p => p.FactoryId).UseFactoryEditor();
			View.Property(p => p.ApprovalStatus);
			View.Property(p => p.AssetRequisitionId).ShowInList(width: 120);
			View.Property(p => p.AssetObject);
			View.Property(p => p.ApplyDepartmentId);
			View.Property(p => p.LendingDepartmentId);
			View.Property(p => p.WarehouseId);
			View.Property(p => p.EmployeeId);
			View.Property(p => p.External).Readonly();
			View.Property(p => p.ApplyDate).UseDateEditor();
            View.Property(p => p.Remark);

			View.ChildrenProperty(p => p.AssetReturnEquipmentList).HasOrderNo(1);
			View.ChildrenProperty(p => p.AssetReturnFixtureList).HasOrderNo(2);
			View.ChildrenProperty(p => p.AssetReturnAttachmentList).HasOrderNo(3);

            View.AttachChildrenProperty(typeof(WorkFlowRecord), e =>
            {
                var args = e as ChildPagingDataArgs;
                var parent = args.Parent as AssetReturn;
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }

                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id,
                    typeof(AssetReturn).FullName, args.SortInfo, args.PagingInfo);

            }).HasLabel("审核记录").HasOrderNo(4);
        }

        ///<summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.AssetReturns.Behaviors.AssetReturnDetailsBehavior");
            View.UseCommand("SIE.Web.EMS.AssetReturns.Commands.SaveAssetReturnCommand");
            View.RemoveCommands(ConfigCommands.ModuleConfigCommand);
            View.UseDetail(8);
            View.Property(p => p.ReturnNo).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.FactoryId).UseFactoryEditor().ShowInDetail(columnSpan: 2)
                .Cascade(p => p.AssetRequisitionId, null).Cascade(p => p.AssetObject, null).Cascade(p => p.ApplyDepartmentId, null)
                .Cascade(p => p.LendingDepartmentId, null).Cascade(p => p.WarehouseId, null).Cascade(p => p.External, null)
                .Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.AssetRequisitionId).UseDataSource((e, c, r) =>
            {
                var entity = e as AssetReturn;
                return RT.Service.Resolve<AssetRequisitionController>().GetAssetRequisitionsForReturn(entity.FactoryId, c, r);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.AssetObject), nameof(e.AssetRequisition.AssetObject));
                keyValues.Add(nameof(e.ApplyDepartmentId), nameof(e.AssetRequisition.ApplyDepartmentId));
                keyValues.Add("ApplyDepartmentId_Display", nameof(e.AssetRequisition.ApplyDepartmentName));
                keyValues.Add(nameof(e.LendingDepartmentId), nameof(e.AssetRequisition.LendingDepartmentId));
                keyValues.Add("LendingDepartmentId_Display", nameof(e.AssetRequisition.LendingDepartmentName));
                keyValues.Add(nameof(e.WarehouseId), nameof(e.AssetRequisition.WarehouseId));
                keyValues.Add("WarehouseId_Display", nameof(e.AssetRequisition.WarehouseCode));
                keyValues.Add(nameof(e.External), nameof(e.AssetRequisition.External));
                m.DicLinkField = keyValues;
            }).Readonly(p => p.PersistenceStatus != PersistenceStatus.New).ShowInDetail(columnSpan: 2);
            
            View.Property(p => p.AssetObject).Readonly().ShowInDetail(columnSpan: 2).ShowInDetail(columnSpan: 2);
            View.Property(p => p.ApplyDepartmentId).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.LendingDepartmentId).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.WarehouseId).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.EmployeeId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            }).ShowInDetail(columnSpan: 2);
            View.Property(p => p.External).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.ApplyDate).UseDateEditor().ShowInDetail(columnSpan: 2);
            View.Property(p => p.Remark).ShowInDetail(columnSpan: 4).Readonly();

            View.AssociateChildrenProperty(AssetReturn.AssetReturnEquipmentListProperty,e =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<AssetReturn>();

                if (parent == null)
                {
                    return new EntityList<AssetReturnEquipment>();
                }
                else
                {
                    var returnEquiplist = RT.Service.Resolve<AssetReturnController>().GetAssetReturnEquipmentsById(parent.Id, parent.AssetRequisitionId);
                    foreach (var item in returnEquiplist)
                    {
                        item.FactoryId = parent.FactoryId;
                    }
                    return returnEquiplist;
                }
            }, "EditAssetReturnEquipmentViewGroup", false).HasLabel("设备清单").HasOrderNo(1).Show(ChildShowInWhere.All);

            View.AssociateChildrenProperty(AssetReturn.AssetReturnFixtureListProperty,e =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<AssetReturn>();

                if (parent == null)
                {
                    return new EntityList<AssetReturnFixture>();
                }
                else
                {
                    var returnFixturelist = RT.Service.Resolve<AssetReturnController>().GetAssetReturnFixturesById(parent.Id, parent.AssetRequisitionId);
                    return returnFixturelist;
                }
            }, "EditAssetReturnFixtureViewGroup", false).HasLabel("工治具清单").HasOrderNo(2).Show(ChildShowInWhere.All);

            View.ChildrenProperty(p => p.AssetReturnAttachmentList).HasOrderNo(3).Show(ChildShowInWhere.All).ViewGroup = "EditAssetReturnAttachmentViewGroup";

            View.AttachChildrenProperty(typeof(AssetReturnEquipment), (e) =>
            {
                var args = e as ChildPagingDataWithParentEntityArgs;
                AssetReturn entity = null;

                if (args.ParentEntity != null)
                {
                    entity = args.ParentEntity.ToJsonObject<AssetReturn>();
                }
                else 
                {
                    entity = RF.GetById<AssetReturn>(args.Parent.GetId());
                }

                return RT.Service.Resolve<AssetReturnController>().GetExistAssetReturnEquipmentsById(args.SortInfo, null, entity.Id, entity.AssetRequisitionId);

            }, "ExistAssetReturnEquipmentViewGroup").HasLabel("已归还清单").HasOrderNo(4).Show(ChildShowInWhere.All);

            View.AttachChildrenProperty(typeof(AssetReturnFixture), (e) =>
            {
                var args = e as ChildPagingDataWithParentEntityArgs;
                AssetReturn entity = null;

                if (args.ParentEntity != null)
                {
                    entity = args.ParentEntity.ToJsonObject<AssetReturn>();
                }
                else
                {
                    entity = RF.GetById<AssetReturn>(args.Parent.GetId());
                }

                return RT.Service.Resolve<AssetReturnController>().GetExistAssetReturnFixturesById(args.SortInfo, null, entity.Id, entity.AssetRequisitionId);
            }, "ExistAssetReturnFixtureViewGroup").HasLabel("已归还清单").HasOrderNo(4).Show(ChildShowInWhere.All);
        }
    }
}
