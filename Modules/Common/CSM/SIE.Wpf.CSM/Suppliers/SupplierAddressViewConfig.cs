using SIE.CSM.Suppliers;
using SIE.Wpf.Common;
using SIE.Wpf.Common.Commands;
using SIE.Wpf.CSM.Suppliers.Commonds;

namespace SIE.Wpf.CSM.Suppliers
{
    /// <summary>
    /// 供应商地址视图配置
    /// </summary>
    internal class SupplierAddressViewConfig : WPFViewConfig<SupplierAddress>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.ReplaceCommands(WPFCommandNames.ListCopy, typeof(AddressCopyCommand));
            View.ReplaceCommands(typeof(EnableCommand), typeof(AddressEnabledCommand));
            View.ReplaceCommands(typeof(DisableCommand), typeof(AddressDisEnabledCommand));
            View.UseCommands(typeof(SupplierAddressDefaultCommand));
            View.RemoveCommands(WPFCommandNames.ListSave);
        }

        /// <summary>
        /// 视图列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.AddressType).UseListSetting(e => { e.HelpInfo = "地址快码类型(ADDRESS_TYPE)"; })
                .UseCatalogEditor(e => e.CatalogType = SupplierAddress.CatalogAddressType);
            View.Property(p => p.Name);
            View.Property(p => p.Country).UseListSetting(w => w.ListGridWidth = 60).UseRegionalEditor(w => w.ReloadDataOnPopping = true);
            View.Property(p => p.Province).UseListSetting(w => w.ListGridWidth = 60).UseRegionalEditor(w => { w.UpperLevelProperty = SupplierAddress.CountryProperty; w.ReloadDataOnPopping = true; });
            View.Property(p => p.City).UseListSetting(w => w.ListGridWidth = 60).UseRegionalEditor(w => { w.UpperLevelProperty = SupplierAddress.ProvinceProperty; w.UpperLevel2Property = SupplierAddress.CountryProperty; w.ReloadDataOnPopping = true; });
            View.Property(p => p.Area).UseListSetting(w => w.ListGridWidth = 60).UseRegionalEditor(w => { w.UpperLevelProperty = SupplierAddress.CityProperty; w.UpperLevel2Property = SupplierAddress.ProvinceProperty; w.ReloadDataOnPopping = true; });
            View.Property(p => p.Address);
            View.Property(p => p.Contacts);
            View.Property(p => p.Phone);
            View.Property(p => p.Fax);
            View.Property(p => p.EMail);
            View.Property(p => p.ZipCode).UseListSetting(w => w.ListGridWidth = 70);
            View.Property(p => p.IsDefault).Readonly();
            View.Property(p => p.State).Readonly().UseListSetting(w => w.ListGridWidth = 60);
            View.Property(p => p.Remark);
        }
    }
}
