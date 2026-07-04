using SIE.EMS.AssetIssues;
using SIE.Warehouses;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.AssetIssues
{
	/// <summary>
	/// 发放工治具清单视图配置
	/// </summary>
	public class AssetIssueFixtureViewConfig : WebViewConfig<AssetIssueFixture>
	{
		/// <summary>
		/// 工治具清单编辑视图
		/// </summary>
		public const string EditAssetIssueFixtureViewGroup = "EditAssetIssueFixtureViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { EditAssetIssueFixtureViewGroup });

			if (ViewGroup == EditAssetIssueFixtureViewGroup)
			{
				ConfigEditAssetIssueFixtureView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.DisableEditing();
			View.Property(p => p.LineNo);
			View.Property(p => p.FixtureEncode).ShowInList(width: 120);
			View.Property(p => p.ModelCode);
			View.Property(p => p.ModelName);
			View.Property(p => p.FixtureType);
			View.Property(p => p.ManageMode);
			View.Property(p => p.FixtureAccountId).ShowInList(width: 120);
			View.Property(p => p.Qty);
			View.Property(p => p.UnitName);
			View.Property(p => p.QualityStatus);
			View.Property(p => p.StorageLocationId);
			View.Property(p => p.ReturnStatus);
			View.Property(p => p.ReturnNo);
			View.Property(p => p.ReturnDate);
		}

		///<summary>
		/// 配置工治具清单编辑视图
		/// </summary>
		protected void ConfigEditAssetIssueFixtureView()
		{
			View.AddBehavior("SIE.Web.EMS.AssetIssues.Behaviors.AssetIssueFixtureBehavior");
			View.UseCommand("SIE.Web.EMS.AssetIssues.Commands.CopyAssetIssueFixtureCommand");
			using (View.OrderProperties())
			{
				View.WithoutPaging();
				View.UseGridSelectionModel(checkOnly: true);
				View.Property(p => p.LineNo).Readonly().DisableSort().Show();
				View.Property(p => p.FixtureEncodeId).Show(ShowInWhere.Hide);
				View.Property(p => p.FixtureEncode).Readonly().DisableSort().ShowInList(width: 120).Show();
				View.Property(p => p.ModelCode).Readonly().DisableSort().Show();
				View.Property(p => p.ModelName).Readonly().DisableSort().Show();
				View.Property(p => p.FixtureType).Readonly().DisableSort().Show();
				View.Property(p => p.ManageMode).Readonly().DisableSort().Show();
				View.Property(p => p.FixtureAccountId).UseDataSource((e, c, r) =>
				{
					var entity = e as AssetIssueFixture;
					return RT.Service.Resolve<AssetIssueController>().GetFixtureIDAccounts(c, entity, r);
				}).UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.QualityStatus), "QualityState");
					keyValues.Add("StorageLocationId_Display", "LocationCode");
					keyValues.Add(nameof(e.StorageLocationId), "LocationId");
					m.DicLinkField = keyValues;
				}).Readonly(p=>p.ManageMode == SIE.Fixtures.ManageMode.Code).DisableSort().ShowInList(width: 120).Show();
				View.Property(p => p.NotPickQty).Readonly().DisableSort().Show();
				View.Property(p => p.Qty).HasLabel("本次发放数量".L10N() + "*").UseSpinEditor(m => m.MinValue = 0)
					.Readonly(p => p.ManageMode == SIE.Fixtures.ManageMode.Number).DisableSort().Show();
				View.Property(p => p.UnitName).Readonly().DisableSort().Show();
				View.Property(p => p.QualityStatus).HasLabel("质量状态".L10N() + "*")
					.Readonly(p => p.ManageMode == SIE.Fixtures.ManageMode.Number).DisableSort().Show();
				View.Property(p => p.StorageLocationId).UseDataSource((e, c, r) =>
				{
					var entity = e as AssetIssueFixture;
					return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocationDatas((double)entity.WarehouseId, r, c);
				}).HasLabel("发放库位".L10N() + "*").Readonly(p => p.ManageMode == SIE.Fixtures.ManageMode.Number).DisableSort().Show();
				View.Property(p => p.StoreUsableQty).Readonly().DisableSort().Show();
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}

		}
	}
}