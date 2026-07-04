using SIE.CSM.Common;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.CSM._Extentions_;
using SIE.Web.CSM.Suppliers.Commands;

namespace SIE.Web.CSM.Customers
{
    /// <summary>
    /// 客户地址视图配置
    /// </summary>
    internal class CustomerAddressViewConfig : WebViewConfig<CustomerAddress>
    {
        /// <summary>
        /// 视图列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands().RemoveCommands(WebCommandNames.Save);
            View.ReplaceCommands(WebCommandNames.Add, typeof(AddAddressCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Edit, typeof(EditAddressCommand).FullName);
            //地址使用快码
            View.Property(p => p.AddressType).UseListSetting(e => { e.HelpInfo = "地址快码类型(ADDRESS_TYPE)"; }).UseCatalogEditor(e => { e.CatalogType = SupplierAddress.CatalogAddressType;e.CatalogReloadData = true; });
            View.Property(p => p.Name);
            View.Property(p => p.Country).UseDropDownEditor(() => RT.Service.Resolve<RegionalInfoController>().GetRegionDic(null, null)).ShowInList(width: 60);
            View.Property(p => p.Province).UseSupplierProvinceEditor(2, p =>
            {//省，初始化系统需要执行脚本才会有数据
                p.Editable = false;
                p.ValueField = RegionalInfo.RegionProperty.Name;
            }).UseListSetting(e => { e.HelpInfo = "显示选择国家的省份"; }).ShowInList(width: 80);
            View.Property(p => p.City).UseSupplierProvinceEditor(3, p =>
            {//城市
                p.Editable = false;
                p.ValueField = RegionalInfo.RegionProperty.Name;
            }).UseListSetting(e => { e.HelpInfo = "显示选择省份的地级市"; }).ShowInList(width: 80);
            View.Property(p => p.Area).UseSupplierProvinceEditor(4, p =>
            {//城区
                p.Editable = false;
                p.ValueField = RegionalInfo.RegionProperty.Name;
            }).UseListSetting(e => { e.HelpInfo = "显示选择地级市的城区"; }).ShowInList(width: 80);
            View.Property(p => p.Address);
            View.Property(p => p.Contacts);
            View.Property(p => p.Phone);
            View.Property(p => p.Fax);
            View.Property(p => p.Email);
            View.Property(p => p.ZipCode);
            View.Property(p => p.State).Readonly();
            View.Property(p => p.Remark);
            View.Property(p => p.FullAddress);
        }
    }
}
