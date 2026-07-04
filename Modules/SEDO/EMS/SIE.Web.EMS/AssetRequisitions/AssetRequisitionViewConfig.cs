using SIE.Domain;
using SIE.EMS.AssetRequisitions;
using SIE.EMS.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.AssetRequisitions
{
	/// <summary>
	/// 资产领用视图配置
	/// </summary>
	public class AssetRequisitionViewConfig : WebViewConfig<AssetRequisition>
    {
		/// <summary>
		/// 外部领用信息查看视图
		/// </summary>
		public const string ExternalInfoViewGroup = "ExternalInfoViewGroup";

		/// <summary>
		/// 外部领用信息编辑视图
		/// </summary>
		public const string EditExternalInfoViewGroup = "EditExternalInfoViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { ExternalInfoViewGroup, EditExternalInfoViewGroup });

			if (ViewGroup == ExternalInfoViewGroup)
			{
				ConfigExternalInfoView();
			}

			if (ViewGroup == EditExternalInfoViewGroup)
			{
				ConfigEditExternalInfoView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.AddBehavior("SIE.Web.EMS.Common.Script.ApprovalBehavior");
			View.AddBehavior("SIE.Web.EMS.AssetRequisitions.Behaviors.AssetRequisitionBehavior");
			View.UseCommand(WebCommandNames.Add);
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.EditAssetRequisitionCommand");
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.DeleteAssetRequisitionCommand");
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.SubmitAssetRequisitionCommand");
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.CancelAssetRequisitionCommand");
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.ApprovalAssetRequisitionCommand");
			View.FormEdit();
			View.Property(p => p.RequisitionNo).ShowInList(width:120);
			View.Property(p => p.FactoryId).UseFactoryEditor();
			View.Property(p => p.ApprovalStatus);
			View.Property(p => p.IssueStatus);
			View.Property(p => p.RequisitionType);
			View.Property(p => p.AssetObject);
			View.Property(p => p.ApplyDepartmentId);
			View.Property(p => p.LendingDepartmentId);
			View.Property(p => p.WarehouseId);
			View.Property(p => p.External).Readonly();
			View.Property(p => p.EmployeeId);
			View.Property(p => p.ApplyDate).UseDateEditor();
			View.Property(p => p.RetureDate).UseDateEditor();
			View.Property(p => p.ReturnStatus);
			View.Property(p => p.Amount);
			View.Property(p => p.Remark);

			View.ChildrenProperty(p => p.AssetRequisitionEquipmentList).HasOrderNo(1);
			View.ChildrenProperty(p => p.AssetRequisitionFixtureList).HasOrderNo(2);
			View.AttachDetailChildrenProperty(typeof(AssetRequisition), c =>
			{
				AssetRequisition entity = c.Parent as AssetRequisition;
				return RF.GetById<AssetRequisition>(entity.Id, new EagerLoadOptions().LoadWithViewProperty());
			}, ExternalInfoViewGroup, null).HasLabel("外部领用信息").HasOrderNo(3);
			View.ChildrenProperty(p => p.AssetRequisitionAttachmentList).HasOrderNo(4);

			View.AttachChildrenProperty(typeof(WorkFlowRecord), e =>
			{
				var args = e as ChildPagingDataArgs;
				var parent = args.Parent as AssetRequisition;
				if (parent == null)
				{
					return new EntityList<WorkFlowRecord>();
				}

				return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id,
					typeof(AssetRequisition).FullName, args.SortInfo, args.PagingInfo);

			}).HasLabel("审核记录").HasOrderNo(5);
		}

		///<summary>
		/// 配置表单视图
		/// </summary>
		protected override void ConfigDetailsView()
		{
			View.AddBehavior("SIE.Web.EMS.AssetRequisitions.Behaviors.AssetRequisitionDetailsBehavior");
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.SaveAssetRequisitionCommand");
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.SaveAndSubmitAssetRequisitionCommand");
			View.RemoveCommands(ConfigCommands.ModuleConfigCommand);
			View.UseDetail(4);
			View.Property(p => p.RequisitionNo).Readonly();
			View.Property(p => p.FactoryId).UseFactoryEditor().Cascade(p => p.ApplyDepartmentId, null).Cascade(p => p.LendingDepartmentId, null);
			View.Property(p => p.RequisitionType).Cascade(p=>p.LendingDepartmentId, null);
			View.Property(p => p.AssetObject);
			View.Property(p => p.ApplyDepartmentId).UseUserBussinessDepartmentEditor();
			View.Property(p => p.LendingDepartmentId).UseUserBussinessDepartmentEditor().Readonly(p => p.RequisitionType == SIE.EMS.Enums.RequisitionType.Consume);
            View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.External);
            View.Property(p => p.EmployeeId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.ApplyDate).UseDateEditor();
			View.Property(p => p.RetureDate).UseDateEditor();
			View.Property(p => p.Amount).Readonly();
			View.Property(p => p.Remark).HasLabel("备注").ShowInDetail(columnSpan:2);

            View.ChildrenProperty(p => p.AssetRequisitionEquipmentList).HasOrderNo(1).ViewGroup = "EditAssetRequisitionEquipmentViewGroup";

			View.AssociateChildrenProperty(AssetRequisition.AssetRequisitionFixtureListProperty,
			e =>
			{
				var arg = e as ChildPagingDataWithParentEntityArgs;
				var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<AssetRequisition>();

				if (parent == null)
				{
					return new EntityList<AssetRequisitionFixture>();
				}
				else
				{
					return RT.Service.Resolve<AssetRequisitionController>().GetAssetRequisitionFixtureList(parent.Id, parent.WarehouseId ?? 0, arg.SortInfo, arg.PagingInfo);
				}
			}, "EditAssetRequisitionFixtureViewGroup").HasLabel("工治具清单").HasOrderNo(2).Show(ChildShowInWhere.All);

			View.AttachDetailChildrenProperty(typeof(AssetRequisition), c =>
            {
                AssetRequisition entity = c.Parent as AssetRequisition;
                return RF.GetById<AssetRequisition>(entity.Id, new EagerLoadOptions().LoadWithViewProperty());
            }, EditExternalInfoViewGroup, null).HasLabel("外部领用信息").HasOrderNo(3).Show(ChildShowInWhere.All);
            View.ChildrenProperty(p => p.AssetRequisitionAttachmentList).HasOrderNo(4);
        }

		///<summary>
		/// 配置外部领用信息查看视图
		/// </summary>
		protected void ConfigExternalInfoView()
		{
			View.RemoveCommands(ConfigCommands.ModuleConfigCommand);

			View.UseDetail(3);
			View.DisableEditing();
			using (View.OrderProperties())
			{
				View.Property(p => p.ExternalType).HasLabel("外部领用类型".L10N() + "*").Show();
				View.Property(p => p.SupplierId).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
					m.DicLinkField = keyValues;
				}).Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Supply).Show();
				View.Property(p => p.SupplierName).Readonly().Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Supply).Show();

				View.Property(p => p.CustomerId).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.CustomerName), nameof(e.Customer.Name));
					m.DicLinkField = keyValues;
				}).Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Customer).Show();
				View.Property(p => p.CustomerName).Readonly().Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Customer).Show();

				View.Property(p => p.Destination).Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Other).Show();
				View.Property(p => p.ContactPerson).HasLabel("联系人".L10N() + "*").Show();
				View.Property(p => p.ContactInformation).HasLabel("联系方式".L10N() + "*").Show();
				View.Property(p => p.Address).HasLabel("联系地址".L10N() + "*").Show();
				View.Property(p => p.DeliveryWay).HasLabel("发货方式".L10N() + "*").Show();
				View.Property(p => p.TrackNo).Show();
			}
		}

		///<summary>
		/// 配置外部领用信息编辑视图
		/// </summary>
		protected void ConfigEditExternalInfoView()
		{
			View.RemoveCommands(ConfigCommands.ModuleConfigCommand);

			View.UseDetail(4);
			using (View.OrderProperties())
			{
				View.Property(p => p.ExternalType).HasLabel("外部领用类型".L10N() + "*").Show();
				View.Property(p => p.SupplierId).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
					m.DicLinkField = keyValues;
				}).Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Supply).Show();
				View.Property(p => p.SupplierName).Readonly().Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Supply).Show();

				View.Property(p => p.CustomerId).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.CustomerName), nameof(e.Customer.Name));
					m.DicLinkField = keyValues;
				}).Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Customer).Show();
				View.Property(p => p.CustomerName).Readonly().Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Customer).Show();

				View.Property(p => p.Destination).Visibility(p => p.ExternalType == SIE.EMS.Enums.ExternalType.Other).Show();
				View.Property(p => p.ContactPerson).HasLabel("联系人".L10N() + "*").Show();
				View.Property(p => p.ContactInformation).HasLabel("联系方式".L10N() + "*").Show();
				View.Property(p => p.Address).HasLabel("联系地址".L10N() + "*").Show();
				View.Property(p => p.DeliveryWay).HasLabel("发货方式".L10N() + "*").Show();
				View.Property(p => p.TrackNo).Show();
			}
		}

		/// <summary>
		/// 配置下拉视图
		/// </summary>
		protected override void ConfigSelectionView() 
		{
			View.DisableEditing();
			View.Property(p => p.RequisitionNo).ShowInList(width: 120).Show();
			View.Property(p => p.FactoryId).Show();
			View.Property(p => p.ApprovalStatus).Show();
			View.Property(p => p.IssueStatus).Show();
			View.Property(p => p.RequisitionType).Show();
			View.Property(p => p.AssetObject).Show();
			View.Property(p => p.ApplyDepartmentId).Show();
			View.Property(p => p.LendingDepartmentId).Show();
			View.Property(p => p.WarehouseId).Show();
			View.Property(p => p.External).Readonly().Show();
			View.Property(p => p.Remark).Show();
		}
	}
}