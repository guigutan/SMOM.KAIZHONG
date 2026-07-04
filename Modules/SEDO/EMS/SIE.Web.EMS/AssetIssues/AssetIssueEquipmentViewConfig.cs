using SIE.EMS.AssetIssues;
using SIE.EMS.Equipments;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.AssetIssues
{
	/// <summary>
	/// 发放设备清单视图配置
	/// </summary>
	public class AssetIssueEquipmentViewConfig : WebViewConfig<AssetIssueEquipment>
	{
		/// <summary>
		/// 设备清单编辑视图
		/// </summary>
		public const string EditAssetIssueEquipmentViewGroup = "EditAssetIssueEquipmentViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { EditAssetIssueEquipmentViewGroup });

			if (ViewGroup == EditAssetIssueEquipmentViewGroup)
			{
				ConfigEditAssetIssueEquipmentView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.DisableEditing();
			View.Property(p => p.LineNo);
			View.Property(p => p.EquipAccountCode);
			View.Property(p => p.EquipAccountName);
			View.Property(p => p.UseState);
			View.Property(p => p.Alias);
			View.Property(p => p.EquipModelCode);
			View.Property(p => p.EquipModelName);
			View.Property(p => p.Specifications);
			View.Property(p => p.EquipTypeCode);
			View.Property(p => p.EquipTypeName);
			View.Property(p => p.ReturnStatus);
			View.Property(p => p.ReturnNo);
			View.Property(p => p.ReturnDate);
		}

		///<summary>
		/// 配置设备清单编辑视图
		/// </summary>
		protected void ConfigEditAssetIssueEquipmentView()
		{
			bool trueValue = true;
			View.AddBehavior("SIE.Web.EMS.AssetIssues.Behaviors.AssetIssueEquipmentBehavior");
			using (View.OrderProperties())
			{
				View.WithoutPaging();
				View.UseGridSelectionModel(checkOnly: true);
				View.Property(p => p.LineNo).Readonly().DisableSort().Show();
				View.Property(p => p.EquipAccountId).UseDataSource((e, c, r) =>
				{
					var entity = e as AssetIssueEquipment;
					return RT.Service.Resolve<EquipController>().GetEquipAccounts(c, entity, r);
				}).Readonly(p=>p.IsSelectEquipAccount == trueValue).DisableSort().HasLabel("设备编码".L10N() + "*").ShowInList(width:115).Show();
				View.Property(p => p.EquipAccountName).Readonly().DisableSort().Show();
				View.Property(p => p.UseState).Readonly().DisableSort().Show();
				View.Property(p => p.Alias).Readonly().DisableSort().Show();
				View.Property(p => p.EquipModelCode).Readonly().DisableSort().Show();
				View.Property(p => p.EquipModelName).Readonly().DisableSort().Show();
				View.Property(p => p.Specifications).Readonly().DisableSort().Show();
				View.Property(p => p.EquipTypeCode).Readonly().DisableSort().Show();
				View.Property(p => p.EquipTypeName).Readonly().DisableSort().Show();
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}

		}
	}
}