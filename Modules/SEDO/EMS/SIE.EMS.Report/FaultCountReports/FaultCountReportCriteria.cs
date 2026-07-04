using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Report.FaultCountReports
{
    /// <summary>
    /// 故障统计报表查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("故障统计报表查询")]
    public class FaultCountReportCriteria : Criteria
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<FaultCountReportCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<FaultCountReportCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<FaultCountReportCriteria>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<FaultCountReportCriteria>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 设备编码 Code
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> CodeProperty = P<FaultCountReportCriteria>.Register(e => e.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 维修责任人 RepairMaster
        /// <summary>
        /// 维修责任人Id
        /// </summary>
        [Label("维修责任人")]
        public static readonly IRefIdProperty RepairMasterIdProperty =
            P<FaultCountReportCriteria>.RegisterRefId(e => e.RepairMasterId, ReferenceType.Normal);

        /// <summary>
        /// 维修责任人Id
        /// </summary>
        public double? RepairMasterId
        {
            get { return (double?)this.GetRefNullableId(RepairMasterIdProperty); }
            set { this.SetRefNullableId(RepairMasterIdProperty, value); }
        }

        /// <summary>
        /// 维修责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> RepairMasterProperty =
            P<FaultCountReportCriteria>.RegisterRef(e => e.RepairMaster, RepairMasterIdProperty);

        /// <summary>
        /// 维修责任人
        /// </summary>
        public Employee RepairMaster
        {
            get { return this.GetRefEntity(RepairMasterProperty); }
            set { this.SetRefEntity(RepairMasterProperty, value); }
        }
        #endregion

        #region 报修时间 ApplyRepairDate
        /// <summary>
        /// 报修时间
        /// </summary>
        [Label("报修时间")]
        public static readonly Property<DateRange> ApplyRepairDateProperty = P<FaultCountReportCriteria>.Register(e => e.ApplyRepairDate);

        /// <summary>
        /// 报修时间
        /// </summary>
        public DateRange ApplyRepairDate
        {
            get { return this.GetProperty(ApplyRepairDateProperty); }
            set { this.SetProperty(ApplyRepairDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        /// <returns>返回结果</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FaultCountReportController>().GetFaultCountReports(this);
        }
    }
}
