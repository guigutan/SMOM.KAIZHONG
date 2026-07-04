using SIE.CSM.Common;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Web.CSM._Extentions_;
using System;

namespace SIE.Web.CSM.Suppliers.ViewModels
{
    /// <summary>
    /// 区域
    /// </summary>
    [RootEntity, Serializable]
    [Label("区域")]
    public class RegionalViewModel : ViewModel
    {
        #region 国家 Country
        /// <summary>
        /// 国家
        /// </summary>
        [Label("国家")]
        public static readonly Property<string> CountryProperty = P<RegionalViewModel>.Register(e => e.Country);

        /// <summary>
        /// 国家
        /// </summary>
        public string Country
        {
            get { return GetProperty(CountryProperty); }
            set { SetProperty(CountryProperty, value); }
        }
        #endregion

        #region 省 Province
        /// <summary>
        /// 省
        /// </summary>
        [Label("省")]
        public static readonly Property<string> ProvinceProperty = P<RegionalViewModel>.Register(e => e.Province);

        /// <summary>
        /// 省
        /// </summary>
        public string Province
        {
            get { return GetProperty(ProvinceProperty); }
            set { SetProperty(ProvinceProperty, value); }
        }
        #endregion

        #region 市 City
        /// <summary>
        /// 市
        /// </summary>
        [Label("市")]
        public static readonly Property<string> CityProperty = P<RegionalViewModel>.Register(e => e.City);

        /// <summary>
        /// 市
        /// </summary>
        public string City
        {
            get { return GetProperty(CityProperty); }
            set { SetProperty(CityProperty, value); }
        }
        #endregion

        #region 区 Area
        /// <summary>
        /// 区
        /// </summary>
        [Label("区")]
        public static readonly Property<string> AreaProperty = P<RegionalViewModel>.Register(e => e.Area);

        /// <summary>
        /// 区
        /// </summary>
        public string Area
        {
            get { return GetProperty(AreaProperty); }
            set { SetProperty(AreaProperty, value); }
        }
        #endregion

        #region 详细地址 Address
        /// <summary>
        /// 详细地址
        /// </summary>
        [Label("详细地址")]
        public static readonly Property<string> AddressProperty = P<RegionalViewModel>.Register(e => e.Address);

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address
        {
            get { return this.GetProperty(AddressProperty); }
            set { this.SetProperty(AddressProperty, value); }
        }
        #endregion

        #region 地址 FullAddress
        /// <summary>
        /// 地址
        /// </summary>
        [Label("地址")]
        public static readonly Property<string> FullAddressProperty = P<RegionalViewModel>.Register(e => e.FullAddress);

        /// <summary>
        /// 地址
        /// </summary>
        public string FullAddress
        {
            get { return this.GetProperty(FullAddressProperty); }
            set { this.SetProperty(FullAddressProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 视图配置
    /// </summary>
    internal class RegionalViewModelViewConfig : WebViewConfig<RegionalViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.Country).UseSupplierProvinceEditor(1, p =>
            {
                p.ValueField = RegionalInfo.RegionProperty.Name;
            }).UseListSetting(e => { e.HelpInfo = "更换国家清空省份"; }).Cascade(p => p.Province, null);
            View.Property(p => p.Province).UseSupplierProvinceEditor(2, p =>
            {
                p.ValueField = RegionalInfo.RegionProperty.Name;
            }).UseListSetting(e => { e.HelpInfo = "更换省份清空地级市"; }).Cascade(p => p.City, null);
            View.Property(p => p.City).UseSupplierProvinceEditor(3, p =>
            {
                p.ValueField = RegionalInfo.RegionProperty.Name;
            }).UseListSetting(e => { e.HelpInfo = "更换地级市清空城区"; }).Cascade(p => p.Area, null);
            View.Property(p => p.Area).UseSupplierProvinceEditor(4, p =>
            {
                p.ValueField = RegionalInfo.RegionProperty.Name;
            }).UseListSetting(e => { e.HelpInfo = "更换城区清空详细地址"; }).Cascade(p => p.Address, null);
            View.Property(p => p.Address).UseMemoEditor();
        }
    }
}
