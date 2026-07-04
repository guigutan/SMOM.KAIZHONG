using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Text.RegularExpressions;

namespace SIE.CSM.Suppliers
{
    /// <summary>
    /// 供应商地址
    /// </summary>
    [ChildEntity, Serializable]
    [Label("供应商地址")]
    [DisplayMember(nameof(SupplierAddress.Name))]
    public partial class SupplierAddress : BaseRegionalInfo, IStateEntity
    {
        /// <summary>
        /// 快码类型：地址类型
        /// </summary>
        public const string CatalogAddressType = "ADDRESS_TYPE";

        /// <summary>
        /// 初始化数据
        /// </summary>
        public SupplierAddress() { State = State.Enable; }

        #region 业务实体ID OrgId
        /// <summary>
        /// 业务实体ID
        /// </summary>
        [Label("业务实体ID")]
        public static readonly Property<int> OrgIdProperty = P<SupplierAddress>.Register(e => e.OrgId);

        /// <summary>
        /// 业务实体ID
        /// </summary>
        public int OrgId
        {
            get { return GetProperty(OrgIdProperty); }
            set { SetProperty(OrgIdProperty, value); }
        }
        #endregion

        #region 公司名称 Name
        /// <summary>
        /// 公司名称
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("公司名称")]
        public static readonly Property<string> NameProperty = P<SupplierAddress>.Register(e => e.Name);

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 联系人 Contacts
        /// <summary>
        /// 联系人
        /// </summary>
        [Required]
        [MaxLength(30)]
        [Label("联系人")]
        public static readonly Property<string> ContactsProperty = P<SupplierAddress>.Register(e => e.Contacts);

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
        public static readonly Property<string> PhoneProperty = P<SupplierAddress>.Register(e => e.Phone);

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
        public static readonly Property<string> FaxProperty = P<SupplierAddress>.Register(e => e.Fax);

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
        public static readonly Property<string> ZipCodeProperty = P<SupplierAddress>.Register(e => e.ZipCode);

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
        [Required]
        [Label("地址类型")]
        public static readonly Property<string> AddressTypeProperty = P<SupplierAddress>.Register(e => e.AddressType);

        /// <summary>
        /// 地址类型
        /// </summary>
        public string AddressType
        {
            get { return GetProperty(AddressTypeProperty); }
            set { SetProperty(AddressTypeProperty, value); }
        }
        #endregion

        #region 电子邮箱 EMail
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Label("电子邮箱")]
        public static readonly Property<string> EMailProperty = P<SupplierAddress>.Register(e => e.EMail);

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string EMail
        {
            get { return GetProperty(EMailProperty); }
            set { SetProperty(EMailProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<SupplierAddress>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<SupplierAddress>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 默认 IsDefault
        /// <summary>
        /// 默认
        /// </summary>
        [Label("默认")]
        public static readonly Property<bool> IsDefaultProperty = P<SupplierAddress>.Register(e => e.IsDefault);

        /// <summary>
        /// 默认
        /// </summary>
        public bool IsDefault
        {
            get { return GetProperty(IsDefaultProperty); }
            set { SetProperty(IsDefaultProperty, value); }
        }
        #endregion

        #region 来源主键 SourceKey
        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        [Label("来源主键")]
        public static readonly Property<string> SourceKeyProperty = P<SupplierAddress>.Register(e => e.SourceKey);

        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        public string SourceKey
        {
            get { return this.GetProperty(SourceKeyProperty); }
            set { this.SetProperty(SourceKeyProperty, value); }
        }
        #endregion

        #region 供应商与地址关系 Supplier
        /// <summary>
        /// 供应商与地址关系Id
        /// </summary>
        [Label("供应商与地址")]
        public static readonly IRefIdProperty SupplierIdProperty = P<SupplierAddress>.RegisterRefId(e => e.SupplierId, ReferenceType.Parent);

        /// <summary>
        /// 供应商与地址关系Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefNullableId(SupplierIdProperty); }
            set { SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商与地址关系
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<SupplierAddress>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商与地址关系
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 供应商地址 实体配置
    /// </summary>
    internal class SupplierAddressConfig : EntityConfig<SupplierAddress>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="rules">电话号码验证规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(SupplierAddress.PhoneProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^((0\d{2,3}-\d{7,8})|(1[35847]\d{9}))$"),
                MessageBuilder = (o) =>
                {
                    return "电话号码格式不正确".L10N();
                }
            });
            rules.AddRule(SupplierAddress.ZipCodeProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^\d{6}$"),
                MessageBuilder = (o) =>
                {
                    return "邮政编码格式不正确".L10N();
                }
            });
            rules.AddRule(SupplierAddress.EMailProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^([a-zA-Z0-9_\.-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$"),
                MessageBuilder = (o) =>
                {
                    return "电子邮箱格式不正确".L10N();
                }
            });
        }

        /// <summary>
        /// 数据库表名
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CSM_SUPPLIER_ADDR").MapAllProperties();
            Meta.Property(SupplierAddress.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(SupplierAddress.AddressProperty).ColumnMeta.HasLength(4000);
            Meta.Property(SupplierAddress.FullAddressProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}