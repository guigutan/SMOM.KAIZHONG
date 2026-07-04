using SIE.Domain;
using SIE.EMS.AssetIssues;
using SIE.EMS.AssetRequisitions;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.Resources;
using System.Collections.Generic;

namespace SIE.Web.EMS.AssetIssues
{
	/// <summary>
	/// 资产发放视图配置
	/// </summary>
	public class AssetIssueViewConfig : WebViewConfig<AssetIssue>
	{
		/// <summary>
		/// 外部领用信息查看视图
		/// </summary>
		public const string IssueExternalInfoViewGroup = "IssueExternalInfoViewGroup";

		/// <summary>
		/// 外部领用信息编辑视图
		/// </summary>
		public const string EditIssueExternalInfoViewGroup = "EditIssueExternalInfoViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { IssueExternalInfoViewGroup, EditIssueExternalInfoViewGroup });

			if (ViewGroup == IssueExternalInfoViewGroup)
			{
				ConfigIssueExternalInfoView();
			}

			if (ViewGroup == EditIssueExternalInfoViewGroup)
			{
				ConfigEditIssueExternalInfoView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.AddBehavior("SIE.Web.EMS.Common.Script.ApprovalBehavior");
			View.AddBehavior("SIE.Web.EMS.AssetIssues.Behaviors.AssetIssueBehavior");
			View.UseCommand(WebCommandNames.Add);
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.EditAssetRequisitionCommand");
			View.UseCommand("SIE.Web.EMS.AssetIssues.Commands.DeleteAssetIssueCommand");
			View.UseCommand("SIE.Web.EMS.AssetIssues.Commands.SubmitAssetIssueCommand");
			View.UseCommand("SIE.Web.EMS.AssetIssues.Commands.CancelAssetIssueCommand");
			View.UseCommand("SIE.Web.EMS.AssetIssues.Commands.ApprovalAssetIssueCommand");
			View.FormEdit();
			View.Property(p => p.IssueNo).ShowInList(width:120);
			View.Property(p => p.FactoryId).UseFactoryEditor();
			View.Property(p => p.RequisitionNo).ShowInList(width: 120);
			View.Property(p => p.ApprovalStatus);
			View.Property(p => p.RequisitionType);
			View.Property(p => p.AssetObject);
			View.Property(p => p.ApplyDepartmentId);
			View.Property(p => p.LendingDepartmentId);
			View.Property(p => p.WarehouseId);
			View.Property(p => p.External).Readonly();
			View.Property(p => p.Remark);

			View.ChildrenProperty(p => p.AssetIssueEquipmentList).HasOrderNo(1);
			View.ChildrenProperty(p => p.AssetIssueFixtureList).HasOrderNo(2);
			View.AttachDetailChildrenProperty(typeof(AssetIssue), c =>
			{
				AssetIssue entity = c.Parent as AssetIssue;
				return RF.GetById<AssetIssue>(entity.Id, new EagerLoadOptions().LoadWithViewProperty());
			}, IssueExternalInfoViewGroup, null).HasLabel("外部领用信息").HasOrderNo(3);

			View.AttachChildrenProperty(typeof(WorkFlowRecord), e =>
			{
				var args = e as ChildPagingDataArgs;
				var parent = args.Parent as AssetIssue;
				if (parent == null)
				{
					return new EntityList<WorkFlowRecord>();
				}

				return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id,
					typeof(AssetIssue).FullName, args.SortInfo, args.PagingInfo);

			}).HasLabel("审核记录").HasOrderNo(4);
		}

