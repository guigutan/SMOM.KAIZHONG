using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 供应商中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("供应商中间表")]
    public partial class SupplierInf : DownloadBaseEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<SupplierInf>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<SupplierInf>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<SupplierInf>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<string> TypeProperty = P<SupplierInf>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 简称 ShortName
        /// <summary>
        /// 简称
        /// </summary>
        [Label("简称")]
        public static readonly Property<string> ShortNameProperty = P<SupplierInf>.Register(e => e.ShortName);

        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName
        {
            get { return GetProperty(ShortNameProperty); }
            set { SetProperty(ShortNameProperty, value); }
        }
        #endregion

        #region 英文名称 EnglishName
        /// <summary>
        /// 英文名称
        /// </summary>
        [Label("英文名称")]
        public static readonly Property<string> EnglishNameProperty = P<SupplierInf>.Register(e => e.EnglishName);

        /// <summary>
        /// 英文名称
        /// </summary>
        public string EnglishName
        {
            get { return GetProperty(EnglishNameProperty); }
            set { SetProperty(EnglishNameProperty, value); }
        }
        #endregion

        #region 所在区域 Region
        /// <summary>
        /// 所在区域
        /// </summary>
        [Label("所在区域")]
        public static readonly Property<string> RegionProperty = P<SupplierInf>.Register(e => e.Region);

        /// <summary>
        /// 所在区域
        /// </summary>
        public string Region
        {
            get { return GetProperty(RegionProperty); }
            set { SetProperty(RegionProperty, value); }
        }
        #endregion

        #region 税号 DutyParagraph
        /// <summary>
        /// 税号
        /// </summary>
        [Label("税号")]
        public static readonly Property<string> DutyParagraphProperty = P<SupplierInf>.Register(e => e.DutyParagraph);

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
        public static readonly Property<string> ContactsProperty = P<SupplierInf>.Register(e => e.Contacts);

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts
        {
            get { return GetProperty(ContactsProperty); }
            set { SetProperty(ContactsProperty, value); }
        }
        #endregion

        #region 联系电话 ContactNumber
        /// <summary>
        /// 联系电话
        /// </summary>
        [Label("联系电话")]
        public static readonly Property<string> ContactNumberProperty = P<SupplierInf>.Register(e => e.ContactNumber);

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNumber
        {
            get { return GetProperty(ContactNumberProperty); }
            set { SetProperty(ContactNumberProperty, value); }
        }
        #endregion

        #region 联系地址 ContactAddress
        /// <summary>
        /// 联系地址
        /// </summary>
        [MaxLength(400)]
        [Label("联系地址")]
        public static readonly Property<string> ContactAddressProperty = P<SupplierInf>.Register(e => e.ContactAddress);

        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContactAddress
        {
            get { return GetProperty(ContactAddressProperty); }
            set { SetProperty(ContactAddressProperty, value); }
        }
        #endregion

        #region 电子邮件 Email
        /// <summary>
        /// 电子邮件
        /// </summary>
        [Label("电子邮件")]
        public static readonly Property<string> EmailProperty = P<SupplierInf>.Register(e => e.Email);

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email
        {
            get { return GetProperty(EmailProperty); }
            set { SetProperty(EmailProperty, value); }
        }
        #endregion

        #region 邮编 ZipCode
        /// <summary>
        /// 邮编
        /// </summary>
        [Label("邮编")]
        public static readonly Property<string> ZipCodeProperty = P<SupplierInf>.Register(e => e.ZipCode);

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
        public static readonly Property<string> RemarkProperty = P<SupplierInf>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 是否门户 IsPortal
        /// <summary>
        /// 是否门户
        /// </summary>
        [Label("是否门户")]
        public static readonly Property<bool> IsPortalProperty = P<SupplierInf>.Register(e => e.IsPortal);

        /// <summary>
        /// 是否门户
        /// </summary>
        public bool IsPortal
        {
            get { return this.GetProperty(IsPortalProperty); }
            set { this.SetProperty(IsPortalProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 供应商中间表 实体配置
    /// </summary>
    internal class SupplierInfConfig : EntityConfig<SupplierInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_SUPPLIER").MapAllProperties();
            Meta.Property(SupplierInf.ContactAddressProperty).ColumnMeta.HasLength(800);
            Meta.Property(SupplierInf.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}