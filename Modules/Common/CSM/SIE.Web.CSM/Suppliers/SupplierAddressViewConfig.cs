using SIE.CSM.Common;
using SIE.CSM.Suppliers;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.CSM._Extentions_;
using SIE.Web.CSM.Suppliers.Commands;

namespace SIE.Web.CSM.Suppliers
{
    /// <summary>
    /// 供应商地址视图配置
    /// </summary>
    internal class SupplierAddressViewConfig : WebViewConfig<SupplierAddress>
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();//行内编辑
        }

        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().RemoveCommands(WebCommandNames.Save);
            View.ReplaceCommands(WebCommandNames.Add, typeof(AddAddressCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Edit, typeof(EditAddressCommand).FullName);
            //地址快码
            View.Property(p => p.AddressType).UseListSetting(e => { e.HelpInfo = "地址快码类型(ADDRESS_TYPE)"; })
                .UseCatalogEditor(e => { e.CatalogType = SupplierAddress.CatalogAddressType;e.CatalogReloadData = true; });
            View.Property(p => p.Name);
            View.Property(p => p.Country).UseDropDownEditor(() => RT.Service.Resolve<RegionalInfoController>().GetRegionDic(null, null)).ShowInList(width: 60);
            View.Property(p => p.Province).UseSupplierProvinceEditor(2, p =>
            {//省，初始化需要执行脚本
                p.Editable = false;
                p.ValueField = RegionalInfo.RegionProperty.Name;
            }).UseListSetting(e => { e.HelpInfo = "显示选择国家的省份"; }).ShowInList(width: 80);
            View.Property(p => p.City).UseSupplierProvinceEditor(3, p =>
            {//城市
                p.Editable = false;
                p.ValueField = RegionalInfo.RegionProperty.Name;
            }).UseListSetting(e => { e.HelpInfo = "显示选择省份的地级市"; }).ShowInList(width: 80);
            View.Property(p => p.Area).UseSupplierProvinceEditor(4, p =>
            {//区域
                p.Editable = false;
                p.ValueField = RegionalInfo.RegionProperty.Name;
            }).UseListSetting(e => { e.HelpInfo = "显示选择地级市的城区"; }).ShowInList(width: 80);
            View.Property(p => p.Address);
            View.Property(p => p.Contacts);
            View.Property(p => p.Phone);
            View.Property(p => p.Fax);
            View.Property(p => p.EMail);
            View.Property(p => p.ZipCode).ShowInList(width: 70);
            View.Property(p => p.IsDefault).Readonly();
            View.Property(p => p.State).Readonly().ShowInList(width: 60);
            View.Property(p => p.Remark);
            View.Property(p => p.FullAddress);
        }
    }
}
