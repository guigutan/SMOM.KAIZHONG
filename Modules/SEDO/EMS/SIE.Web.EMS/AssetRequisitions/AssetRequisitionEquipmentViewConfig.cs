using SIE.EMS.AssetRequisitions;
using SIE.EMS.Equipments;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Web.Equipments.Extensions;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.AssetRequisitions
{
	/// <summary>
	/// 领用申请设备清单视图配置
	/// </summary>
	public class AssetRequisitionEquipmentViewConfig : WebViewConfig<AssetRequisitionEquipment>
	{
		/// <summary>
		/// 设备清单编辑视图
		/// </summary>
		public const string EditAssetRequisitionEquipmentViewGroup = "EditAssetRequisitionEquipmentViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { EditAssetRequisitionEquipmentViewGroup });

			if (ViewGroup == EditAssetRequisitionEquipmentViewGroup)
			{
				ConfigEditAssetRequisitionEquipmentView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{ 	  
			View.DisableEditing(); 
			View.Property(p => p.LineNo);
			View.Property(p => p.EquipTypeId);
			View.Property(p => p.EquipTypeName);
			View.Property(p => p.EquipModelId).ShowInList(width: 120);
			View.Property(p => p.EquipModelName);
			View.Property(p => p.Specifications);
			View.Property(p => p.EquipAccountId);
			View.Property(p => p.Qty);
			View.Property(p => p.EstimatedAmount);
			View.Property(p => p.IssuedQty);
			View.Property(p => p.ReturnQty);
			View.Property(p => p.NoGoodsReturnQty).ShowInList(width:120);
			View.Property(p => p.WorkShopId);
			View.Property(p => p.ResourceId);
			View.Property(p => p.Location);
			View.Property(p => p.DepositaryId);
			View.Property(p => p.PickedQty);
		}

		///<summary>
		/// 配置设备清单编辑视图
		/// </summary>
		protected void ConfigEditAssetRequisitionEquipmentView()
		{
			View.AddBehavior("SIE.Web.EMS.AssetRequisitions.Behaviors.AssetRequisitionEquipmentDetailsBehavior");
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.AddAssetRequisitionEquipmentCommand");
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.DeleteAssetRequisitionEquipmentCommand");
			using (View.OrderProperties()) 
			{
				View.Property(p => p.LineNo).Readonly().Show();
				View.Property(p => p.EquipTypeId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EquipTypeController>().GetEquipTypes(pagingInfo, keyword);
                }).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.EquipTypeName), nameof(e.EquipType.TypeName));
					m.DicLinkField = keyValues;
				}).Cascade(p => p.EquipModelId, null).Cascade(p => p.EquipModelName, null)
				  .Cascade(p => p.Specifications, null).Cascade(p => p.EquipAccountId, null).Show();
				View.Property(p => p.EquipTypeName).Readonly(p => p.EquipTypeId != null).Show();
				View.Property(p => p.EquipModelId).UseDataSource((e, c, r) =>
				{
					var entity = e as AssetRequisitionEquipment;
					return RT.Service.Resolve<EquipController>().GetEquipModels(c, entity.EquipTypeId, r);
				}).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.EquipModelName), nameof(e.EquipModel.Name));
					keyValues.Add(nameof(e.Specifications), nameof(e.EquipModel.Specifications));
					m.DicLinkField = keyValues;
				}).Cascade(p => p.EquipAccountId, null).Show();
				View.Property(p => p.EquipModelName).Readonly(p => p.EquipModelId != null).Show();
				View.Property(p => p.Specifications).Readonly(p => p.EquipModelId != null).Show();
				View.Property(p => p.EquipAccountId).UseDataSource((e, c, r) =>
				{
					var entity = e as AssetRequisitionEquipment;
					return RT.Service.Resolve<EquipController>().GetEquipAccounts(c, entity.AssetRequisitionWarehouseId, entity.EquipModelId, entity.EquipTypeId,entity.FactoryId,entity.LendingDepartmentId, r);
				}).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.Qty), nameof(e.EquipAccount.OriginalValue));
					keyValues.Add(nameof(e.EstimatedAmount), nameof(e.EquipAccount.OriginalAssetsValue));
					m.DicLinkField = keyValues;
				}).HasLabel("设备编码".L10N() + "*").Show();
				View.Property(p => p.Qty).HasLabel("申请数量".L10N() + "*").UseSpinEditor(m => m.MinValue = 1).Readonly(p => p.EquipAccountId != null).Show();
				View.Property(p => p.EstimatedAmount).UseSpinEditor(m => { m.MinValue = 0; m.DecimalPrecision = 2; }).Show();
				View.Property(p => p.WorkShopId).UseFactoryWorkshopEditor().Show();
				View.Property(p => p.ResourceId).UseWorkShopResourceEditor().Show();
				View.Property(p => p.Location).Show();
				View.Property(p => p.DepositaryId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
                }).Show();
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}
			
		}
	}
}