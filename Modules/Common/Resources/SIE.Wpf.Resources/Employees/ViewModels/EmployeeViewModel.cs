using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Security.Authentications;
using SIE.Wpf.Editors;
using SIE.Wpf.Resources.Employees.Commands;
using System;
using System.Text.RegularExpressions;

namespace SIE.Wpf.Resources.Employees.ViewModels
{
    /// <summary>
    /// 添加员工模板
    /// </summary>
    [Serializable, RootEntity]
    public class EmployeeViewModel : ViewModel
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public EmployeeViewModel() { }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="employee">Employee</param>
        public EmployeeViewModel(Employee employee)
        {
            Employee = employee;
        }

        #region 工号 Code
        /// <summary>
        /// 工号
        /// </summary> 
        [Label("工号")]
        [Required]
        public static readonly Property<string> CodeProperty = P<EmployeeViewModel>.Register(e => e.Code);

        /// <summary>
        /// 工号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion


        #region 姓名 Name
        /// <summary>
        /// 姓名
        /// </summary> 
        [Label("姓名")]
        [Required]
        public static readonly Property<string> NameProperty = P<EmployeeViewModel>.Register(e => e.Name);

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 电话号码 Phone
        /// <summary>
        /// 电话号码
        /// </summary>
        [Label("电话号码")]
        public static readonly Property<string> PhoneProperty = P<EmployeeViewModel>.Register(e => e.Phone);

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone
        {
            get { return GetProperty(PhoneProperty); }
            set { SetProperty(PhoneProperty, value); }
        }
        #endregion

