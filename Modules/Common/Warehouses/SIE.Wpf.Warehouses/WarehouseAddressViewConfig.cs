using SIE.Warehouses;
using SIE.Wpf.Common;
using SIE.Wpf.CSM;
using SIE.Wpf.Warehouses.Command;

namespace SIE.Wpf.Warehouses
{
    /// <summary>
    /// 仓库地址视图配置
    /// </summary>
    internal class WarehouseAddressViewConfig : WPFViewConfig<WarehouseAddress>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().RemoveCommands(WPFCommandNames.ListSave);
            View.ReplaceCommands(WPFCommandNames.ListCopy, typeof(WarehouseAddressCopyCommand));
            View.Property(p => p.AddressType).UseCatalogEditor(e => e.CatalogType = WarehouseAddress.CatalogAddressType);
            View.Property(p => p.Name);
            View.Property(p => p.Country).UseRegionalEditor(w => w.ReloadDataOnPopping = true);
            View.Property(p => p.Province).UseRegionalEditor(w => { w.UpperLevelProperty = WarehouseAddress.CountryProperty; w.ReloadDataOnPopping = true; });
            View.Property(p => p.City).UseRegionalEditor(w => { w.UpperLevelProperty = WarehouseAddress.ProvinceProperty; w.UpperLevel2Property = WarehouseAddress.CountryProperty; w.ReloadDataOnPopping = true; });
            View.Property(p => p.Area).UseRegionalEditor(w => { w.UpperLevelProperty = WarehouseAddress.CityProperty; w.UpperLevel2Property = WarehouseAddress.ProvinceProperty; w.ReloadDataOnPopping = true; });
            View.Property(p => p.Address);
            View.Property(p => p.Employee).HasLabel("联系人");
            View.Property(p => p.Phone);
            View.Property(p => p.Fax);
            View.Property(p => p.Email);
            View.Property(p => p.ZipCode);
            View.Property(p => p.State).Readonly();
            View.Property(p => p.Remark);
            View.Property(p => p.FullAddress).Readonly();
            //View.Property(DataEntityExtension.UpdateByNameProperty).Readonly(); //更新人
            View.Property(p => p.UpdateDate).Readonly();
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.AddressType).UseCatalogEditor(e => e.CatalogType = WarehouseAddress.CatalogAddressType);
            View.Property(p => p.Name);
            View.Property(p => p.Country);
            View.Property(p => p.Province);
            View.Property(p => p.City);
            View.Property(p => p.Area);
            View.Property(p => p.Address);
            View.Property(p => p.Employee).HasLabel("联系人");
            View.Property(p => p.Phone);
            View.Property(p => p.Fax);
            View.Property(p => p.Email);
            View.Property(p => p.ZipCode);
            View.Property(p => p.State);
            View.Property(p => p.Remark);
        }
    }
}
