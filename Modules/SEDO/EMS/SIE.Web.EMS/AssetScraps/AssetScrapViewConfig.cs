using SIE.Domain;
using SIE.EMS.AssetScraps;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;
using System.Collections.Generic;

namespace SIE.Web.EMS.AssetScraps
{
    /// <summary>
    /// 资产报废视图配置
    /// </summary>
    public class AssetScrapViewConfig : WebViewConfig<AssetScrap>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.AddBehavior("SIE.Web.EMS.AssetScraps.Behaviors.AssetScrapBehavior");
			View.UseCommand(WebCommandNames.Add);
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.EditAssetRequisitionCommand");
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.DeleteAssetRequisitionCommand");
			View.UseCommand("SIE.Web.EMS.AssetScraps.Commands.SubmitAssetScrapCommand");
			View.UseCommand("SIE.Web.EMS.AssetScraps.Commands.CancelAssetScrapCommand");
			View.UseCommand("SIE.Web.EMS.AssetScraps.Commands.ApprovalAssetScrapCommand");
			View.FormEdit();
			View.Property(p => p.No).ShowInList(width: 120);
			View.Property(p => p.FactoryId).UseFactoryEditor();
			View.Property(p => p.ApprovalStatus);
			View.Property(p => p.AssetObject);
			View.Property(p => p.ManageDeptId);
			View.Property(p => p.UseDeptId);
			View.Property(p => p.WarehouseId);
			View.Property(p => p.IsFixAsset).Readonly();
			View.Property(p => p.Amount);
			View.Property(p => p.ApplicantId);
			View.Property(p => p.ApplyDate).UseDateEditor();
			View.Property(p => p.Remark);

			View.ChildrenProperty(p => p.AssetScrapEquipmentList).HasOrderNo(1).Show(ChildShowInWhere.Hide);

			View.AssociateChildrenProperty(AssetScrap.AssetScrapEquipmentListProperty,
				e =>
				{
					var arg = e as ChildPagingDataWithParentEntityArgs;
					var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<AssetScrap>();

					if (parent == null)
					{
						return new EntityList<AssetScrapEquipment>();
					}
					else
					{
						return RT.Service.Resolve<AssetScrapController>().GetAssetScrapEquipmentList(parent.Id, arg.SortInfo, arg.PagingInfo);
					}
				}).HasLabel("设备清单");

			View.ChildrenProperty(p => p.AssetScrapFixtureList).HasOrderNo(2);
			View.ChildrenProperty(p => p.AssetScrapAttachmentList).HasOrderNo(3);

			View.AttachChildrenProperty(typeof(WorkFlowRecord), e =>
			{
				var args = e as ChildPagingDataArgs;
				var parent = args.Parent as AssetScrap;
				if (parent == null)
				{
					return new EntityList<WorkFlowRecord>();
				}

				return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id,
					typeof(AssetScrap).FullName, args.SortInfo, args.PagingInfo);

			}).HasLabel("审核记录").HasOrderNo(4);
		}

		///<summary>
		/// 配置表单视图
		/// </summary>
		protected override void ConfigDetailsView()
		{
			View.AddBehavior("SIE.Web.EMS.AssetScraps.Behaviors.AssetScrapDetailsBehavior");
			View.UseCommand("SIE.Web.EMS.AssetScraps.Commands.SaveAssetScrapCommand");
			View.RemoveCommands(ConfigCommands.ModuleConfigCommand);
			View.UseDetail(4);
			View.Property(p => p.No).Readonly();
			View.Property(p => p.AssetObject);
			View.Property(p => p.FactoryId).UseFactoryEditor().Cascade(p => p.ManageDeptId, null).Cascade(p => p.UseDeptId, null);
			View.Property(p => p.ManageDeptId).UseUserBussinessDepartmentEditor();
			View.Property(p => p.UseDeptId).UseUserBussinessDepartmentEditor();
			View.Property(p => p.WarehouseId).UseDataSource((e, c, r) =>
			{
				return RT.Service.Resolve<SIE.EMS.Warehouses.WarehouseController>().GetStageLocatgionWarehouses(r, c);
			}).UsePagingLookUpEditor((m, e) =>
			{
				Dictionary<string, string> keyValues = new Dictionary<string, string>();
				keyValues.Add(nameof(e.ScrapLocationCode), nameof(e.ScrapLocationCode));
				keyValues.Add(nameof(e.ScrapLocationId), nameof(e.ScrapLocationId));
				m.DicLinkField = keyValues;
			});
			View.Property(p => p.IsFixAsset);
			View.Property(p => p.Amount).Readonly();
			View.Property(p => p.ApplicantId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            });
            View.Property(p => p.ApplyDate).UseDateEditor();
			View.Property(p => p.Remark).ShowInDetail(columnSpan: 2);

			View.AssociateChildrenProperty(AssetScrap.AssetScrapEquipmentListProperty,
			e =>
			{
				var arg = e as ChildPagingDataWithParentEntityArgs;
				var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<AssetScrap>();

				if (parent == null)
				{
					return new EntityList<AssetScrapEquipment>();
				}
				else
				{
					return RT.Service.Resolve<AssetScrapController>().GetAssetScrapEquipmentList(parent.Id, arg.SortInfo, arg.PagingInfo);
				}
			}, "EditAssetScrapEquipmentViewGroup").HasLabel("设备清单").HasOrderNo(1).Show(ChildShowInWhere.All);

			View.AssociateChildrenProperty(AssetScrap.AssetScrapFixtureListProperty,
			e =>
			{
				var arg = e as ChildPagingDataWithParentEntityArgs;
				var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<AssetScrap>();

				if (parent == null)
				{
					return new EntityList<AssetScrapFixture>();
				}
				else
				{
					return RT.Service.Resolve<AssetScrapController>().GetAssetScrapFixtureList(parent.Id, parent.WarehouseId ?? 0, arg.SortInfo, arg.PagingInfo);
				}
			}, "EditAssetScrapFixtureViewGroup").HasLabel("工治具清单").HasOrderNo(2).Show(ChildShowInWhere.All);

			View.ChildrenProperty(p => p.AssetScrapAttachmentList).HasOrderNo(3).ViewGroup = "EditAssetScrapAttachmentViewGroup";
		}
	}
}
