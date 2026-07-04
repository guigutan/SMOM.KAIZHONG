using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.Workbench.EmployeeMarks
{
    /// <summary>
    /// 个人评分
    /// TODO huchuqiang demo数据
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("个人评分")]
    public partial class EmployeeMark : DataEntity
    {
        #region 当前计算时间 CalculateTime
        /// <summary>
        /// 当前计算时间
        /// </summary>
        [Label("当前计算时间")]
        public static readonly Property<DateTime?> CalculateTimeProperty = P<EmployeeMark>.Register(e => e.CalculateTime);

        /// <summary>
        /// 当前计算时间
        /// </summary>
        public DateTime? CalculateTime
        {
            get { return GetProperty(CalculateTimeProperty); }
            set { SetProperty(CalculateTimeProperty, value); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal?> PlanQtyProperty = P<EmployeeMark>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal? PlanQty
        {
            get { return GetProperty(PlanQtyProperty); }
            set { SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 完工数量 FinishedQty
        /// <summary>
        /// 完工数量
        /// </summary>
        [Label("完工数量")]
        public static readonly Property<decimal?> FinishedQtyProperty = P<EmployeeMark>.Register(e => e.FinishedQty);

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal? FinishedQty
        {
            get { return GetProperty(FinishedQtyProperty); }
            set { SetProperty(FinishedQtyProperty, value); }
        }
        #endregion

        #region 分数 Mark
        /// <summary>
        /// 分数
        /// </summary>
        [Label("分数")]
        public static readonly Property<decimal> MarkProperty = P<EmployeeMark>.Register(e => e.Mark);

        /// <summary>
        /// 分数
        /// </summary>
        public decimal Mark
        {
            get { return GetProperty(MarkProperty); }
            set { SetProperty(MarkProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<EmployeeMark>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<EmployeeMark>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty = P<EmployeeMark>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double? ShiftId
        {
            get { return (double?)GetRefNullableId(ShiftIdProperty); }
            set { SetRefNullableId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<EmployeeMark>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 资源
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<EmployeeMark>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<EmployeeMark>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 个人评分 实体配置
    /// </summary>
    internal class EmployeeMarkConfig : EntityConfig<EmployeeMark>
    {
        /// <summary>
        /// 个人作业评分实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("EMPLOYEE_MARK").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}