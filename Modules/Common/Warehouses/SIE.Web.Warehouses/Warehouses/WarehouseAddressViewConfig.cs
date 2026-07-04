using SIE.CSM.Common;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Common;
using SIE.Web.CSM._Extentions_;
using SIE.Web.CSM.Suppliers.Commands;
using SIE.Web.Warehouses.Commands;
using System.Collections.Generic;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 仓库地址
    /// </summary>
    internal class WarehouseAddressViewConfig : WebViewConfig<WarehouseAddress>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.RemoveCommands(WebCommandNames.Save);
            View.ReplaceCommands(WebCommandNames.Copy, typeof(WarehouseAddressCopyCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Add, typeof(AddAddressCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Edit, typeof(EditAddressCommand).FullName);
            View.Property(p => p.AddressType).UseAddressTypeCatalogEditor(e => e.CatalogType = WarehouseAddress.CatalogAddressType);
            View.Property(p => p.Name);
            View.Property(p => p.Country).UseDropDownEditor(() => RT.Service.Resolve<RegionalInfoController>().GetRegionDic(null, null)).ShowInList(width: 60);
            View.Property(p => p.Province).UseSupplierProvinceEditor(2, p =>
            {
                p.Editable = false;
                p.ValueField = RegionalInfo.RegionProperty.Name;
            }).UseListSetting(e => { e.HelpInfo = "显示选择国家的省份"; }).ShowInList(width: 80);
            View.Property(p => p.City).UseSupplierProvinceEditor(3, p =>
            {
                p.Editable = false;
                p.ValueField = RegionalInfo.RegionProperty.Name;
            }).UseListSetting(e => { e.HelpInfo = "显示选择省份的地级市"; }).ShowInList(width: 80);
            View.Property(p => p.Area).UseSupplierProvinceEditor(4, p =>
            {
                p.Editable = false;
                p.ValueField = RegionalInfo.RegionProperty.Name;
            }).UseListSetting(e => { e.HelpInfo = "显示选择地级市的城区"; }).ShowInList(width: 80);
            View.Property(p => p.Address);
            View.Property(p => p.Employee).UseEmployeeEditor((p, t) =>
            {
                var keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(t.Phone), nameof(t.Employee.Phone));
                keyValues.Add(nameof(t.Email), nameof(t.Employee.Email));
                p.DicLinkField = keyValues;
            }).HasLabel("联系人");
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
            View.Property(p => p.AddressType).UseCatalogEditor(e => { e.CatalogType = WarehouseAddress.CatalogAddressType; e.CatalogReloadData = true; })
                .UseListSetting(e => { e.HelpInfo = "地址快码类型(ADDRESS_TYPE)"; });
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