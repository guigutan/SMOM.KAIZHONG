using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 客户地址中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("客户地址中间表")]
    public partial class CustomerAddressInf : DownloadBaseEntity
    {
        #region 详细地址 Address
        /// <summary>
        /// 详细地址
        /// </summary>
        [MaxLength(1000)]
        [Label("详细地址")]
        public static readonly Property<string> AddressProperty = P<CustomerAddressInf>.Register(e => e.Address);

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address
        {
            get { return GetProperty(AddressProperty); }
            set { SetProperty(AddressProperty, value); }
        }
        #endregion

        #region 联系人 Contacts
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactsProperty = P<CustomerAddressInf>.Register(e => e.Contacts);

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
        public static readonly Property<string> PhoneProperty = P<CustomerAddressInf>.Register(e => e.Phone);

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone
        {
            get { return GetProperty(PhoneProperty); }
            set { SetProperty(PhoneProperty, value); }
        }
        #endregion

        #region 邮编 ZipCode
        /// <summary>
        /// 邮编
        /// </summary>
        [Label("邮编")]
        public static readonly Property<string> ZipCodeProperty = P<CustomerAddressInf>.Register(e => e.ZipCode);

        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode
        {
            get { return GetProperty(ZipCodeProperty); }
            set { SetProperty(ZipCodeProperty, value); }
        }
        #endregion

        #region 邮箱 Email
        /// <summary>
        /// 邮箱
        /// </summary>
        [Label("邮箱")]
        public static readonly Property<string> EmailProperty = P<CustomerAddressInf>.Register(e => e.Email);

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            get { return GetProperty(EmailProperty); }
            set { SetProperty(EmailProperty, value); }
        }
        #endregion

        #region 地址类型 AddressType
        /// <summary>
        /// 地址类型
        /// </summary>
        [Label("地址类型")]
        public static readonly Property<string> AddressTypeProperty = P<CustomerAddressInf>.Register(e => e.AddressType);

        /// <summary>
        /// 地址类型
        /// </summary>
        public string AddressType
        {
            get { return GetProperty(AddressTypeProperty); }
            set { SetProperty(AddressTypeProperty, value); }
        }
        #endregion

        #region 公司名称 Name
        /// <summary>
        /// 公司名称
        /// </summary>
        [MaxLength(400)]
        [Label("公司名称")]
        public static readonly Property<string> NameProperty = P<CustomerAddressInf>.Register(e => e.Name);

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 国家 Country
        /// <summary>
        /// 国家
        /// </summary>
        [Label("国家")]
        public static readonly Property<string> CountryProperty = P<CustomerAddressInf>.Register(e => e.Country);

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
        public static readonly Property<string> ProvinceProperty = P<CustomerAddressInf>.Register(e => e.Province);

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
        public static readonly Property<string> CityProperty = P<CustomerAddressInf>.Register(e => e.City);

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
        public static readonly Property<string> AreaProperty = P<CustomerAddressInf>.Register(e => e.Area);

        /// <summary>
        /// 区
        /// </summary>
        public string Area
        {
            get { return GetProperty(AreaProperty); }
            set { SetProperty(AreaProperty, value); }
        }
        #endregion

        #region 传真 Fax
        /// <summary>
        /// 传真
        /// </summary>
        [Label("传真")]
        public static readonly Property<string> FaxProperty = P<CustomerAddressInf>.Register(e => e.Fax);

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax
        {
            get { return GetProperty(FaxProperty); }
            set { SetProperty(FaxProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<CustomerAddressInf>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<CustomerAddressInf>.Register(e => e.CustomerCode);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return GetProperty(CustomerCodeProperty); }
            set { SetProperty(CustomerCodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 客户地址中间表 实体配置
    /// </summary>
    internal class CustomerAddressInfConfig : EntityConfig<CustomerAddressInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_CUST_ADDR").MapAllProperties();
            Meta.Property(CustomerAddressInf.AddressProperty).ColumnMeta.HasLength(2000);
            Meta.Property(CustomerAddressInf.NameProperty).ColumnMeta.HasLength(800);
            Meta.Property(CustomerAddressInf.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}