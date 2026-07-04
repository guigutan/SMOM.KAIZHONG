using SIE.EMS.AssetDisposals;
using SIE.EMS.AssetScraps;
using SIE.Fixtures;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetDisposals
{
	/// <summary>
	/// 工治具清单视图配置
	/// </summary>
	public class AssetDisposalFixtureViewConfig : WebViewConfig<AssetDisposalFixture>
	{
		/// <summary>
		/// 工治具清单编辑视图
		/// </summary>
		public const string EditAssetDisposalFixtureViewGroup = "EditAssetDisposalFixtureViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { EditAssetDisposalFixtureViewGroup });

			if (ViewGroup == EditAssetDisposalFixtureViewGroup)
			{
				ConfigEditAssetDisposalFixtureView();
			}
		}
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.DisableEditing();
			View.Property(p => p.EncodeCode).ShowInList(width: 120);
			View.Property(p => p.ModelCode);
			View.Property(p => p.ModelName);
			View.Property(p => p.FixtureTypeCode);
			View.Property(p => p.Code).ShowInList(width: 120);
			View.Property(p => p.ScrapType).UseCatalogEditor(e => { e.CatalogType = AssetScrapEquipment.EquipScrapType; e.CatalogReloadData = true; });
			View.Property(p => p.Reason);
			View.Property(p => p.FixedAssetsAccountCode);
			View.Property(p => p.FixedAssetsAccountName);
			View.Property(p => p.OriginalValue);
			View.Property(p => p.NetValue);
			View.Property(p => p.ResidualValue);
		}

		///<summary>
		/// 配置工治具清单编辑视图
		/// </summary>
		protected void ConfigEditAssetDisposalFixtureView()
		{
            View.AddBehavior("SIE.Web.EMS.AssetDisposals.Behaviors.AssetDisposalFixtureDetailsBehavior");
            View.UseCommand("SIE.Web.EMS.AssetDisposals.Commands.AddAssetDisposalFixtureCommand");
            View.UseCommand("SIE.Web.EMS.AssetDisposals.Commands.DeleteAssetDisposalFixtureCommand");
            using (View.OrderProperties())
            {
				View.Property(p => p.FixtureEncodeId).UseDataSource((e, c, r) =>
				{
					var fixture = e as AssetDisposalFixture;
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodes(ManageMode.Number, fixture.WarehouseId, r, c);
				}).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.ModelCode), nameof(e.FixtureEncode.ModelCode));
					keyValues.Add(nameof(e.ModelName), nameof(e.FixtureEncode.ModelName));
					keyValues.Add(nameof(e.FixtureTypeCode), nameof(e.FixtureEncode.FixtureType));
					m.DicLinkField = keyValues;
				}).Cascade(p => p.FixtureAccountId, null).ShowInList(width: 120).Show();
				View.Property(p => p.ModelCode).Readonly().Show();
				View.Property(p => p.ModelName).Readonly().Show();
				View.Property(p => p.FixtureTypeCode).Readonly().Show();
				View.Property(p => p.FixtureAccountId).UseDataSource((e, c, r) =>
				{
					var entity = e as AssetDisposalFixture;
					return RT.Service.Resolve<AssetDisposalController>().GetFixtureIDAccounts(c, entity, r);
				}).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.FixedAssetsAccountCode), nameof(e.FixtureAccount.FixedAssetsAccountCode));
					keyValues.Add(nameof(e.FixedAssetsAccountName), nameof(e.FixtureAccount.FixedAssetsAccountName));
					keyValues.Add(nameof(e.OriginalValue), nameof(e.FixtureAccount.OriginalAssetsValue));
					keyValues.Add(nameof(e.NetValue), nameof(e.FixtureAccount.NetAssetValue));
					keyValues.Add(nameof(e.ResidualValue), nameof(e.FixtureAccount.DepreciationResidualValue));
					keyValues.Add(nameof(e.ScrapType), nameof(e.FixtureAccount.ScrapType));
					keyValues.Add(nameof(e.Reason), nameof(e.FixtureAccount.Reason));
					m.DicLinkField = keyValues;
				}).ShowInList(width: 120).Show();
				View.Property(p => p.ScrapType).UseCatalogEditor(e => { e.CatalogType = AssetScrapEquipment.EquipScrapType; e.CatalogReloadData = true; }).Readonly().Show();
				View.Property(p => p.Reason).Readonly().Show();
				View.Property(p => p.FixedAssetsAccountCode).Readonly().Show();
				View.Property(p => p.FixedAssetsAccountName).Readonly().Show();
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
