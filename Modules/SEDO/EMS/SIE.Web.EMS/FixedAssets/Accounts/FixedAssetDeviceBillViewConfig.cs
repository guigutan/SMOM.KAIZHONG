using SIE.EMS.FixedAssets.Accounts;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.EMS.FixedAssets.Accounts.Commands;

namespace SIE.Web.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 设备清单视图配置
    /// </summary>
    public class FixedAssetDeviceBillViewConfig : WebViewConfig<FixedAssetDeviceBill>
    {
        /// <summary>
        /// 设备清单添加视图
        /// </summary>
        public const string AddDeviceBillViewGroup = "AddDeviceBillViewGroup";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { AddDeviceBillViewGroup });

            if (ViewGroup == AddDeviceBillViewGroup)
            {
                ConfigAddDeviceBillView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.IsMajor).Readonly();
            View.Property(p => p.ModelCode);
            View.Property(p => p.ModelName);
            View.Property(p => p.UseState);
            View.Property(p => p.Frozen);
            View.Property(p => p.EquipTypeCode);
            View.Property(p => p.EquipTypeName);
            View.Property(p => p.EquipTypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType;c.ReloadDataOnPopping = true; });
            View.Property(p => p.IsCustomsSupervision).Readonly();
            View.Property(p => p.UseLevel);
            View.Property(p => p.UseDepartmentName);
            View.Property(p => p.Manufacturer);
            View.Property(p => p.SupplierCode);
            View.Property(p => p.SupplierName);
            View.Property(p => p.PurchaseOrderNo);
            View.Property(p => p.EnterDate);
            View.Property(p => p.UsefulLife);
            View.Property(p => p.WarrantyPeriod);
            View.Property(p => p.InstallationLocation);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate);
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected void ConfigAddDeviceBillView()
        {
            View.AddBehavior("SIE.Web.EMS.FixedAssets.Accounts.Scripts.FixedAssetDeviceBillBehavior");
            View.UseCommands("SIE.Web.EMS.FixedAssets.Accounts.Commands.SelEquipsCommand", WebCommandNames.Delete);
            View.DisableEditing();

            using (View.OrderProperties()) 
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.IsMajor).Show();
                View.Property(p => p.ModelCode).Show();
                View.Property(p => p.ModelName).Show();
                View.Property(p => p.UseState).Show();
                View.Property(p => p.Frozen).Show();
                View.Property(p => p.EquipTypeCode).Show();
                View.Property(p => p.EquipTypeName).Show();
                View.Property(p => p.EquipTypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType;c.ReloadDataOnPopping = true; }).Show();
                View.Property(p => p.IsCustomsSupervision).Readonly().Show();
                View.Property(p => p.UseLevel).Show();
                View.Property(p => p.UseDepartmentName).Show();
                View.Property(p => p.Manufacturer).Show();
                View.Property(p => p.SupplierCode).Show();
                View.Property(p => p.SupplierName).Show();
                View.Property(p => p.PurchaseOrderNo).Show();
                View.Property(p => p.EnterDate).Show();
                View.Property(p => p.UsefulLife).Show();
                View.Property(p => p.WarrantyPeriod).Show();
                View.Property(p => p.InstallationLocation).Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}