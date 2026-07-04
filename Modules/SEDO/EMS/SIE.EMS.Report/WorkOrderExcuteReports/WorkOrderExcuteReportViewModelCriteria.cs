using SIE.Domain;
using SIE.EMS.EquipRepairs.Enums;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Report.WorkOrderExcuteReports
{
    /// <summary>
    /// 工单执行统计报表
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工单执行统计报表查询实体")]
    public class WorkOrderExcuteReportViewModelCriteria : Criteria
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<WorkOrderExcuteReportViewModelCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<WorkOrderExcuteReportViewModelCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<WorkOrderExcuteReportViewModelCriteria>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)GetRefNullableId(DepartmentIdProperty); }
            set { SetRefNullableId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<WorkOrderExcuteReportViewModelCriteria>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 工单类型 RepairType
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<EquipRepairType?> RepairTypeProperty = P<WorkOrderExcuteReportViewModelCriteria>.Register(e => e.RepairType);

        /// <summary>
        /// 工单类型
        /// </summary>
        public EquipRepairType? RepairType
        {
            get { return this.GetProperty(RepairTypeProperty); }
            set { this.SetProperty(RepairTypeProperty, value); }
        }
        #endregion

        #region 月份 Month
        /// <summary>
        /// 月份
        /// </summary>
        [Label("月份从")]
        public static readonly Property<DateTime?> BeginMonthProperty = P<WorkOrderExcuteReportViewModelCriteria>.Register(e => e.BeginMonth);

        /// <summary>
        /// 月份
        /// </summary>
        public DateTime? BeginMonth
        {
            get { return GetProperty(BeginMonthProperty); }
            set { SetProperty(BeginMonthProperty, value); }
        }
        #endregion

        #region 月份 Month
        /// <summary>
        /// 月份
        /// </summary>
        [Label("到")]
        public static readonly Property<DateTime?> EndMonthProperty = P<WorkOrderExcuteReportViewModelCriteria>.Register(e => e.EndMonth);

        /// <summary>
        /// 月份
        /// </summary>
        public DateTime? EndMonth
        {
            get { return GetProperty(EndMonthProperty); }
            set { SetProperty(EndMonthProperty, value); }
        }
        #endregion
    }
}
