using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Text.RegularExpressions;

namespace SIE.Warehouses
{
    /// <summary>
    /// 仓库地址
    /// </summary>
    [ChildEntity, Serializable]
    [Label("仓库地址")]
    [DisplayMember(nameof(WarehouseAddress.AddressType))]
    public partial class WarehouseAddress : BaseRegionalInfo, IStateEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WarehouseAddress() { State = State.Enable; }

        /// <summary>
        /// 快码类型：地址类型
        /// </summary>
        public const string CatalogAddressType = "ADDRESS_TYPE";

        #region 地址类型 AddressType
        /// <summary>
        /// 地址类型
        /// </summary>
        [Required]
        [Label("地址类型")]
        [MaxLength(80)]
        public static readonly Property<string> AddressTypeProperty = P<WarehouseAddress>.Register(e => e.AddressType);

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
        [Required]
        [MaxLength(240)]
        [Label("公司名称")]
        public static readonly Property<string> NameProperty = P<WarehouseAddress>.Register(e => e.Name);

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 电话 Phone
        /// <summary>
        /// 电话
        /// </summary>
        [Label("电话")]
        [MaxLength(80)]
        public static readonly Property<string> PhoneProperty = P<WarehouseAddress>.Register(e => e.Phone);

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
        [MaxLength(80)]
        [Label("传真")]
        public static readonly Property<string> FaxProperty = P<WarehouseAddress>.Register(e => e.Fax);

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax
        {
            get { return GetProperty(FaxProperty); }
            set { SetProperty(FaxProperty, value); }
        }
        #endregion

        #region 电子邮箱 Email
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Label("电子邮箱")]
        [MaxLength(80)]
        public static readonly Property<string> EmailProperty = P<WarehouseAddress>.Register(e => e.Email);

        /// <summary>
        /// 电子邮箱
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
        public static readonly Property<string> ZipCodeProperty = P<WarehouseAddress>.Register(e => e.ZipCode);

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
        public static readonly Property<string> RemarkProperty = P<WarehouseAddress>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("联系人")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<WarehouseAddress>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)GetRefId(EmployeeIdProperty); }
            set { SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<WarehouseAddress>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 员工名称 EmployeeName
        /// <summary>
        /// 员工名称
        /// </summary>
        [Label("员工名称")]
        public static readonly Property<string> EmployeeNameProperty = P<WarehouseAddress>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion


        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Source == ManagedPropertyChangedSource.FromUIOperating && e.Property == WarehouseAddress.EmployeeProperty)
            {
                if (Employee == null)
                {
                    Phone = null;
                    Email = null;
                }
                else
                {
                    if (Phone.IsNullOrEmpty())
                        Phone = Employee.Phone;
                    if (Email.IsNullOrEmpty())
                        Email = Employee.Email;
                }
            }

            if (e.Source == ManagedPropertyChangedSource.FromUIOperating && e.Property == WarehouseAddress.AddressProperty)
            {
                FullAddress = Country + Province + City + Area + Address.Trim();
            }
        }

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<WarehouseAddress>.RegisterRefId(e => e.WarehouseId, ReferenceType.Parent);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<WarehouseAddress>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }

        /// <summary>
        /// 启用/禁用
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<WarehouseAddress>.Register(e => e.State);

        /// <summary>
        /// 启用/禁用
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }

            set { SetProperty(StateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 仓库地址 实体配置
    /// </summary>
    internal class WarehouseAddressConfig : EntityConfig<WarehouseAddress>
    {
        /// <summary>
        ///  员工电话号码和邮箱格式验证
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(WarehouseAddress.PhoneProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^((0\d{2,3}-\d{7,8})|(1[35847]\d{9}))$"),
                MessageBuilder = (o) =>
                {
                    return "电话格式不正确".L10N();
                }
            });
            rules.AddRule(WarehouseAddress.ZipCodeProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^\d{6}$"),
                MessageBuilder = (o) =>
                {
                    return "邮编格式不正确".L10N();
                }
            });
            rules.AddRule(WarehouseAddress.EmailProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^([a-zA-Z0-9_\.-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$"),
                MessageBuilder = (o) =>
                {
                    return "邮箱格式不正确".L10N();
                }
            });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_ADDRESS").MapAllProperties();
            Meta.Property(WarehouseAddress.AddressProperty).ColumnMeta.HasLength(4000);
            Meta.Property(WarehouseAddress.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(WarehouseAddress.FullAddressProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}