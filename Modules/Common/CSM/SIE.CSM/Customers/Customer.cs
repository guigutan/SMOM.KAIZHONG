using SIE.Common;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Text.RegularExpressions;

namespace SIE.CSM.Customers
{
    /// <summary>
    /// 客户
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(CustomerCriteria))]
    [Label("客户")]
    [DisplayMember(nameof(Code))]
    public class Customer : DataEntity, IStateEntity
    {
        /// <summary>
        /// 快码类型：客户类型
        /// </summary>
        public const string CatalogCustomerType = "CUSTOMER_TYPE";

        /// <summary>
        /// 构造函数
        /// </summary>
        public Customer()
        {
            State = State.Enable;
            CustomerType = CustomerType.CUSTOMER;
        }

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [MaxLength(40)]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Customer>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 全称 Name
        /// <summary>
        /// 全称
        /// </summary>
        [Required]
        [MaxLength(40)]
        [NotDuplicate]
        [Label("全称")]
        public static readonly Property<string> NameProperty = P<Customer>.Register(e => e.Name);

        /// <summary>
        /// 全称
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
        [MaxLength(30)]
        [Label("简称")]
        public static readonly Property<string> ShortNameProperty = P<Customer>.Register(e => e.ShortName);

        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName
        {
            get { return GetProperty(ShortNameProperty); }
            set { SetProperty(ShortNameProperty, value); }
        }
        #endregion

        #region 所在区域 SalesArea
        /// <summary>
        /// 所在区域
        /// </summary>
        [MaxLength(30)]
        [Label("所在区域")]
        public static readonly Property<string> RegionProperty = P<Customer>.Register(e => e.Region);

        /// <summary>
        /// 所在区域
        /// </summary>
        public string Region
        {
            get { return GetProperty(RegionProperty); }
            set { SetProperty(RegionProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<Customer>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 英文名称 EnglishName
        /// <summary>
        /// 英文名称
        /// </summary>
        [Label("英文名称")]
        public static readonly Property<string> EnglishNameProperty = P<Customer>.Register(e => e.EnglishName);

        /// <summary>
        /// 英文名称
        /// </summary>
        public string EnglishName
        {
            get { return GetProperty(EnglishNameProperty); }
            set { SetProperty(EnglishNameProperty, value); }
        }
        #endregion

        #region 描述 EnglishName
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        [MaxLength(4000)]
        public static readonly Property<string> DescriptionProperty = P<Customer>.Register(e => e.Description);

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
        [Required]
        public static readonly Property<CustomerType> CustomerTypeProperty = P<Customer>.Register(e => e.CustomerType);

        /// <summary>
        /// 类型
        /// </summary>
        public CustomerType CustomerType
        {
            get { return GetProperty(CustomerTypeProperty); }
            set { SetProperty(CustomerTypeProperty, value); }
        }
        #endregion

        #region 己方编码 OwnCode
        /// <summary>
        /// 己方编码
        /// </summary>
        [Label("己方编码")]
        public static readonly Property<string> OwnCodeProperty = P<Customer>.Register(e => e.OwnCode);

        /// <summary>
        /// 己方编码
        /// </summary>
        public string OwnCode
        {
            get { return GetProperty(OwnCodeProperty); }
            set { SetProperty(OwnCodeProperty, value); }
        }
        #endregion

        #region 己方名称 OwnName
        /// <summary>
        /// 己方名称
        /// </summary>
        [Label("己方名称")]
        public static readonly Property<string> OwnNameProperty = P<Customer>.Register(e => e.OwnName);

        /// <summary>
        /// 己方名称
        /// </summary>
        public string OwnName
        {
            get { return GetProperty(OwnNameProperty); }
            set { SetProperty(OwnNameProperty, value); }
        }
        #endregion

        #region 税号 DutyParagraph
        /// <summary>
        /// 税号
        /// </summary>
        [Label("税号")]
        public static readonly Property<string> DutyParagraphProperty = P<Customer>.Register(e => e.DutyParagraph);

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
        public static readonly Property<string> ContactsProperty = P<Customer>.Register(e => e.Contacts);

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
        public static readonly Property<string> ContactsNumberProperty = P<Customer>.Register(e => e.ContactsNumber);

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
        [Label("联系地址")]
        public static readonly Property<string> ContactsAddressProperty = P<Customer>.Register(e => e.ContactsAddress);

        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContactsAddress
        {
            get { return GetProperty(ContactsAddressProperty); }
            set { SetProperty(ContactsAddressProperty, value); }
        }
        #endregion

        #region 电子邮箱 EMail
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Label("电子邮箱")]
        public static readonly Property<string> EMailProperty = P<Customer>.Register(e => e.EMail);

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string EMail
        {
            get { return GetProperty(EMailProperty); }
            set { SetProperty(EMailProperty, value); }
        }
        #endregion

        #region 邮编 PostalCode
        /// <summary>
        /// 邮编
        /// </summary>
        [Label("邮编")]
        public static readonly Property<string> ZipCodeProperty = P<Customer>.Register(e => e.ZipCode);

        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode
        {
            get { return GetProperty(ZipCodeProperty); }
            set { SetProperty(ZipCodeProperty, value); }
        }
        #endregion

        #region 来源 SourceType
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<SourceType> SourceTypeProperty = P<Customer>.Register(e => e.SourceType);

        /// <summary>
        /// 来源
        /// </summary>
        public SourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        [MaxLength(2000)]
        public static readonly Property<string> RemarkProperty = P<Customer>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 来源主键 SourceKey
        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        [Label("来源主键")]
        public static readonly Property<string> SourceKeyProperty = P<Customer>.Register(e => e.SourceKey);

        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        public string SourceKey
        {
            get { return this.GetProperty(SourceKeyProperty); }
            set { this.SetProperty(SourceKeyProperty, value); }
        }
        #endregion

        #region 客户与地址关系 AddressList
        /// <summary>
        /// 地址
        /// </summary>
        public static readonly ListProperty<EntityList<CustomerAddress>> CustomerAddressListProperty = P<Customer>.RegisterList(e => e.CustomerAddressList);

        /// <summary>
        /// 地址
        /// </summary>
        public EntityList<CustomerAddress> CustomerAddressList
        {
            get { return this.GetLazyList(CustomerAddressListProperty); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<Customer>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefNullableId(SupplierIdProperty); }
            set { SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<Customer>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<Customer>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<Customer>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="e">e</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            if (e.Source == ManagedPropertyChangedSource.FromUIOperating && e.Property.Equals(CustomerTypeProperty))
            {
                Supplier = null;
            }

            base.OnPropertyChanged(e);
        }
    }

    /// <summary>
    /// 客户扩展
    /// </summary>
    [RootEntity, Serializable]
    [Label("客户扩展")]
    public class CustomerUnit: Customer
    {

    }
    /// <summary>
    /// 客户 实体配置
    /// </summary>
    internal class CustomerConfig : EntityConfig<Customer>
    {
        ///// <summary>
        ///// 验证规则
        ///// </summary>
        ///// <param name="rules">电话号码验证规则</param>
        //protected override void AddValidations(IValidationDeclarer rules)
        //{
        //    base.AddValidations(rules);
        //    rules.AddRule(Customer.ContactsNumberProperty, new RegexMatchRule()
        //    {
        //        Regex = new Regex(@"^((0\d{2,3}-\d{7,8})|(1[3584]\d{9}))$"),
        //        MessageBuilder = (o) =>
        //        {
        //            return "联系电话格式不正确";
        //        }
        //    });
        //    rules.AddRule(Customer.ZipCodeProperty, new RegexMatchRule()
        //    {
        //        Regex = new Regex(@"^\d{6}$"),
        //        MessageBuilder = (o) =>
        //        {
        //            return "邮编格式不正确";
        //        }
        //    });
        //    rules.AddRule(Customer.EMailProperty, new RegexMatchRule()
        //    {
        //        Regex = new Regex(@"^([a-zA-Z0-9_\.-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$"),
        //        MessageBuilder = (o) =>
        //        {
        //            return "电子邮箱格式不正确";
        //        }
        //    });
        //}

        /// <summary>
        /// 数据库客户表名
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CSM_CUSTOMER").MapAllProperties();
            Meta.Property(Customer.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(Customer.DescriptionProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}