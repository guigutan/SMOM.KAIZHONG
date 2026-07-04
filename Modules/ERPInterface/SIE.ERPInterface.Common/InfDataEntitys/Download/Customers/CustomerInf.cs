using SIE.CSM.Customers;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 客户中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("客户中间表")]
    public partial class CustomerInf : DownloadBaseEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<CustomerInf>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<CustomerInf>.Register(e => e.Name);

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
        public static readonly Property<string> ShortNameProperty = P<CustomerInf>.Register(e => e.ShortName);

        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName
        {
            get { return GetProperty(ShortNameProperty); }
            set { SetProperty(ShortNameProperty, value); }
        }
        #endregion

        #region 所在区域 Region
        /// <summary>
        /// 所在区域
        /// </summary>
        [Label("所在区域")]
        public static readonly Property<string> RegionProperty = P<CustomerInf>.Register(e => e.Region);

        /// <summary>
        /// 所在区域
        /// </summary>
        public string Region
        {
            get { return GetProperty(RegionProperty); }
            set { SetProperty(RegionProperty, value); }
        }
        #endregion

        #region 英文名称 EnglishName
        /// <summary>
        /// 英文名称
        /// </summary>
        [Label("英文名称")]
        public static readonly Property<string> EnglishNameProperty = P<CustomerInf>.Register(e => e.EnglishName);

        /// <summary>
        /// 英文名称
        /// </summary>
        public string EnglishName
        {
            get { return GetProperty(EnglishNameProperty); }
            set { SetProperty(EnglishNameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(4000)]
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<CustomerInf>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 类型 CustomerType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<CustomerType> CustomerTypeProperty = P<CustomerInf>.Register(e => e.CustomerType);

        /// <summary>
        /// 类型
        /// </summary>
        public CustomerType CustomerType
        {
            get { return GetProperty(CustomerTypeProperty); }
            set { SetProperty(CustomerTypeProperty, value); }
        }
        #endregion

        #region 税号 DutyParagraph
        /// <summary>
        /// 税号
        /// </summary>
        [Label("税号")]
        public static readonly Property<string> DutyParagraphProperty = P<CustomerInf>.Register(e => e.DutyParagraph);

        /// <summary>
        /// 税号
        /// </summary>
        public string DutyParagraph
        {
            get { return GetProperty(DutyParagraphProperty); }
            set { SetProperty(DutyParagraphProperty, value); }
        }
        #endregion

        #region 联系人 Contacts
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactsProperty = P<CustomerInf>.Register(e => e.Contacts);

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts
        {
            get { return GetProperty(ContactsProperty); }
            set { SetProperty(ContactsProperty, value); }
        }
        #endregion

        #region 联系电话 ContactsNumber
        /// <summary>
        /// 联系电话
        /// </summary>
        [Label("联系电话")]
        public static readonly Property<string> ContactsNumberProperty = P<CustomerInf>.Register(e => e.ContactsNumber);

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactsNumber
        {
            get { return GetProperty(ContactsNumberProperty); }
            set { SetProperty(ContactsNumberProperty, value); }
        }
        #endregion

        #region 联系地址 ContactsAddress
        /// <summary>
        /// 联系地址
        /// </summary>
        [MaxLength(1000)]
        [Label("联系地址")]
        public static readonly Property<string> ContactsAddressProperty = P<CustomerInf>.Register(e => e.ContactsAddress);

        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContactsAddress
        {
            get { return GetProperty(ContactsAddressProperty); }
            set { SetProperty(ContactsAddressProperty, value); }
        }
        #endregion

        #region 电子邮件 EMail
        /// <summary>
        /// 电子邮件
        /// </summary>
        [Label("电子邮件")]
        public static readonly Property<string> EMailProperty = P<CustomerInf>.Register(e => e.EMail);

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string EMail
        {
            get { return GetProperty(EMailProperty); }
            set { SetProperty(EMailProperty, value); }
        }
        #endregion

        #region 邮编 ZipCode
        /// <summary>
        /// 邮编
        /// </summary>
        [Label("邮编")]
        public static readonly Property<string> ZipCodeProperty = P<CustomerInf>.Register(e => e.ZipCode);

        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode
        {
            get { return GetProperty(ZipCodeProperty); }
            set { SetProperty(ZipCodeProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<CustomerInf>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 已方编码 OwnCode
        /// <summary>
        /// 已方编码
        /// </summary>
        [Label("已方编码")]
        public static readonly Property<string> OwnCodeProperty = P<CustomerInf>.Register(e => e.OwnCode);

        /// <summary>
        /// 已方编码
        /// </summary>
        public string OwnCode
        {
            get { return GetProperty(OwnCodeProperty); }
            set { SetProperty(OwnCodeProperty, value); }
        }
        #endregion

        #region 已方名称 OwnName
        /// <summary>
        /// 已方名称
        /// </summary>
        [Label("已方名称")]
        public static readonly Property<string> OwnNameProperty = P<CustomerInf>.Register(e => e.OwnName);

        /// <summary>
        /// 已方名称
        /// </summary>
        public string OwnName
        {
            get { return GetProperty(OwnNameProperty); }
            set { SetProperty(OwnNameProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 客户中间表 实体配置
    /// </summary>
    internal class CustomerInfConfig : EntityConfig<CustomerInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_CUSTOMER").MapAllProperties();
            Meta.Property(CustomerInf.DescriptionProperty).ColumnMeta.HasLength(4000);
            Meta.Property(CustomerInf.ContactsAddressProperty).ColumnMeta.HasLength(2000);
            Meta.Property(CustomerInf.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(CustomerInf.CodeProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}