using SIE.Domain;
using SIE.EMS.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.InventoryPlans
{
    /// <summary>
    /// 盘点人
    /// </summary>
    [ChildEntity, Serializable]
    [Label("盘点人")]
    public partial class InventoryCounter : DataEntity
    {
        #region 盘点计划 InventoryPlan
        /// <summary>
        /// 盘点计划Id
        /// </summary>
        [Label("盘点计划")]
        public static readonly IRefIdProperty InventoryPlanIdProperty = P<InventoryCounter>.RegisterRefId(e => e.InventoryPlanId, ReferenceType.Parent);

        /// <summary>
        /// 盘点计划Id
        /// </summary>
        public double InventoryPlanId
        {
            get { return (double)GetRefId(InventoryPlanIdProperty); }
            set { SetRefId(InventoryPlanIdProperty, value); }
        }

        /// <summary>
        /// 盘点计划
        /// </summary>
        public static readonly RefEntityProperty<InventoryPlan> InventoryPlanProperty = P<InventoryCounter>.RegisterRef(e => e.InventoryPlan, InventoryPlanIdProperty);

        /// <summary>
        /// 盘点计划
        /// </summary>
        public InventoryPlan InventoryPlan
        {
            get { return GetRefEntity(InventoryPlanProperty); }
            set { SetRefEntity(InventoryPlanProperty, value); }
        }
        #endregion

        #region 初盘 First
        /// <summary>
        /// 初盘
        /// </summary>
        [Label("初盘")]
        public static readonly Property<bool> FirstProperty = P<InventoryCounter>.Register(e => e.First);

        /// <summary>
        /// 初盘
        /// </summary>
        public bool First
        {
            get { return GetProperty(FirstProperty); }
            set { SetProperty(FirstProperty, value); }
        }
        #endregion

        #region 复盘 Second
        /// <summary>
        /// 复盘
        /// </summary>
        [Label("复盘")]
        public static readonly Property<bool> SecondProperty = P<InventoryCounter>.Register(e => e.Second);

        /// <summary>
        /// 复盘
        /// </summary>
        public bool Second
        {
            get { return GetProperty(SecondProperty); }
            set { SetProperty(SecondProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<InventoryCounter>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<InventoryCounter>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 盘点范围 InventoryScope
        /// <summary>
        /// 盘点范围
        /// </summary>
        [Label("盘点范围")]
        public static readonly Property<InventoryScope> InventoryScopeProperty = P<InventoryCounter>.Register(e => e.InventoryScope);

        /// <summary>
        /// 盘点范围
        /// </summary>
        public InventoryScope InventoryScope
        {
            get { return GetProperty(InventoryScopeProperty); }
            set { SetProperty(InventoryScopeProperty, value); }
        }
        #endregion


        #region 只读 IsReadOnly
        /// <summary>
        /// 只读
        /// </summary>
        [Label("只读")]
        public static readonly Property<bool> IsReadOnlyProperty = P<InventoryCounter>.Register(e => e.IsReadOnly);

        /// <summary>
        /// 只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return this.GetProperty(IsReadOnlyProperty); }
            set { this.SetProperty(IsReadOnlyProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工号 EmployeeCode
        /// <summary>
        /// 工号
        /// </summary>
        [Label("工号")]
        public static readonly Property<string> EmployeeCodeProperty = P<InventoryCounter>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 工号
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
        }
        #endregion

        #region 姓名 Name
        /// <summary>
        /// 姓名
        /// </summary>
        [Label("姓名")]
        public static readonly Property<string> NameProperty = P<InventoryCounter>.RegisterView(e => e.Name, p => p.Employee.Name);

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 盘点人 实体配置
    /// </summary>
    internal class InventoryCounterConfig : EntityConfig<InventoryCounter>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INV_CNT").MapAllPropertiesExcept(InventoryCounter.IsReadOnlyProperty);
            Meta.EnablePhantoms();
        }
    }
}