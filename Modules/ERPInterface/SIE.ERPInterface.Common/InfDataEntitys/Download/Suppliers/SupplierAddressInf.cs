using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 供应商地址中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("供应商地址中间表")]
    public partial class SupplierAddressInf : DownloadBaseEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<SupplierAddressInf>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 简称 ShortName
        /// <summary>
        /// 简称
        /// </summary>
        [Label("简称")]
        public static readonly Property<string> ShortNameProperty = P<SupplierAddressInf>.Register(e => e.ShortName);

        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName
        {
            get { return GetProperty(ShortNameProperty); }
            set { SetProperty(ShortNameProperty, value); }
        }
        #endregion

        #region 联系人 Contacts
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactsProperty = P<SupplierAddressInf>.Register(e => e.Contacts);

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts
        {
            get { return GetProperty(ContactsProperty); }
            set { SetProperty(ContactsProperty, value); }
        }
        #endregion

        #region 电话 Phone
        /// <summary>
        /// 电话
        /// </summary>
        [Label("电话")]
        public static readonly Property<string> PhoneProperty = P<SupplierAddressInf>.Register(e => e.Phone);

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone
        {
            get { return GetProperty(PhoneProperty); }
            set { SetProperty(PhoneProperty, value); }
        }
        #endregion

        #region 传真 Fax
        /// <summary>
        /// 传真
        /// </summary>
        [Label("传真")]
        public static readonly Property<string> FaxProperty = P<SupplierAddressInf>.Register(e => e.Fax);

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax
        {
            get { return GetProperty(FaxProperty); }
            set { SetProperty(FaxProperty, value); }
        }
        #endregion

        #region 邮编 ZipCode
        /// <summary>
        /// 邮编
        /// </summary>
        [Label("邮编")]
        public static readonly Property<string> ZipCodeProperty = P<SupplierAddressInf>.Register(e => e.ZipCode);

        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode
        {
            get { return GetProperty(ZipCodeProperty); }
            set { SetProperty(ZipCodeProperty, value); }
        }
        #endregion

        #region 地址类型 AddressType
        /// <summary>
        /// 地址类型
        /// </summary>
        [Label("地址类型")]
        public static readonly Property<string> AddressTypeProperty = P<SupplierAddressInf>.Register(e => e.AddressType);

        /// <summary>
        /// 地址类型
        /// </summary>
        public string AddressType
        {
            get { return GetProperty(AddressTypeProperty); }
            set { SetProperty(AddressTypeProperty, value); }
        }
        #endregion

        #region 国家 Country
        /// <summary>
        /// 国家
        /// </summary>
        [Label("国家")]
        public static readonly Property<string> CountryProperty = P<SupplierAddressInf>.Register(e => e.Country);

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
        public static readonly Property<string> ProvinceProperty = P<SupplierAddressInf>.Register(e => e.Province);

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
        public static readonly Property<string> CityProperty = P<SupplierAddressInf>.Register(e => e.City);

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
        public static readonly Property<string> AreaProperty = P<SupplierAddressInf>.Register(e => e.Area);

        /// <summary>
        /// 区
        /// </summary>
        public string Area
        {
            get { return GetProperty(AreaProperty); }
            set { SetProperty(AreaProperty, value); }
        }
        #endregion

        #region 详细地址 DetailAddress
        /// <summary>
        /// 详细地址
        /// </summary>
        [MaxLength(1000)]
        [Label("详细地址")]
        public static readonly Property<string> DetailAddressProperty = P<SupplierAddressInf>.Register(e => e.DetailAddress);

        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress
        {
            get { return GetProperty(DetailAddressProperty); }
            set { SetProperty(DetailAddressProperty, value); }
        }
        #endregion

        #region 电子邮箱 Email
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Label("电子邮箱")]
        public static readonly Property<string> EmailProperty = P<SupplierAddressInf>.Register(e => e.Email);

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email
        {
            get { return GetProperty(EmailProperty); }
            set { SetProperty(EmailProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<SupplierAddressInf>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<SupplierAddressInf>.Register(e => e.SupplierCode);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return GetProperty(SupplierCodeProperty); }
            set { SetProperty(SupplierCodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 供应商地址中间表 实体配置
    /// </summary>
    internal class SupplierAddressInfConfig : EntityConfig<SupplierAddressInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_SUPPLIER_ADDR").MapAllProperties();
            Meta.Property(SupplierAddressInf.DetailAddressProperty).ColumnMeta.HasLength(2000);
            Meta.Property(SupplierAddressInf.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}