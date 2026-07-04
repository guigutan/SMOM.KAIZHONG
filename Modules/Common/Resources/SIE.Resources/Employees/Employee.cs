using SIE.Common.Users;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 员工
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("员工")]
    [DisplayMember(nameof(Name))]
    public partial class Employee : DataEntity
    {
        #region 工号 Code
        /// <summary>
        /// 工号
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("工号")]
        public static readonly Property<string> CodeProperty = P<Employee>.Register(e => e.Code);

        /// <summary>
        /// 工号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 员工组 EmployeeGroup
        /// <summary>
        /// 员工组Id
        /// </summary>
        [Label("员工组")]
        public static readonly IRefIdProperty EmployeeGroupIdProperty = P<Employee>.RegisterRefId(e => e.EmployeeGroupId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EmployeeGroup> EmployeeGroupProperty = P<Employee>.RegisterRef(e => e.EmployeeGroup, EmployeeGroupIdProperty);

        /// <summary>
        /// 员工组
        /// </summary>
        public EmployeeGroup EmployeeGroup
        {
            get { return GetRefEntity(EmployeeGroupProperty); }
            set { SetRefEntity(EmployeeGroupProperty, value); }
        }
        #endregion

        #region 姓名 Name
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("姓名")]
        public static readonly Property<string> NameProperty = P<Employee>.Register(e => e.Name);

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 入职时间 HireDate
        /// <summary>
        /// 入职时间
        /// </summary>
        [Label("入职时间")]
        public static readonly Property<DateTime?> HireDateProperty = P<Employee>.Register(e => e.HireDate);

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? HireDate
        {
            get { return GetProperty(HireDateProperty); }
            set { SetProperty(HireDateProperty, value); }
        }
        #endregion

        #region 电话号码 Phone
        /// <summary>
        /// 电话号码
        /// </summary>
        [Label("电话号码")]
        public static readonly Property<string> PhoneProperty = P<Employee>.Register(e => e.Phone);

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
        [MaxLength(80)]
        [Label("电子邮箱")]
        public static readonly Property<string> EmailProperty = P<Employee>.Register(e => e.Email);

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email
        {
            get { return GetProperty(EmailProperty); }
            set { SetProperty(EmailProperty, value); }
        }
        #endregion

        #region 图片 Photo
        /// <summary>
        /// 图片
        /// </summary>
        [Label("照片")]
        public static readonly Property<byte[]> PhotoProperty = P<Employee>.Register(e => e.Photo);

        /// <summary>
        /// 图片
        /// </summary>
        public byte[] Photo
        {
            get { return GetProperty(PhotoProperty); }
            set { SetProperty(PhotoProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        [MaxLength(400)]
        public static readonly Property<string> RemarkProperty = P<Employee>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 员工与资源 ResourceList
        /// <summary>
        /// 员工与资源 列表
        /// </summary>
        [Label("资源列表")]
        public static readonly ListProperty<EntityList<EmployeeResource>> ResourceListProperty = P<Employee>.RegisterList(e => e.ResourceList);

        /// <summary>
        /// 员工与资源 列表
        /// </summary>
        public EntityList<EmployeeResource> ResourceList
        {
            get { return this.GetLazyList(ResourceListProperty); }
        }
        #endregion

        #region 员工与工厂关系 EnterpriseList
        /// <summary>
        /// 员工与工厂 列表
        /// </summary>
        [Label("工厂列表")]
        public static readonly ListProperty<EntityList<EmployeeEnterprise>> EnterpriseListProperty = P<Employee>.RegisterList(e => e.EnterpriseList);
        /// <summary>
        /// 员工与工厂 列表
        /// </summary>
        public EntityList<EmployeeEnterprise> EnterpriseList
        {
            get { return this.GetLazyList(EnterpriseListProperty); }
        }
        #endregion

        #region 性别 Sex
        /// <summary>
        /// 性别
        /// </summary>
        [Label("性别")]
        public static readonly Property<Sex> SexProperty = P<Employee>.Register(e => e.Sex);

        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex
        {
            get { return GetProperty(SexProperty); }
            set { SetProperty(SexProperty, value); }
        }
        #endregion

        #region 员工状态 EmployeeStatus
        /// <summary>
        /// 员工状态
        /// </summary>
        [Label("员工状态")]
        public static readonly Property<EmployeeStatus> EmployeeStatusProperty = P<Employee>.Register(e => e.EmployeeStatus);

        /// <summary>
        /// 员工状态
        /// </summary>
        public EmployeeStatus EmployeeStatus
        {
            get { return GetProperty(EmployeeStatusProperty); }
            set { SetProperty(EmployeeStatusProperty, value); }
        }
        #endregion

        #region 员工类型 EmployeeType
        /// <summary>
        /// 员工类型
        /// </summary>
        [Label("员工类型")]
        public static readonly Property<EmployeeType?> EmployeeTypeProperty = P<Employee>.Register(e => e.EmployeeType);

        /// <summary>
        /// 员工类型
        /// </summary>
        public EmployeeType? EmployeeType
        {
            get { return GetProperty(EmployeeTypeProperty); }
            set { SetProperty(EmployeeTypeProperty, value); }
        }
        #endregion

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组")]
        public static readonly IRefIdProperty WorkGroupIdProperty = P<Employee>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkGroup> WorkGroupProperty = P<Employee>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public WorkGroup WorkGroup
        {
            get { return GetRefEntity(WorkGroupProperty); }
            set { SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion

        #region 用户 User
        /// <summary>
        /// 用户Id
        /// </summary>
        [Label("用户")]
        public static readonly IRefIdProperty UserIdProperty =
            P<Employee>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

        /// <summary>
        /// 用户Id
        /// </summary>
        public double? UserId
        {
            get { return (double?)this.GetRefNullableId(UserIdProperty); }
            set { this.SetRefNullableId(UserIdProperty, value); }
        }

        /// <summary>
        /// 用户
        /// </summary>
        public static readonly RefEntityProperty<User> UserProperty =
            P<Employee>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 用户
        /// </summary>
        public User User
        {
            get { return this.GetRefEntity(UserProperty); }
            set { this.SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region 员工组 EmployeeGroupName
        /// <summary>
        /// 员工组
        /// </summary>
        [Label("员工组")]
        public static readonly Property<string> EmployeeGroupNameProperty = P<Employee>.RegisterView(e => e.EmployeeGroupName, p => p.EmployeeGroup.Name);

        /// <summary>
        /// 员工组
        /// </summary>
        public string EmployeeGroupName
        {
            get { return this.GetProperty(EmployeeGroupNameProperty); }
            set { SetProperty(EmployeeGroupNameProperty, value); }
        }
        #endregion

        #region 用户 UserCode
        /// <summary>
        /// 用户
        /// </summary>
        [Label("用户")]
        public static readonly Property<string> UserCodeProperty = P<Employee>.RegisterView(e => e.UserCode, p => p.User.Code);

        /// <summary>
        /// 用户
        /// </summary>
        public string UserCode
        {
            get { return this.GetProperty(UserCodeProperty); }
            set { SetProperty(UserCodeProperty, value); }
        }
        #endregion

        #region 飞书Id FeiId
        /// <summary>
        /// 飞书Id
        /// </summary>
        [Label("飞书Id")]
        public static readonly Property<string> FeiIdProperty = P<Employee>.Register(e => e.FeiId);

        /// <summary>
        /// 飞书Id
        /// </summary>
        public string FeiId
        {
            get { return this.GetProperty(FeiIdProperty); }
            set { this.SetProperty(FeiIdProperty, value); }
        }
        #endregion

        #region 一级组织 OrgLevel1
        /// <summary>
        /// 一级组织
        /// </summary>
        [Label("一级组织")]
        public static readonly Property<string> OrgLevel1Property = P<Employee>.Register(e => e.OrgLevel1);

        /// <summary>
        /// 一级组织
        /// </summary>
        public string OrgLevel1
        {
            get { return this.GetProperty(OrgLevel1Property); }
            set { this.SetProperty(OrgLevel1Property, value); }
        }
        #endregion

        #region 二级组织 OrgLevel2
        /// <summary>
        /// 二级组织
        /// </summary>
        [Label("二级组织")]
        public static readonly Property<string> OrgLevel2Property = P<Employee>.Register(e => e.OrgLevel2);

        /// <summary>
        /// 二级组织
        /// </summary>
        public string OrgLevel2
        {
            get { return this.GetProperty(OrgLevel2Property); }
            set { this.SetProperty(OrgLevel2Property, value); }
        }
        #endregion

        #region 三级组织 OrgLevel3
        /// <summary>
        /// 三级组织
        /// </summary>
        [Label("三级组织")]
        public static readonly Property<string> OrgLevel3Property = P<Employee>.Register(e => e.OrgLevel3);

        /// <summary>
        /// 三级组织
        /// </summary>
        public string OrgLevel3
        {
            get { return this.GetProperty(OrgLevel3Property); }
            set { this.SetProperty(OrgLevel3Property, value); }
        }
        #endregion

        #region 四级组织 OrgLevel4
        /// <summary>
        /// 四级组织
        /// </summary>
        [Label("四级组织")]
        public static readonly Property<string> OrgLevel4Property = P<Employee>.Register(e => e.OrgLevel4);

        /// <summary>
        /// 四级组织
        /// </summary>
        public string OrgLevel4
        {
            get { return this.GetProperty(OrgLevel4Property); }
            set { this.SetProperty(OrgLevel4Property, value); }
        }
        #endregion
    }

    /// <summary>
    /// 员工 实体配置
    /// </summary>
    internal class EmployeeConfig : EntityConfig<Employee>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_EMP").MapAllProperties();
            Meta.Property(Employee.UserIdProperty).MapColumn().IgnoreFK();
            Meta.Property(Employee.SexProperty).MapColumn().IsNullable();//SIE.Resources.Employee没有此字段，数据库需要可空
            Meta.Property(Employee.RemarkProperty).MapColumn().HasLength(1200);
            Meta.DisableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}