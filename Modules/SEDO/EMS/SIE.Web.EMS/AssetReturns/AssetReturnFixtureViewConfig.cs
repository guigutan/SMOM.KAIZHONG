using SIE.EMS.AssetReturns;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetReturns
{
	/// <summary>
	/// 归还工治具清单视图配置
	/// </summary>
	public class AssetReturnFixtureViewConfig : WebViewConfig<AssetReturnFixture>
	{
        /// <summary>
        /// 工治具清单编辑视图
        /// </summary>
        public const string EditAssetReturnFixtureViewGroup = "EditAssetReturnFixtureViewGroup";

        /// <summary>
        /// 工治具清单归还视图
        /// </summary>
        public const string ExistAssetReturnFixtureViewGroup = "ExistAssetReturnFixtureViewGroup";

        /// <summary>
        /// 配置视图属性
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { EditAssetReturnFixtureViewGroup, ExistAssetReturnFixtureViewGroup });

            if (ViewGroup == EditAssetReturnFixtureViewGroup)
            {
                ConfigEditAssetReturnFixtureView();
            }

            if (ViewGroup == ExistAssetReturnFixtureViewGroup)
            {
                ConfigExistAssetReturnFixtureView();
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
			View.Property(p => p.ReturnType);
			View.Property(p => p.Qty);
			View.Property(p => p.QualityStatus);
			View.Property(p => p.UnitName);
		}

        ///<summary>
        /// 配置工治具清单编辑视图
        /// </summary>
        protected void ConfigEditAssetReturnFixtureView()
        {
            View.AddBehavior("SIE.Web.EMS.AssetReturns.Behaviors.AssetReturnFixtureBehavior");
            View.UseCommand("SIE.Web.EMS.AssetReturns.Commands.CopyAssetReturnFixtureCommand");
            using (View.OrderProperties())
            {
                View.WithoutPaging();
                View.UseGridSelectionModel(checkOnly: true);
                View.Property(p => p.LineNo).Readonly().DisableSort().Show();
                View.Property(p => p.FixtureEncode).Readonly().DisableSort().ShowInList(width: 120).Show();
                View.Property(p => p.ModelCode).Readonly().DisableSort().Show();
                View.Property(p => p.ModelName).Readonly().DisableSort().Show();
                View.Property(p => p.FixtureType).Readonly().DisableSort().Show();
                View.Property(p => p.ManageMode).Readonly().DisableSort().Show();
                View.Property(p => p.Sn).Readonly().DisableSort().ShowInList(width: 120).Show();
                View.Property(p => p.NotReturnQty).Readonly().DisableSort().Show();
                View.Property(p => p.ReturnType).HasLabel("归还类型".L10N() + "*")
                    .Cascade(p => p.QualityStatus, null).Show();
                View.Property(p => p.Qty).HasLabel("归还数量".L10N() + "*").UseSpinEditor(m => m.MinValue = 0)
                    .Readonly(p => p.ManageMode == SIE.Fixtures.ManageMode.Number).DisableSort().Show();
                View.Property(p => p.QualityStatus).HasLabel("质量状态".L10N() + "*")
                    .Readonly(p => p.ReturnType != SIE.EMS.Enums.ReturnType.Real).DisableSort().Show();
                View.Property(p => p.UnitName).Readonly().DisableSort().Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }

        }

        ///<summary>
        /// 配置工治具清单归还视图
        /// </summary>
        protected void ConfigExistAssetReturnFixtureView()
        {
            using (View.OrderProperties())
            {
                View.WithoutPaging();
                View.DisableEditing();
                View.Property(p => p.ReturnNo).DisableSort().ShowInList(width: 120).Show();
                View.Property(p => p.ApprovalStatus).DisableSort().Show();
                View.Property(p => p.LineNo).DisableSort().Show();
                View.Property(p => p.FixtureEncode).DisableSort().ShowInList(width: 120).Show();
                View.Property(p => p.ModelCode).DisableSort().Show();
                View.Property(p => p.ModelName).DisableSort().Show();
                View.Property(p => p.FixtureType).DisableSort().Show();
                View.Property(p => p.ManageMode).DisableSort().Show();
                View.Property(p => p.Sn).DisableSort().ShowInList(width: 120).Show();
                View.Property(p => p.NotReturnQty).DisableSort().Show();
                View.Property(p => p.ReturnType).DisableSort().Show();
                View.Property(p => p.Qty).DisableSort().Show();
                View.Property(p => p.QualityStatus).DisableSort().Show();
                View.Property(p => p.UnitName).DisableSort().Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
