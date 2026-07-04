using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Wpf.Common;
using SIE.Wpf.CSM.Customers.Commands;

namespace SIE.Wpf.CSM.Customers
{
    /// <summary>
    /// 客户地址视图配置
    /// </summary>
    internal class CustomerAddressViewConfig : WPFViewConfig<CustomerAddress>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.UseCommands(typeof(CustomerAddressDefaultCommand));
            View.UseDefaultCommands().RemoveCommands(WPFCommandNames.ListSave);
            View.UseCommands(WPFCommandNames.Undo);
            View.ReplaceCommands(WPFCommandNames.ListDelete, typeof(CustomerAddressDeleteCommand));
        }

        /// <summary>
        /// 视图列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.AddressType).UseListSetting(e => { e.HelpInfo = "地址快码类型(ADDRESS_TYPE)"; })
                .UseCatalogEditor(e => { e.CatalogType = SupplierAddress.CatalogAddressType;});
            View.Property(p => p.Name);
            View.Property(p => p.Country).UseRegionalEditor(w => w.ReloadDataOnPopping = true);
            View.Property(p => p.Province).UseRegionalEditor(w => { w.UpperLevelProperty = CustomerAddress.CountryProperty; w.ReloadDataOnPopping = true; });
            View.Property(p => p.City).UseRegionalEditor(w => { w.UpperLevelProperty = CustomerAddress.ProvinceProperty; w.UpperLevel2Property = CustomerAddress.CountryProperty; w.ReloadDataOnPopping = true; });
            View.Property(p => p.Area).UseRegionalEditor(w => { w.UpperLevelProperty = CustomerAddress.CityProperty; w.UpperLevel2Property = CustomerAddress.ProvinceProperty; w.ReloadDataOnPopping = true; });
            View.Property(p => p.Address);
            View.Property(p => p.Contacts);
            View.Property(p => p.Phone);
            View.Property(p => p.Fax);
            View.Property(p => p.Email);
            View.Property(p => p.ZipCode);
            View.Property(p => p.IsDefault).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.Remark);
            //View.Property(DataEntityExtension.UpdateByNameProperty).HasLabel("更新人").Readonly();
            //View.Property(DataEntity.UpdateDateProperty).Readonly();
        }
    }
}
