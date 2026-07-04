using SIE.Domain;
using SIE.EMS.AssetScraps;
using SIE.Warehouses;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetScraps
{
	/// <summary>
	/// 资产报废工治具清单视图配置
	/// </summary>
	public class AssetScrapFixtureViewConfig : WebViewConfig<AssetScrapFixture>
	{
		/// <summary>
		/// 工治具清单编辑视图
		/// </summary>
		public const string EditAssetScrapFixtureViewGroup = "EditAssetScrapFixtureViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { EditAssetScrapFixtureViewGroup });

			if (ViewGroup == EditAssetScrapFixtureViewGroup)
			{
				ConfigEditAssetScrapFixtureView();
			}
		}
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.DisableEditing();
			View.Property(p => p.FixtureEncodeCode).ShowInList(width: 120);
			View.Property(p => p.ModelCode);
			View.Property(p => p.ModelName);
			View.Property(p => p.FixtureType);
			View.Property(p => p.ManageMode);
			View.Property(p => p.FixtureAccountId).ShowInList(width: 120);
			View.Property(p => p.Qty);
			View.Property(p => p.UnitName);
			View.Property(p => p.StorageLocationId).HasLabel("报废库位");
			View.Property(p => p.FixtureQualityState);
			View.Property(p => p.ScrapType).UseCatalogEditor(e => { e.CatalogType = AssetScrapFixture.FixtureScrapType; e.CatalogReloadData = true; });
			View.Property(p => p.Reason);
			View.Property(p => p.ScrapWarehouseId);
			View.Property(p => p.ScrapLocationId);
			View.Property(p => p.FixedAssetsAccountCode);
			View.Property(p => p.FixedAssetsAccountName);
			View.Property(p => p.OriginalAssetsValue);
			View.Property(p => p.ScrapNetValue);
		}

		///<summary>
		/// 配置工治具清单编辑视图
		/// </summary>
		protected void ConfigEditAssetScrapFixtureView()
		{
            View.AddBehavior("SIE.Web.EMS.AssetScraps.Behaviors.AssetScrapFixtureDetailsBehavior");
            View.UseCommand("SIE.Web.EMS.AssetScraps.Commands.AddAssetScrapFixtureCommand");
            View.UseCommand("SIE.Web.EMS.AssetScraps.Commands.DeleteAssetScrapFixtureCommand");
            using (View.OrderProperties())
            {
				View.Property(p => p.FixtureEncodeId).UseDataSource((e, c, r) =>
				{
					var entity = e as AssetScrapFixture;
					return RT.Service.Resolve<AssetScrapController>().GetAssetScrapFixtureEncodes(c, entity, r);
				}).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.ModelCode), nameof(e.FixtureEncode.ModelCode));
					keyValues.Add(nameof(e.ModelName), nameof(e.FixtureEncode.ModelName));
					keyValues.Add(nameof(e.FixtureType), nameof(e.FixtureEncode.FixtureType));
					keyValues.Add(nameof(e.ManageMode), nameof(e.FixtureEncode.ManageMode));
					keyValues.Add(nameof(e.UnitName), nameof(e.FixtureEncode.UnitName));
					m.DicLinkField = keyValues;
				}).Cascade(p => p.FixtureAccountId, null)
				  .Cascade(p => p.FixtureQualityState, null)
				  .Cascade(p => p.StorageLocationId, null).ShowInList(width: 120).Show();

                View.Property(p => p.ModelCode).Readonly().Show();
                View.Property(p => p.ModelName).Readonly().Show();
                View.Property(p => p.FixtureType).Readonly().Show();
                View.Property(p => p.ManageMode).Readonly().Show();
				View.Property(p => p.FixtureAccountId).UseDataSource((e, c, r) =>
				{
					var entity = e as AssetScrapFixture;
					return RT.Service.Resolve<AssetScrapController>().GetAssetScrapFixtureIDAccounts(c, entity, r);
				}).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.FixtureQualityState), "QualityState");
					keyValues.Add("StorageLocationId_Display", "LocationCode");
                    keyValues.Add(nameof(e.StorageLocationId), "LocationId");
					keyValues.Add(nameof(e.Qty), "ScrapQty");
					keyValues.Add(nameof(e.FixedAssetsAccountCode), "FixedAssetsAccountCode");
					keyValues.Add(nameof(e.FixedAssetsAccountName), "FixedAssetsAccountName");
					keyValues.Add(nameof(e.OriginalAssetsValue), "OriginalAssetsValue");
					m.DicLinkField = keyValues;
				}).Readonly(p => p.ManageMode != SIE.Fixtures.ManageMode.Number).DisableSort().ShowInList(width: 120).Show();
				View.Property(p => p.Qty).Readonly(p => p.ManageMode != SIE.Fixtures.ManageMode.Code).UseSpinEditor(m => m.MinValue = 1).Show();
				View.Property(p => p.UnitName).Readonly().Show();
				View.Property(p => p.FixtureQualityState).Readonly(p => p.ManageMode != SIE.Fixtures.ManageMode.Code).Show();
				View.Property(p => p.StorageLocationId).UseDataSource((e, c, r) =>
				{
					var entity = e as AssetScrapFixture;
					return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocationDatas(entity.WarehouseId ?? 0, r, c);
				}).Readonly(p => p.ManageMode != SIE.Fixtures.ManageMode.Code).DisableSort().Show();
				View.Property(p => p.StoreUsableQty).Readonly().Show();
				View.Property(p => p.ScrapType).UseCatalogEditor(e => { e.CatalogType = AssetScrapFixture.FixtureScrapType; e.CatalogReloadData = true; }).Show();
				View.Property(p => p.Reason).Show();
				View.Property(p => p.ScrapWarehouseId).UseDataSource((e, c, r) =>
				{
					return RT.Service.Resolve<SIE.EMS.Warehouses.WarehouseController>().GetStageLocatgionWarehouses(r, c);
				}).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add("ScrapLocationId_Display", nameof(e.AssetScrap.ScrapLocationCode));
					keyValues.Add(nameof(e.ScrapLocationId), nameof(e.ScrapLocationId));
					m.DicLinkField = keyValues;
				}).Show();
				View.Property(p => p.ScrapLocationId).UseDataSource((e, c, r) =>
				{
					var entity = e as AssetScrapFixture;
					return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocationDatas(entity.ScrapWarehouseId, r, c);
				}).Show();
				View.Property(p => p.FixedAssetsAccountCode).Readonly().Show();
				View.Property(p => p.FixedAssetsAccountName).Readonly().Show();
				View.Property(p => p.OriginalAssetsValue).Readonly().Show();
				View.Property(p => p.ScrapNetValue).UseSpinEditor(m => m.DecimalPrecision = 2).Show();
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
	}
}
