using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Rbac.Users;
using System;
using SIE.Common.Configs;
using SIE.EMS.DevicePurs.Configs;

namespace SIE.EMS.DevicePurs
{
    /// <summary>
    /// 设备与人员权限维护
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DevicePurCriteria))]
    [Label("设备与人员权限维护")]
    [DisplayMember(nameof(EmployeeNames))]
    [EntityWithConfig(typeof(EnableDevicePermissionsConfig))]
    public partial class DevicePur : DataEntity
    {
        #region 设备维修 EquipMaintain
        /// <summary>
        /// 设备维修
        /// </summary>
        [Label("设备维修")]
        public static readonly Property<bool> EquipMaintainProperty = P<DevicePur>.Register(e => e.EquipMaintain);

        /// <summary>
        /// 设备维修
        /// </summary>
        public bool EquipMaintain
        {
            get { return GetProperty(EquipMaintainProperty); }
            set { SetProperty(EquipMaintainProperty, value); }
        }
        #endregion

        #region 点检确认人 CheckConfirm
        /// <summary>
        /// 点检确认人
        /// </summary>
        [Label("点检确认人")]
        public static readonly Property<bool> CheckConfirmProperty = P<DevicePur>.Register(e => e.CheckConfirm);

        /// <summary>
        /// 点检确认人
        /// </summary>
        public bool CheckConfirm
        {
            get { return GetProperty(CheckConfirmProperty); }
            set { SetProperty(CheckConfirmProperty, value); }
        }
        #endregion

        #region 保养确认人 MaintainConfirm
        /// <summary>
        /// 保养确认人
        /// </summary>
        [Label("保养确认人")]
        public static readonly Property<bool> MaintainConfirmProperty = P<DevicePur>.Register(e => e.MaintainConfirm);

        /// <summary>
        /// 保养确认人
        /// </summary>
        public bool MaintainConfirm
        {
            get { return GetProperty(MaintainConfirmProperty); }
            set { SetProperty(MaintainConfirmProperty, value); }
        }
        #endregion

        #region 维修确认人 RepairConfirm
        /// <summary>
        /// 维修确认人
        /// </summary>
        [Label("维修确认人")]
        public static readonly Property<bool> RepairConfirmProperty = P<DevicePur>.Register(e => e.RepairConfirm);

        /// <summary>
        /// 维修确认人
        /// </summary>
        public bool RepairConfirm
        {
            get { return GetProperty(RepairConfirmProperty); }
            set { SetProperty(RepairConfirmProperty, value); }
        }
        #endregion

        #region 设备与人员权限与用户的关系 User
        /// <summary>
        /// 设备与人员权限与用户的关系Id
        /// </summary>        
        public static readonly IRefIdProperty UserIdProperty
            = P<DevicePur>.RegisterRefId(e => e.UserId, ReferenceType.Normal);
        /// <summary>
        /// 设备与人员权限与用户的关系
        /// </summary>
        public double? UserId
        {
            get { return (double?)GetRefId(UserIdProperty); }
            set { SetRefId(UserIdProperty, value); }
        }

        /// <summary>
        /// 设备与人员权限与用户的关系
        /// </summary>
        public static readonly RefEntityProperty<User> UserProperty
            = P<DevicePur>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 设备与人员权限与用户的关系
        /// </summary>
        public User User
        {
            get { return GetRefEntity(UserProperty); }
            set { SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region 设备与人员权限与用户组的关系 UserGroup
        /// <summary>
        /// 设备与人员权限与用户组的关系Id
        /// </summary>        
        public static readonly IRefIdProperty UserGroupIdProperty
            = P<DevicePur>.RegisterRefId(e => e.UserGroupId, ReferenceType.Normal);

        /// <summary>
        /// 设备与人员权限与用户组的关系
        /// </summary>
        public double? UserGroupId
        {
            get { return (double?)GetRefId(UserGroupIdProperty); }
            set { SetRefId(UserGroupIdProperty, value); }
        }

        /// <summary>
        /// 设备与人员权限与用户组的关系
        /// </summary>
        public static readonly RefEntityProperty<UserGroup> UserGroupProperty
            = P<DevicePur>.RegisterRef(e => e.UserGroup, UserGroupIdProperty);

        /// <summary>
        /// 设备与人员权限与用户组的关系
        /// </summary>
        public UserGroup UserGroup
        {
            get { return GetRefEntity(UserGroupProperty); }
            set { SetRefEntity(UserGroupProperty, value); }
        }
        #endregion

        #region 设备清单列表 DeviceBillList
        /// <summary>
        /// 设备清单列表
        /// </summary>
        [Label("设备清单")]
        public static readonly ListProperty<EntityList<DeviceBill>> DeviceBillListProperty = P<DevicePur>.RegisterList(e => e.DeviceBillList);

        /// <summary>
        /// 设备清单列表
        /// </summary>
        public EntityList<DeviceBill> DeviceBillList
        {
            get { return this.GetLazyList(DeviceBillListProperty); }
        }
        #endregion

        #region 责任部门列表 DeviceDepaList
        /// <summary>
        /// 责任部门列表
        /// </summary>
        [Label("责任部门列表")]
        public static readonly ListProperty<EntityList<DeviceDepa>> DeviceDepaListProperty
            = P<DevicePur>.RegisterList(e => e.DeviceDepaList);

        /// <summary>
        /// 责任部门列表
        /// </summary>
        public EntityList<DeviceDepa> DeviceDepaList
        {
            get { return this.GetLazyList(DeviceDepaListProperty); }
        }
        #endregion

        #region 业务部门列表 DeviceUseDepaList
        /// <summary>
        /// 业务部门列表
        /// </summary>
        [Label("业务部门列表")]
        public static readonly ListProperty<EntityList<DeviceUseDepartment>> DeviceUseDepaListProperty
            = P<DevicePur>.RegisterList(e => e.DeviceUseDepaList);

        /// <summary>
        /// 业务部门列表
        /// </summary>
        public EntityList<DeviceUseDepartment> DeviceUseDepaList
        {
            get { return this.GetLazyList(DeviceUseDepaListProperty); }
        }
        #endregion

        #region 预算部门列表 DeviceBudgetDepartmentList
        /// <summary>
        /// 预算部门列表
        /// </summary>
        [Label("预算部门列表")]
        public static readonly ListProperty<EntityList<DeviceBudgetDepartment>> DeviceBudgetDepartmentListProperty
            = P<DevicePur>.RegisterList(e => e.DeviceBudgetDepartmentList);

        /// <summary>
        /// 预算部门列表
        /// </summary>
        public EntityList<DeviceBudgetDepartment> DeviceBudgetDepartmentList
        {
            get { return this.GetLazyList(DeviceBudgetDepartmentListProperty); }
        }
        #endregion

        #region 采购对象权限列表 DevicePurchaseList
        /// <summary>
        /// 采购对象权限列表
        /// </summary>
        [Label("采购对象权限列表")]
        public static readonly ListProperty<EntityList<DevicePurchaseObjectType>> DevicePurchaseListProperty = P<DevicePur>.RegisterList(e => e.DevicePurchaseList);

        /// <summary>
        /// 采购对象权限列表
        /// </summary>
        public EntityList<DevicePurchaseObjectType> DevicePurchaseList
        {
            get { return this.GetLazyList(DevicePurchaseListProperty); }
        }
        #endregion

        #region 用户名 EmployeeNames
        /// <summary>
        /// 用户名
        /// </summary>
        [Label("用户名")]
        public static readonly Property<string> EmployeeNamesProperty = P<DevicePur>.Register(e => e.EmployeeNames);

        /// <summary>
        /// 用户名
        /// </summary>
        public string EmployeeNames
        {
            get { return this.GetProperty(EmployeeNamesProperty); }
            set { this.SetProperty(EmployeeNamesProperty, value); }
        }
        #endregion

        #region 非映射字段

        #region 查询关键字 SearchKeywordDontMap
        /// <summary>
        /// 查询关键字
        /// </summary>
        [Label("查询关键字")]
        public static readonly Property<string> SearchKeywordDontMapProperty = P<DevicePur>.Register(e => e.SearchKeywordDontMap);

        /// <summary>
        /// 查询关键字
        /// </summary>
        public string SearchKeywordDontMap
        {
            get { return this.GetProperty(SearchKeywordDontMapProperty); }
            set { this.SetProperty(SearchKeywordDontMapProperty, value); }
        }
        #endregion

        #endregion

        #region 视图属性

        #region 用户编码 EmployeeCode
        /// <summary>
        /// 用户编码
        /// </summary>
        [Label("用户编码")]
        public static readonly Property<string> EmployeeCodeProperty = P<DevicePur>.RegisterView(e => e.EmployeeCode, p => p.User.Code);

        /// <summary>
        /// 用户编码
        /// </summary>
        public string EmployeeCode
        {
            get { return GetProperty(EmployeeCodeProperty); }
            set { SetProperty(EmployeeCodeProperty, value); }
        }
        #endregion

        #region 用户名称 EmployeeName
        /// <summary>
        /// 用户名称
        /// </summary>
        [Label("用户名称")]
        public static readonly Property<string> EmployeeNameProperty = P<DevicePur>.RegisterView(e => e.EmployeeName, p => p.User.Employee.Name);

        /// <summary>
        /// 用户名称
        /// </summary>
        public string EmployeeName
        {
            get { return GetProperty(EmployeeNameProperty); }
            set { SetProperty(EmployeeNameProperty, value); }
        }
        #endregion

        #region 用户组编码 UserGroupCode
        /// <summary>
        /// 用户组编码
        /// </summary>
        [Label("用户组编码")]
        public static readonly Property<string> UserGroupCodeProperty = P<DevicePur>.RegisterView(e => e.UserGroupCode, p => p.UserGroup.Code);

        /// <summary>
        /// 用户组编码
        /// </summary>
        public string UserGroupCode
        {
            get { return GetProperty(UserGroupCodeProperty); }
            set { SetProperty(UserGroupCodeProperty, value); }
        }
        #endregion

        #region 用户组名称 UserGroupName
        /// <summary>
        /// 用户组名称
        /// </summary>
        [Label("用户组名称")]
        public static readonly Property<string> UserGroupNameProperty = P<DevicePur>.RegisterView(e => e.UserGroupName, p => p.UserGroup.Name);

        /// <summary>
        /// 用户组名称
        /// </summary>
        public string UserGroupName
        {
            get { return GetProperty(UserGroupNameProperty); }
            set { SetProperty(UserGroupNameProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 设备与人员权限 实体配置
    /// </summary>
    internal class DevicePurConfig : EntityConfig<DevicePur>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_DEVICE_PUR").MapAllProperties();
            Meta.Property(DevicePur.SearchKeywordDontMapProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}