using SIE.EMS.AssetDisposals;
using SIE.EMS.AssetScraps;
using SIE.EMS.Equipments;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetDisposals
{
	/// <summary>
	/// 设备清单视图配置
	/// </summary>
	public class AssetDisposalEquipmentViewConfig : WebViewConfig<AssetDisposalEquipment>
	{
		/// <summary>
		/// 设备清单编辑视图
		/// </summary>
		public const string EditAssetDisposalEquipmentViewGroup = "EditAssetDisposalEquipmentViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { EditAssetDisposalEquipmentViewGroup });

			if (ViewGroup == EditAssetDisposalEquipmentViewGroup)
			{
				ConfigEditAssetDisposalEquipmentView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.DisableEditing();
			View.Property(p => p.EquipAccountCode).ShowInList(width: 120);
			View.Property(p => p.EquipAccountName);
			View.Property(p => p.Alias);
			View.Property(p => p.EquipModelCode);
			View.Property(p => p.EquipModelName);
			View.Property(p => p.Specifications);
			View.Property(p => p.FixedAssetsAccountCode);
			View.Property(p => p.FixedAssetsAccountName);
			View.Property(p => p.ScrapType).UseCatalogEditor(e => { e.CatalogType = AssetScrapEquipment.EquipScrapType;e.CatalogReloadData = true; });
			View.Property(p => p.Reason);
			View.Property(p => p.OriginalValue);
			View.Property(p => p.NetValue);
			View.Property(p => p.ResidualValue);
		}

		///<summary>
		/// 配置设备清单编辑视图
		/// </summary>
		protected void ConfigEditAssetDisposalEquipmentView()
		{
            View.AddBehavior("SIE.Web.EMS.AssetDisposals.Behaviors.AssetDisposalEquipmentDetailsBehavior");
            View.UseCommand("SIE.Web.EMS.AssetDisposals.Commands.AddAssetDisposalEquipmentCommand");
            View.UseCommand("SIE.Web.EMS.AssetDisposals.Commands.DeleteAssetDisposalEquipmentCommand");
            using (View.OrderProperties())
            {
				View.Property(p => p.EquipAccountId).UseDataSource((e, c, r) =>
				{
					var entity = e as AssetDisposalEquipment;
					return RT.Service.Resolve<EquipController>().GetEquipAccounts(c, entity, r);
				}).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
					keyValues.Add(nameof(e.Alias), nameof(e.EquipAccount.Alias));
					keyValues.Add(nameof(e.EquipModelCode), nameof(e.EquipAccount.ModelCode));
					keyValues.Add(nameof(e.EquipModelName), nameof(e.EquipAccount.ModelName));
					keyValues.Add(nameof(e.Specifications), nameof(e.EquipAccount.Specifications));
					keyValues.Add(nameof(e.FixedAssetsAccountCode), nameof(e.EquipAccount.FixedAssetsAccountCode));
					keyValues.Add(nameof(e.FixedAssetsAccountName), nameof(e.EquipAccount.FixedAssetsAccountName));
					keyValues.Add(nameof(e.OriginalValue), nameof(e.EquipAccount.OriginalAssetsValue));
					keyValues.Add(nameof(e.NetValue), nameof(e.EquipAccount.NetAssetValue));
					keyValues.Add(nameof(e.ResidualValue), nameof(e.EquipAccount.DepreciationResidualValue));
					keyValues.Add(nameof(e.ScrapType), nameof(e.EquipAccount.ScrapType));
					keyValues.Add(nameof(e.Reason), nameof(e.EquipAccount.Reason));
					m.DicLinkField = keyValues;
				}).ShowInList(width: 120).Show();
                View.Property(p => p.EquipAccountName).Readonly().Show();
                View.Property(p => p.Alias).Readonly().Show();
                View.Property(p => p.EquipModelCode).Readonly().Show();
                View.Property(p => p.EquipModelName).Readonly().Show();
                View.Property(p => p.Specifications).Readonly().Show();
                View.Property(p => p.FixedAssetsAccountCode).Readonly().Show();
                View.Property(p => p.FixedAssetsAccountName).Readonly().Show();
				View.Property(p => p.ScrapType).UseCatalogEditor(e => { e.CatalogType = AssetScrapEquipment.EquipScrapType; e.CatalogReloadData = true; }).Readonly().Show();
				View.Property(p => p.Reason).Readonly().Show();
				View.Property(p => p.OriginalValue).Readonly().Show();
				View.Property(p => p.NetValue).Readonly().Show();
				View.Property(p => p.ResidualValue).Readonly().Show();
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
	}
}
