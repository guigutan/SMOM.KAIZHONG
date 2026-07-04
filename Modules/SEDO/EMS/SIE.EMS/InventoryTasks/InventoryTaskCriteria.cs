using SIE.Domain;
using SIE.EMS.Enums;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("盘点任务查询")]
    public partial class InventoryTaskCriteria : Criteria
    {
        #region 任务单号 TaskNo
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> TaskNoProperty = P<InventoryTaskCriteria>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
            set { this.SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 计划单号 PlanNo
        /// <summary>
        /// 计划单号
        /// </summary>
        [Label("计划单号")]
        public static readonly Property<string> PlanNoProperty = P<InventoryTaskCriteria>.Register(e => e.PlanNo);

        /// <summary>
        /// 计划单号
        /// </summary>
        public string PlanNo
        {
            get { return GetProperty(PlanNoProperty); }
            set { SetProperty(PlanNoProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<InventoryTaskCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)GetRefNullableId(FactoryIdProperty); }
            set { SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<InventoryTaskCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 资产对象 InventoryAssetObject
        /// <summary>
        /// 资产对象
        /// </summary>
        [Label("资产对象")]
        [Required]
        public static readonly Property<InventoryAssetObject?> InventoryAssetObjectProperty = P<InventoryTaskCriteria>.Register(e => e.InventoryAssetObject);

        /// <summary>
        /// 资产对象
        /// </summary>
        public InventoryAssetObject? InventoryAssetObject
        {
            get { return GetProperty(InventoryAssetObjectProperty); }
            set { SetProperty(InventoryAssetObjectProperty, value); }
        }
        #endregion

        #region 盘点类型 InventoryType
        /// <summary>
        /// 盘点类型
        /// </summary>
        [Label("盘点类型")]
        public static readonly Property<string> InventoryTypeProperty = P<InventoryTaskCriteria>.Register(e => e.InventoryType);

        /// <summary>
        /// 盘点类型
        /// </summary>
        public string InventoryType
        {
            get { return this.GetProperty(InventoryTypeProperty); }
            set { this.SetProperty(InventoryTypeProperty, value); }
        }
        #endregion

        #region 盘点责任人 Responsible
        /// <summary>
        /// 盘点责任人Id
        /// </summary>
        [Label("盘点责任人")]
        public static readonly IRefIdProperty ResponsibleIdProperty = P<InventoryTaskCriteria>.RegisterRefId(e => e.ResponsibleId, ReferenceType.Normal);

        /// <summary>
        /// 盘点责任人Id
        /// </summary>
        public double? ResponsibleId
        {
            get { return (double?)GetRefNullableId(ResponsibleIdProperty); }
            set { SetRefNullableId(ResponsibleIdProperty, value); }
        }

        /// <summary>
        /// 盘点责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ResponsibleProperty = P<InventoryTaskCriteria>.RegisterRef(e => e.Responsible, ResponsibleIdProperty);

        /// <summary>
        /// 盘点责任人
        /// </summary>
        public Employee Responsible
        {
            get { return GetRefEntity(ResponsibleProperty); }
            set { SetRefEntity(ResponsibleProperty, value); }
        }
        #endregion

        #region 盘点状态 InventoryTaskStatus
        /// <summary>
        /// 盘点状态
        /// </summary>
        [Label("盘点状态")]
        public static readonly Property<InventoryTaskStatus?> InventoryTaskStatusProperty = P<InventoryTaskCriteria>.Register(e => e.InventoryTaskStatus);

        /// <summary>
        /// 盘点状态
        /// </summary>
        public InventoryTaskStatus? InventoryTaskStatus
        {
            get { return GetProperty(InventoryTaskStatusProperty); }
            set { SetProperty(InventoryTaskStatusProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<InventoryTaskCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<InventoryTaskController>().CriteriaInventoryTasks(this);
        }
    }
}
