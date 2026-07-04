using SIE.Domain;
using SIE.EMS.AssetRequisitions;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using System.Collections.Generic;

namespace SIE.Web.EMS.AssetRequisitions
{
	/// <summary>
	/// 领用申请工治具清单视图配置
	/// </summary>
	public class AssetRequisitionFixtureViewConfig : WebViewConfig<AssetRequisitionFixture>
	{
		/// <summary>
		/// 工治具清单编辑视图
		/// </summary>
		public const string EditAssetRequisitionFixtureViewGroup = "EditAssetRequisitionFixtureViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { EditAssetRequisitionFixtureViewGroup });

			if (ViewGroup == EditAssetRequisitionFixtureViewGroup)
			{
				ConfigEditAssetRequisitionFixtureView();
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
			View.Property(p => p.Qty);
			View.Property(p => p.EstimatedAmount);
			View.Property(p => p.IssuedQty);
			View.Property(p => p.ReturnQty);
			View.Property(p => p.NoGoodsReturnQty).ShowInList(width: 120);
			View.Property(p => p.PickedQty);
			View.Property(p => p.UnitName);
		}

		///<summary>
		/// 配置工治具清单编辑视图
		/// </summary>
		protected void ConfigEditAssetRequisitionFixtureView()
		{
			View.AddBehavior("SIE.Web.EMS.AssetRequisitions.Behaviors.AssetRequisitionFixtureDetailsBehavior");
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.AddAssetRequisitionFixtureCommand");
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.DeleteAssetRequisitionFixtureCommand");
			using (View.OrderProperties())
			{
				View.Property(p => p.LineNo).Readonly().Show();
				View.Property(p => p.FixtureEncodeId).
					UseDataSource((e, c, r) =>
					{
						return RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodes(r, c);
					}).
				UsePagingLookUpEditor((m, e) =>
				{
					Dictionary<string, string> keyValues = new Dictionary<string, string>();
					keyValues.Add(nameof(e.ModelCode), nameof(e.FixtureEncode.ModelCode));
					keyValues.Add(nameof(e.ModelName), nameof(e.FixtureEncode.ModelName));
					keyValues.Add(nameof(e.FixtureType), nameof(e.FixtureEncode.FixtureType));
					keyValues.Add(nameof(e.ManageMode), nameof(e.FixtureEncode.ManageMode));
					keyValues.Add(nameof(e.UnitName), nameof(e.FixtureEncode.UnitName));
					m.DicLinkField = keyValues;
				}).ShowInList(width: 120).Show();

				View.Property(p => p.ModelCode).Readonly().Show();
				View.Property(p => p.ModelName).Readonly().Show();
				View.Property(p => p.FixtureType).Readonly().Show();
				View.Property(p => p.ManageMode).Readonly().Show();
				View.Property(p => p.Qty).UseSpinEditor(m => m.MinValue = 1).Show();
				View.Property(p => p.EstimatedAmount).UseSpinEditor(m => { m.MinValue = 0; m.DecimalPrecision = 2; }).Show();
				View.Property(p => p.UnitName).Readonly().Show();
				View.Property(p => p.StoreUsableQty).Readonly().Show();
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}

		}
	}
}