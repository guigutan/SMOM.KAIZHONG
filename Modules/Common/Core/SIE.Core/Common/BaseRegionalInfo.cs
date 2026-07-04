using SIE.Domain;
using SIE.ManagedProperty;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Common
{
    /// <summary>
    /// 区域地址信息
    /// </summary>
    [RootEntity, Serializable]
    [Label("区域")]
    public class BaseRegionalInfo : DataEntity
    {
        #region 国家 Country
        /// <summary>
        /// 国家
        /// </summary>
        [Label("国家")]
        public static readonly Property<string> CountryProperty = P<BaseRegionalInfo>.Register(e => e.Country);

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
        public static readonly Property<string> ProvinceProperty = P<BaseRegionalInfo>.Register(e => e.Province);

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
        public static readonly Property<string> CityProperty = P<BaseRegionalInfo>.Register(e => e.City);

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
        public static readonly Property<string> AreaProperty = P<BaseRegionalInfo>.Register(e => e.Area);

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
        [Required]
        [Label("详细地址")]
        [MaxLength(2000)]
        public static readonly Property<string> AddressProperty = P<BaseRegionalInfo>.Register(e => e.Address);

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address
        {
            get { return GetProperty(AddressProperty); }
            set { SetProperty(AddressProperty, value); }
        }
        #endregion

        #region 地址 FullAddress
        /// <summary>
        /// 地址
        /// </summary>
        [Label("地址")]
        [MaxLength(2000)]
        public static readonly Property<string> FullAddressProperty = P<BaseRegionalInfo>.Register(e => e.FullAddress);

        /// <summary>
        /// 地址
        /// </summary>
        public string FullAddress
        {
            get { return this.GetProperty(FullAddressProperty); }
            set { this.SetProperty(FullAddressProperty, value); }
        }
        #endregion

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            if (e.Property.Name == CountryProperty.Name)
                Province = null;
            if (e.Property.Name == ProvinceProperty.Name)
                City = null;
            if (e.Property.Name == CityProperty.Name)
                Area = null;
            if (e.Property.Name == AreaProperty.Name)
                Address = null;
            if (e.Property.Name == AddressProperty.Name)
                FullAddress = string.Concat(Country, Province, City, Area, Address);

            base.OnPropertyChanged(e);
        }
    }
}
