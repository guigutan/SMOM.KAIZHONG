using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Text.RegularExpressions;

namespace SIE.CSM.Customers
{
    /// <summary>
    /// 客户地址
    /// </summary>
    [ChildEntity, Serializable]
    [Label("客户地址")]
    [DisplayMember(nameof(CustomerAddress.Address))]
    public class CustomerAddress : BaseRegionalInfo, IStateEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomerAddress() { State = State.Enable; }

        #region 联系人 Contacts
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactsProperty = P<CustomerAddress>.Register(e => e.Contacts);

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
        public static readonly Property<string> PhoneProperty = P<CustomerAddress>.Register(e => e.Phone);

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
        /// </summary>、
        [Label("邮编")]
        public static readonly Property<string> ZipCodeProperty = P<CustomerAddress>.Register(e => e.ZipCode);

        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode
        {
            get { return GetProperty(ZipCodeProperty); }
            set { SetProperty(ZipCodeProperty, value); }
        }
        #endregion

        #region 电子邮箱 Email
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Label("电子邮箱")]
        public static readonly Property<string> EmailProperty = P<CustomerAddress>.Register(e => e.Email);

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email
        {
            get { return GetProperty(EmailProperty); }
            set { SetProperty(EmailProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<CustomerAddress>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 地址类型 AddressType
        /// <summary>
        /// 地址类型
        /// </summary>
        [Label("地址类型")]
        [Required]
        public static readonly Property<string> AddressTypeProperty = P<CustomerAddress>.Register(e => e.AddressType);

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
        [Label("公司名称")]
        [MaxLength(80)]
        [Required]
        public static readonly Property<string> NameProperty = P<CustomerAddress>.Register(e => e.Name);

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 传真 Fax
        /// <summary>
        /// 传真
        /// </summary>
        [Label("传真")]
        public static readonly Property<string> FaxProperty = P<CustomerAddress>.Register(e => e.Fax);

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
        [Label("备注")]
        [MaxLength(2000)]
        public static readonly Property<string> RemarkProperty = P<CustomerAddress>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 默认 IsDefault
        /// <summary>
        /// 默认
        /// </summary>
        [Label("默认")]
        public static readonly Property<bool> IsDefaultProperty = P<CustomerAddress>.Register(e => e.IsDefault);

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
        public static readonly Property<string> SourceKeyProperty = P<CustomerAddress>.Register(e => e.SourceKey);

        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        public string SourceKey
        {
            get { return this.GetProperty(SourceKeyProperty); }
            set { this.SetProperty(SourceKeyProperty, value); }
        }
        #endregion

        #region 客户与地址关系 Customer
        /// <summary>
        /// 客户与地址关系Id
        /// </summary>
        public static readonly IRefIdProperty CustomerIdProperty = P<CustomerAddress>.RegisterRefId(e => e.CustomerId, ReferenceType.Parent);

        /// <summary>
        /// 客户与地址关系Id
        /// </summary>
        public double? CustomerId
        {
            get { return (double?)GetRefNullableId(CustomerIdProperty); }
            set { SetRefNullableId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户与地址关系
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<CustomerAddress>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户与地址关系
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 客户地址 实体配置
    /// </summary>
    internal class CustomerAddressConfig : EntityConfig<CustomerAddress>
    {
        /// <summary>
        ///  员工电话号码和邮箱格式验证
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(CustomerAddress.PhoneProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^((0\d{2,3}-\d{7,8})|(1[35847]\d{9}))$"),
                MessageBuilder = (o) =>
                {
                    return "电话格式不正确".L10N();
                }
            });
            rules.AddRule(CustomerAddress.ZipCodeProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^\d{6}$"),
                MessageBuilder = (o) =>
                {
                    return "邮编格式不正确".L10N();
                }
            });
            rules.AddRule(CustomerAddress.EmailProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^([a-zA-Z0-9_\.-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$"),
                MessageBuilder = (o) =>
                {
                    return "邮箱格式不正确".L10N();
                }
            });
        }

        /// <summary>
        /// 客户地址数据库表名
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CSM_CUST_ADDR").MapAllProperties();
            Meta.Property(CustomerAddress.AddressProperty).ColumnMeta.HasLength(4000);
            Meta.Property(CustomerAddress.FullAddressProperty).ColumnMeta.HasLength(4000);
            Meta.Property(CustomerAddress.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}