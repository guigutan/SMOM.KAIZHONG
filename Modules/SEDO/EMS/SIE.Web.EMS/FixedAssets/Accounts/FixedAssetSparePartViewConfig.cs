using SIE.EMS.FixedAssets.Accounts;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 设备清单视图配置
    /// </summary>
    public class FixedAssetSparePartViewConfig : WebViewConfig<FixedAssetSparePart>
    {
        /// <summary>
        /// 备件清单添加视图
        /// </summary>
        public const string AddFixedAssetSparePartViewGroup = "AddFixedAssetSparePartViewGroup";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { AddFixedAssetSparePartViewGroup });

            if (ViewGroup == AddFixedAssetSparePartViewGroup)
            {
                ConfigAddFixedAssetSparePartView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.OrderNumberCode);
            View.Property(p => p.SparePartCode);
            View.Property(p => p.SparePartName);
            View.Property(p => p.IsMajor).Readonly();
            View.Property(p => p.State);
            View.Property(p => p.Specification);
            View.Property(p => p.SpartType);
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
        protected void ConfigAddFixedAssetSparePartView()
        {
            View.AddBehavior("SIE.Web.EMS.FixedAssets.Accounts.Scripts.FixedAssetSparePartBehavior");
            View.UseCommands("SIE.Web.EMS.FixedAssets.Accounts.Commands.SelFixedAssetSparePartCommand", WebCommandNames.Delete);
            View.DisableEditing();

            using (View.OrderProperties())
            {
                View.Property(p => p.OrderNumberCode).Show();
                View.Property(p => p.SparePartCode).Show();
                View.Property(p => p.SparePartName).Show();
                View.Property(p => p.IsMajor).Show();
                View.Property(p => p.State).Show();
                View.Property(p => p.Specification).Show();
                View.Property(p => p.SpartType).Show();
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