		///<summary>
		/// 配置表单视图
		/// </summary>
		protected override void ConfigDetailsView()
		{
			View.AddBehavior("SIE.Web.EMS.AssetIssues.Behaviors.AssetIssueDetailsBehavior");
			View.UseCommand("SIE.Web.EMS.AssetIssues.Commands.SaveAssetIssueCommand");
			View.RemoveCommands(ConfigCommands.ModuleConfigCommand);
			View.UseDetail(4);
			View.Property(p => p.IssueNo).Readonly();
			View.Property(p => p.AssetRequisitionId).UseDataSource((e, c, r) =>
			{
				return RT.Service.Resolve<AssetRequisitionController>().GetAssetRequisitionsForIssue(c, r);
			}).UsePagingLookUpEditor((m, e) =>
			{
				Dictionary<string, string> keyValues = new Dictionary<string, string>();
				keyValues.Add(nameof(e.FactoryId), nameof(e.AssetRequisition.FactoryId));
				keyValues.Add("FactoryId_Display", nameof(e.AssetRequisition.FactoryName));
				keyValues.Add(nameof(e.RequisitionType), nameof(e.AssetRequisition.RequisitionType));
				keyValues.Add(nameof(e.AssetObject), nameof(e.AssetRequisition.AssetObject));
				keyValues.Add(nameof(e.ApplyDepartmentId), nameof(e.AssetRequisition.ApplyDepartmentId));
				keyValues.Add("ApplyDepartmentId_Display", nameof(e.AssetRequisition.ApplyDepartmentName));
				keyValues.Add(nameof(e.LendingDepartmentId), nameof(e.AssetRequisition.LendingDepartmentId));
				keyValues.Add("LendingDepartmentId_Display", nameof(e.AssetRequisition.LendingDepartmentName));
				keyValues.Add(nameof(e.WarehouseId), nameof(e.AssetRequisition.WarehouseId));
				keyValues.Add("WarehouseId_Display", nameof(e.AssetRequisition.WarehouseCode));
				keyValues.Add(nameof(e.External), nameof(e.AssetRequisition.External));
				keyValues.Add(nameof(e.Remark), nameof(e.AssetRequisition.Remark));
				keyValues.Add(nameof(e.ExternalType), nameof(e.AssetRequisition.ExternalType));
				keyValues.Add(nameof(e.SupplierCode), nameof(e.AssetRequisition.SupplierCode));
				keyValues.Add(nameof(e.SupplierName), nameof(e.AssetRequisition.SupplierName));
				keyValues.Add(nameof(e.CustomerCode), nameof(e.AssetRequisition.CustomerCode));
				keyValues.Add(nameof(e.CustomerName), nameof(e.AssetRequisition.CustomerName));
				keyValues.Add(nameof(e.Destination), nameof(e.AssetRequisition.Destination));
				keyValues.Add(nameof(e.ContactPerson), nameof(e.AssetRequisition.ContactPerson));
				keyValues.Add(nameof(e.ContactInformation), nameof(e.AssetRequisition.ContactInformation));
				keyValues.Add(nameof(e.Address), nameof(e.AssetRequisition.Address));
				keyValues.Add(nameof(e.DeliveryWay), nameof(e.AssetRequisition.DeliveryWay));
				keyValues.Add(nameof(e.TrackNo), nameof(e.AssetRequisition.TrackNo));
				m.DicLinkField = keyValues;
			}).Readonly(p=>p.PersistenceStatus != PersistenceStatus.New).HasLabel("领用单号");
			View.Property(p => p.FactoryId).Readonly();
			View.Property(p => p.RequisitionType).Readonly();
			View.Property(p => p.AssetObject).Readonly();
			View.Property(p => p.ApplyDepartmentId).Readonly();
			View.Property(p => p.LendingDepartmentId).Readonly();
			View.Property(p => p.WarehouseId).Readonly();
			View.Property(p => p.External).Readonly();
			View.Property(p => p.Remark).ShowInDetail(columnSpan: 2).Readonly();

			View.AssociateChildrenProperty(AssetIssue.AssetIssueEquipmentListProperty,
            e =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<AssetIssue>();

                if (parent == null)
                {
                    return new EntityList<AssetIssueEquipment>();
                }
                else
                {
					var issueEquiplist = RT.Service.Resolve<AssetIssueController>().GetAssetIssueEquipmentsById(parent.Id, parent.AssetRequisitionId);
                    foreach (var item in issueEquiplist)
                    {
						item.FactoryId = parent.FactoryId;
						item.LendingDepartmentId = parent.LendingDepartmentId;
					}
					return issueEquiplist;
                }
            }, "EditAssetIssueEquipmentViewGroup", false).HasLabel("设备清单").HasOrderNo(1).Show(ChildShowInWhere.All);

			View.AssociateChildrenProperty(AssetIssue.AssetIssueFixtureListProperty,
			e =>
			{
				var arg = e as ChildPagingDataWithParentEntityArgs;
				var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<AssetIssue>();

				if (parent == null)
				{
					return new EntityList<AssetIssueFixture>();
				}
				else
				{
					var issueFixturelist = RT.Service.Resolve<AssetIssueController>().GetAssetIssueFixturesById(parent.Id, parent.AssetRequisitionId);
					foreach (var item in issueFixturelist)
					{
						item.WarehouseId = parent.WarehouseId;
					}
					return issueFixturelist;
				}
			}, "EditAssetIssueFixtureViewGroup", false).HasLabel("工治具清单").HasOrderNo(2).Show(ChildShowInWhere.All);

			View.AttachDetailChildrenProperty(typeof(AssetIssue), c =>
			{
				AssetIssue entity = c.Parent as AssetIssue;
				return RF.GetById<AssetIssue>(entity.Id, new EagerLoadOptions().LoadWithViewProperty());
			}, EditIssueExternalInfoViewGroup, null).HasLabel("外部领用信息").HasOrderNo(3).Show(ChildShowInWhere.All);
		}

		///<summary>
		/// 配置外部领用信息查看视图
		/// </summary>
		protected void ConfigIssueExternalInfoView()
		{
			View.RemoveCommands(ConfigCommands.ModuleConfigCommand);

			View.UseDetail(3);
			View.DisableEditing();
			using (View.OrderProperties())
			{
				View.Property(p => p.ExternalType).Show();
				View.Property(p => p.SupplierCode).Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Supply).Show();
				View.Property(p => p.SupplierName).Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Supply).Show();
				View.Property(p => p.CustomerCode).Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Customer).Show();
				View.Property(p => p.CustomerName).Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Customer).Show();
				View.Property(p => p.Destination).Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Other).Show();
				View.Property(p => p.ContactPerson).Show();
				View.Property(p => p.ContactInformation).Show();
				View.Property(p => p.Address).Show();
				View.Property(p => p.DeliveryWay).Show();
				View.Property(p => p.TrackNo).Show();
			}
		}

		///<summary>
		/// 配置外部领用信息编辑视图
		/// </summary>
		protected void ConfigEditIssueExternalInfoView()
		{
			View.RemoveCommands(ConfigCommands.ModuleConfigCommand);

			View.UseDetail(4);
			using (View.OrderProperties())
			{
				View.Property(p => p.ExternalType).Readonly().Show();
				View.Property(p => p.SupplierCode).Readonly().Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Supply).Show();
				View.Property(p => p.SupplierName).Readonly().Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Supply).Show();
				View.Property(p => p.CustomerCode).Readonly().Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Customer).Show();
				View.Property(p => p.CustomerName).Readonly().Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Customer).Show();
				View.Property(p => p.Destination).Readonly().Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Other).Show();
				View.Property(p => p.ContactPerson).Show();
				View.Property(p => p.ContactInformation).Show();
				View.Property(p => p.Address).Show();
				View.Property(p => p.DeliveryWay).Show();
				View.Property(p => p.TrackNo).Show();
			}
		}
	}
}