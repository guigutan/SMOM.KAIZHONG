using SIE.Domain;
using SIE.Inventory.Task;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 任务分配规则查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("任务分配规则查询")]
    public partial class TaskAllotRuleCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<TaskAllotRuleCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<TaskAllotRuleCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 任务类型 OperationType
        /// <summary>
        /// 任务类型
        /// </summary>
        [Label("任务类型")]
        public static readonly Property<OperationType?> OperationTypeProperty = P<TaskAllotRuleCriteria>.Register(e => e.OperationType);

        /// <summary>
        /// 任务类型
        /// </summary>
        public OperationType? OperationType
        {
            get { return this.GetProperty(OperationTypeProperty); }
            set { this.SetProperty(OperationTypeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<TaskAllotRuleCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<TaskAllotRuleCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 逻辑分区 LogicArea
        /// <summary>
        /// 逻辑分区
        /// </summary>
        [Label("逻辑分区")]
        public static readonly Property<string> LogicAreaProperty = P<TaskAllotRuleCriteria>.Register(e => e.LogicArea);

        /// <summary>
        /// 逻辑分区
        /// </summary>
        public string LogicArea
        {
            get { return GetProperty(LogicAreaProperty); }
            set { SetProperty(LogicAreaProperty, value); }
        }
        #endregion

        #region 库存类别 ItemCategory
        /// <summary>
        /// 库存类别
        /// </summary>
        [Label("库存类别")]
        public static readonly Property<string> ItemCategoryProperty = P<TaskAllotRuleCriteria>.Register(e => e.ItemCategory);

        /// <summary>
        /// 库存类别
        /// </summary>
        public string ItemCategory
        {
            get { return GetProperty(ItemCategoryProperty); }
            set { SetProperty(ItemCategoryProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工
        /// </summary>
        [Label("员工")]
        public static readonly Property<string> EmployeeProperty = P<TaskAllotRuleCriteria>.Register(e => e.Employee);

        /// <summary>
        /// 员工
        /// </summary>
        public string Employee
        {
            get { return GetProperty(EmployeeProperty); }
            set { SetProperty(EmployeeProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<TaskAllotRuleCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        /// <returns>返回结果</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<RuleController>().GetTaskAllotRules(this);
        }
    }
}