        #region 电子邮箱 Email
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Label("电子邮箱")]
        public static readonly Property<string> EmailProperty = P<EmployeeViewModel>.Register(e => e.Email);

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email
        {
            get { return GetProperty(EmailProperty); }
            set { SetProperty(EmailProperty, value); }
        }
        #endregion


        /// <summary>
        /// 接收更改
        /// </summary>
        public void AcceptChanges()
        {
            Employee.WorkGroup = WorkGroup;
            Employee.EmployeeGroup = EmployeeGroup;
        }

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组")]
        public static readonly IRefIdProperty WorkGroupIdProperty = P<EmployeeViewModel>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double? WorkGroupId
        {
            get { return (double?)GetRefNullableId(WorkGroupIdProperty); }
            set { SetRefNullableId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> WorkGroupProperty = P<EmployeeViewModel>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public WorkGroup WorkGroup
        {
            get { return GetRefEntity(WorkGroupProperty); }
            set { SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工
        /// </summary>
        [Label("员工")]
        public static readonly Property<Employee> EmployeeProperty = P<EmployeeViewModel>.Register(e => e.Employee);

        /// <summary>
        /// 员工
        /// </summary>

        public Employee Employee
        {
            get { return this.GetProperty(EmployeeProperty); }
            set { this.SetProperty(EmployeeProperty, value); }
        }
        #endregion

        #region 入职时间 EmployeeHireDate
        /// <summary>
        /// 入职时间
        /// </summary>
        [Label("入职时间")]
        public static readonly Property<DateTime?> EmployeeHireDateProperty = P<EmployeeViewModel>.RegisterView(e => e.EmployeeHireDate, p => p.Employee.HireDate);

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? EmployeeHireDate
        {
            get { return this.GetProperty(EmployeeHireDateProperty); }
        }
        #endregion

        #region 注释 EmployeeRemark
        /// <summary>
        /// 注释
        /// </summary>
        [Label("注释")]
        public static readonly Property<string> EmployeeRemarkProperty = P<EmployeeViewModel>.RegisterView(e => e.EmployeeRemark, p => p.Employee.Remark);

        /// <summary>
        /// 注释
        /// </summary>
        public string EmployeeRemark
        {
            get { return this.GetProperty(EmployeeRemarkProperty); }
        }
        #endregion

        #region 员工状态 EmployeeStatus
        /// <summary>
        /// 员工状态
        /// </summary>
        [Label("员工状态")]
        public static readonly Property<SIE.Resources.EmployeeStatus> EmployeeStatusProperty = P<EmployeeViewModel>.RegisterView(e => e.EmployeeStatus, p => p.Employee.EmployeeStatus);

        /// <summary>
        /// 员工状态
        /// </summary>
        public SIE.Resources.EmployeeStatus EmployeeStatus
        {
            get { return this.GetProperty(EmployeeStatusProperty); }
        }
        #endregion

        #region 性别 EmployeeSex
        /// <summary>
        /// 性别
        /// </summary>
        [Label("性别")]
        public static readonly Property<Sex> EmployeeSexProperty = P<EmployeeViewModel>.RegisterView(e => e.EmployeeSex, p => p.Employee.Sex);

        /// <summary>
        /// 性别
        /// </summary>
        public Sex EmployeeSex
        {
            get { return this.GetProperty(EmployeeSexProperty); }
        }
        #endregion



        #region 员工组 EmployeeGroup
        /// <summary>
        /// 员工组Id
        /// </summary>
        [Label("员工组")]
        public static readonly IRefIdProperty EmployeeGroupIdProperty = P<EmployeeViewModel>.RegisterRefId(e => e.EmployeeGroupId, ReferenceType.Normal);

        /// <summary>
        /// 员工组Id
        /// </summary>
        public double? EmployeeGroupId
        {
            get { return (double?)GetRefNullableId(EmployeeGroupIdProperty); }
            set { SetRefNullableId(EmployeeGroupIdProperty, value); }
        }

        /// <summary>
        /// 员工组
        /// </summary>
        public static readonly RefEntityProperty<EmployeeGroup> EmployeeGroupProperty = P<EmployeeViewModel>.RegisterRef(e => e.EmployeeGroup, EmployeeGroupIdProperty);

        /// <summary>
        /// 员工组
        /// </summary>
        public EmployeeGroup EmployeeGroup
        {
            get { return GetRefEntity(EmployeeGroupProperty); }
            set { SetRefEntity(EmployeeGroupProperty, value); }
        }
        #endregion

        #region UserCode 用户编码
        /// <summary>
        /// 用户编码
        /// </summary>
        [Required]
        [Label("用户编码")]
        public static readonly Property<string> UserCodeProperty = P<EmployeeViewModel>.Register(e => e.UserCode);

        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserCode
        {
            get { return this.GetProperty(UserCodeProperty); }
            set { this.SetProperty(UserCodeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 认证方式
        /// </summary>
        [Required]
        [Label("认证方式")]
        public static readonly Property<string> AuthenticateModeProperty = P<EmployeeViewModel>.Register(e => e.AuthenticateMode);

        /// <summary>
        /// 认证方式
        /// </summary>
        public string AuthenticateMode
        {
            get { return this.GetProperty(AuthenticateModeProperty); }
            set { this.SetProperty(AuthenticateModeProperty, value); }
        }

        #region CreateAccount 创建账户
        /// <summary>
        /// 创建账户
        /// </summary>
        [Label("创建账户")]
        public static readonly Property<bool> CreateAccountProperty = P<EmployeeViewModel>.Register(e => e.CreateAccount, (o, e) =>
        {
            var owner = o as EmployeeViewModel;
            if (e.NewValue.ConvertTo<bool>())
            {
                owner.UserCode = owner.Employee.Code;
            }
            else
            {
                owner.UserCode = string.Empty;
            }
        });

        /// <summary>
        /// 创建账户
        /// </summary>
        public bool CreateAccount
        {
            get { return this.GetProperty(CreateAccountProperty); }
            set { this.SetProperty(CreateAccountProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 员工配置
    /// </summary>
    internal class EmployeeConfig : EntityConfig<EmployeeViewModel>
    {
        /// <summary>
        ///  员工电话号码和邮箱格式验证
        /// </summary>
        /// <param name="rules">IValidationDeclarer</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            //工号、姓名不为空
            rules.AddRule(EmployeeViewModel.CodeProperty, new RequiredRule());
            rules.AddRule(EmployeeViewModel.NameProperty, new RequiredRule());
            rules.AddRule(EmployeeViewModel.PhoneProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^((0\d{2,3}-\d{7,8})|(1[3584]\d{9}))$"),
                MessageBuilder = (o) =>
                {
                    return "电话号码格式不正确".L10N();
                }
            });
            rules.AddRule(EmployeeViewModel.EmailProperty, new RegexMatchRule()
            {
                Regex = new Regex(@"^([a-zA-Z0-9])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$"),
                MessageBuilder = (o) =>
                {
                    return "邮箱格式不正确".L10N();
                }
            });
            base.AddValidations(rules);
        }
    }

    /// <summary>
    /// 员工新增视图配置
    /// </summary>
    internal class EmployeeAddViewModelViewConfig : WPFViewConfig<EmployeeViewModel>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(Employee));
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            //View.UseDefaultCommands().RemoveCommands(WPFCommandNames.FormCopy);
            //View.ReplaceCommands(WPFCommandNames.FormAdd, typeof(EmployeeViewModelAddCommand));
            //View.ReplaceCommands(WPFCommandNames.FormSave, typeof(EmployeeSaveCommand));
            View.UseCommands(typeof(EmployeeSaveCommand), typeof(EmployeeFormAddCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly(false);
                View.Property(p => p.Employee.Photo).UseImageEditor().UseFormSetting(p => { p.RowSpan = 8; }).Readonly(false).HasLabel("照片");
                View.Property(p => p.Name).Readonly(false);
                View.Property(p => p.EmployeeGroup).Readonly(false);
                View.Property(p => p.WorkGroup).Readonly(false);
                View.Property(p => p.Employee.HireDate).Readonly(false).UseEditor(WPFEditorNames.DateTime).HasLabel("入职时间");
                View.Property(p => p.Phone).Readonly(false);
                View.Property(p => p.Email).Readonly(false);
                //View.Property(p => p.EmployeeRemark).Readonly(false);
                View.Property(p => p.Employee.EmployeeStatus).Readonly(false).UseEditor(WPFEditorNames.Enum).HasLabel("员工状态");
                View.Property(p => p.Employee.Sex).Readonly(false).UseEditor(WPFEditorNames.Enum).HasLabel("性别");
                View.Property(EmployeeViewModel.CreateAccountProperty).HasLabel("同时创建账号").Readonly(false);
                View.Property(EmployeeViewModel.UserCodeProperty).HasLabel("用户编码").Visibility(EmployeeViewModel.CreateAccountProperty);
                View.Property(EmployeeViewModel.AuthenticateModeProperty).HasLabel("认证方式").Visibility(EmployeeViewModel.CreateAccountProperty).UseDropDownEditor(() => AuthenticateManager.Current.EnumToDictionary());
            }
        }
    }
}