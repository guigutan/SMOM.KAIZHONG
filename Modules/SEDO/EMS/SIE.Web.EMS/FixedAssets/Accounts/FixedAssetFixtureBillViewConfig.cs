using SIE.EMS.FixedAssets.Accounts;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 工治具清单视图配置
    /// </summary>
    public class FixedAssetFixtureBillViewConfig : WebViewConfig<FixedAssetFixtureBill>
    {
        /// <summary>
        /// 工治具清单添加视图
        /// </summary>
        public const string AddFixedAssetFixtureViewGroup = "AddFixedAssetFixtureViewGroup";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { AddFixedAssetFixtureViewGroup });

            if (ViewGroup == AddFixedAssetFixtureViewGroup)
            {
                ConfigAddFixedAssetFixtureView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.Code);
            View.Property(p => p.EncodeCode);
            View.Property(p => p.IsMajor).Readonly();
            View.Property(p => p.AccountState);
            View.Property(p => p.ModelCode);
            View.Property(p => p.ModelName);
            View.Property(p => p.FixtureTypeCode);
            View.Property(p => p.UnitName);
            View.Property(p => p.WarehouseName);
            View.Property(p => p.StorageLocationName);
            View.Property(p => p.SupplierCode);
            View.Property(p => p.SupplierName);

            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate);
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected void ConfigAddFixedAssetFixtureView()
        {
            View.AddBehavior("SIE.Web.EMS.FixedAssets.Accounts.Scripts.FixedAssetFixtureBillBehavior");
            View.UseCommands("SIE.Web.EMS.FixedAssets.Accounts.Commands.SelFixedAssetFixtureCommand", WebCommandNames.Delete);
            View.DisableEditing();

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.EncodeCode).Show();
                View.Property(p => p.IsMajor).Show();
                View.Property(p => p.AccountState).Show();
                View.Property(p => p.ModelCode).Show();
                View.Property(p => p.ModelName).Show();
                View.Property(p => p.FixtureTypeCode).Show();
                View.Property(p => p.UnitName).Show();
                View.Property(p => p.WarehouseName).Show();
                View.Property(p => p.StorageLocationName).Show();
                View.Property(p => p.SupplierCode).Show();
                View.Property(p => p.SupplierName).Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
