using SIE.Common.InvOrg;
using SIE.Domain;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepairs.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.Report.FaultCountReports
{
    /// <summary>
    /// 故障统计报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(FaultCountReportCriteria))]
    [Label("故障统计报表")]
    public class FaultCountReport : Entity<Double>
    {
        #region 工单编码 RepairNo
        /// <summary>
        /// 工单编码
        /// </summary>
        [Label("工单编码")]
        public static readonly Property<string> RepairNoProperty = P<FaultCountReport>.Register(e => e.RepairNo);

        /// <summary>
        /// 工单编码
        /// </summary>
        public string RepairNo
        {
            get { return this.GetProperty(RepairNoProperty); }
            set { this.SetProperty(RepairNoProperty, value); }
        }
        #endregion

        #region 故障现象(备注) DeviceAbnormalRemark
        /// <summary>
        /// 故障现象(备注)
        /// </summary>
        [Label("故障现象(备注)")]
        public static readonly Property<string> DeviceAbnormalRemarkProperty = P<FaultCountReport>.Register(e => e.DeviceAbnormalRemark);

        /// <summary>
        /// 故障现象(备注)
        /// </summary>
        public string DeviceAbnormalRemark
        {
            get { return this.GetProperty(DeviceAbnormalRemarkProperty); }
            set { this.SetProperty(DeviceAbnormalRemarkProperty, value); }
        }
        #endregion

        #region 工单状态 RepairState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<EquipRepairState> RepairStateProperty = P<FaultCountReport>.Register(e => e.RepairState);

        /// <summary>
        /// 工单状态
        /// </summary>
        public EquipRepairState RepairState
        {
            get { return this.GetProperty(RepairStateProperty); }
            set { this.SetProperty(RepairStateProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<FaultCountReport>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<FaultCountReport>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 报修人 ApplyRepairEmployee
        /// <summary>
        /// 报修人Id
        /// </summary>
        [Label("报修人")]
        public static readonly IRefIdProperty ApplyRepairEmployeeIdProperty =
            P<FaultCountReport>.RegisterRefId(e => e.ApplyRepairEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 报修人Id
        /// </summary>
        public double ApplyRepairEmployeeId
        {
            get { return (double)this.GetRefId(ApplyRepairEmployeeIdProperty); }
            set { this.SetRefId(ApplyRepairEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 报修人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ApplyRepairEmployeeProperty =
            P<FaultCountReport>.RegisterRef(e => e.ApplyRepairEmployee, ApplyRepairEmployeeIdProperty);

        /// <summary>
        /// 报修人
        /// </summary>
        public Employee ApplyRepairEmployee
        {
            get { return this.GetRefEntity(ApplyRepairEmployeeProperty); }
            set { this.SetRefEntity(ApplyRepairEmployeeProperty, value); }
        }
        #endregion

        #region 报修时间 ApplyRepairDate
        /// <summary>
        /// 报修时间
        /// </summary>
        [Label("报修时间")]
        public static readonly Property<DateTime> ApplyRepairDateProperty = P<FaultCountReport>.Register(e => e.ApplyRepairDate);

        /// <summary>
        /// 报修时间
        /// </summary>
        public DateTime ApplyRepairDate
        {
            get { return this.GetProperty(ApplyRepairDateProperty); }
            set { this.SetProperty(ApplyRepairDateProperty, value); }
        }
        #endregion

        #region 维修责任人 RepairMaster
        /// <summary>
        /// 维修责任人Id
        /// </summary>
        [Label("维修责任人")]
        public static readonly IRefIdProperty RepairMasterIdProperty =
            P<FaultCountReport>.RegisterRefId(e => e.RepairMasterId, ReferenceType.Normal);

        /// <summary>
        /// 维修责任人Id
        /// </summary>
        public double? RepairMasterId
        {
            get { return (double?)this.GetRefId(RepairMasterIdProperty); }
            set { this.SetRefId(RepairMasterIdProperty, value); }
        }

        /// <summary>
        /// 维修责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> RepairMasterProperty =
            P<FaultCountReport>.RegisterRef(e => e.RepairMaster, RepairMasterIdProperty);

        /// <summary>
        /// 维修责任人
        /// </summary>
        public Employee RepairMaster
        {
            get { return this.GetRefEntity(RepairMasterProperty); }
            set { this.SetRefEntity(RepairMasterProperty, value); }
        }
        #endregion

        #region 维修响应时间 ReceiveOrderDate
        /// <summary>
        /// 维修响应时间
        /// </summary>
        [Label("维修响应时间")]
        public static readonly Property<DateTime?> ReceiveOrderDateProperty = P<FaultCountReport>.Register(e => e.ReceiveOrderDate);

        /// <summary>
        /// 维修响应时间
        /// </summary>
        public DateTime? ReceiveOrderDate
        {
            get { return this.GetProperty(ReceiveOrderDateProperty); }
            set { this.SetProperty(ReceiveOrderDateProperty, value); }
        }
        #endregion

        #region 维修开始时间 RepairBeginDate
        /// <summary>
        /// 维修开始时间
        /// </summary>
        [Label("维修开始时间")]
        public static readonly Property<DateTime?> RepairBeginDateProperty = P<FaultCountReport>.Register(e => e.RepairBeginDate);

        /// <summary>
        /// 维修开始时间
        /// </summary>
        public DateTime? RepairBeginDate
        {
            get { return this.GetProperty(RepairBeginDateProperty); }
            set { this.SetProperty(RepairBeginDateProperty, value); }
        }
        #endregion

        #region 维修完成时间 RepairFinishDate
        /// <summary>
        /// 维修完成时间
        /// </summary>
        [Label("维修完成时间")]
        public static readonly Property<DateTime?> RepairFinishDateProperty = P<FaultCountReport>.Register(e => e.RepairFinishDate);

        /// <summary>
        /// 维修完成时间
        /// </summary>
        public DateTime? RepairFinishDate
        {
            get { return this.GetProperty(RepairFinishDateProperty); }
            set { this.SetProperty(RepairFinishDateProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<FaultCountReport>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<FaultCountReport>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 设备型号编码 EquipModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> EquipModelCodeProperty = P<FaultCountReport>.RegisterView(e => e.EquipModelCode, p => p.EquipAccount.EquipModel.Code);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
        }
        #endregion

        #region 设备型号 EquipAccountMode
        /// <summary>
        /// 
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipAccountModeProperty = P<FaultCountReport>.RegisterView(e => e.EquipAccountMode, p => p.EquipAccount.EquipModel.Name);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipAccountMode
        {
            get { return this.GetProperty(EquipAccountModeProperty); }
        }
        #endregion

        #region 设备型号 EquipModelId
        /// <summary>
        /// 
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<double> EquipModelIdProperty = P<FaultCountReport>.RegisterView(e => e.EquipModelId, p => p.EquipAccount.EquipModelId);

        /// <summary>
        /// 设备型号
        /// </summary>
        public double EquipModelId
        {
            get { return this.GetProperty(EquipModelIdProperty); }
        }
        #endregion

        #region 设备类型 EquipAccountType
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipAccountTypeProperty = P<FaultCountReport>.RegisterView(e => e.EquipAccountType, p => p.EquipAccount.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipAccountType
        {
            get { return this.GetProperty(EquipAccountTypeProperty); }
        }
        #endregion

        #region 使用部门 UseDepartment
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("使用部门")]
        public static readonly Property<string> UseDepartmentProperty = P<FaultCountReport>.RegisterView(e => e.UseDepartment, p => p.EquipAccount.UseDepartment.Name);

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDepartment
        {
            get { return this.GetProperty(UseDepartmentProperty); }
        }
        #endregion

        #region 安装位置 InstallationLocation
        /// <summary>
        /// 安装位置
        /// </summary>
        [Label("安装位置")]
        public static readonly Property<string> InstallationLocationProperty = P<FaultCountReport>.RegisterView(e => e.InstallationLocation, p => p.EquipAccount.InstallationLocation);

        /// <summary>
        /// 安装位置
        /// </summary>
        public string InstallationLocation
        {
            get { return this.GetProperty(InstallationLocationProperty); }
        }
        #endregion

        #region 报修人姓名 ApplyRepairEmployeeName
        /// <summary>
        /// 报修人姓名
        /// </summary>
        [Label("报修人姓名")]
        public static readonly Property<string> ApplyRepairEmployeeNameProperty = P<FaultCountReport>.RegisterView(e => e.ApplyRepairEmployeeName, p => p.ApplyRepairEmployee.Name);

        /// <summary>
        /// 报修人姓名
        /// </summary>
        public string ApplyRepairEmployeeName
        {
            get { return this.GetProperty(ApplyRepairEmployeeNameProperty); }
        }
        #endregion

        #region 工厂 FactoryName
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryNameProperty = P<FaultCountReport>.RegisterView(e => e.FactoryName, p => p.EquipAccount.Factory.Name);

        /// <summary>
        /// 工厂
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 库存组织 InvOrgId
        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<Int32> InvOrgIdProperty = P<FaultCountReport>.Register(e => e.InvOrgId);

        /// <summary>
        /// 库存组织
        /// </summary>
        public Int32 InvOrgId
        {
            get { return this.GetProperty(InvOrgIdProperty); }
        }
        #endregion

        #endregion


        /// <summary>
        /// 配置元数据
        /// </summary>
        internal class RiskOnhandListConfig : EntityConfig<FaultCountReport>
        {
            /// <summary>
            /// 配置元数据
            /// </summary>
            protected override void ConfigMeta()
            {
                var trans = RF.Find<EquipRepairBill>().EntityMeta;
                var repair_no = trans.Property(EquipRepairBill.RepairNoProperty).ColumnMeta.ColumnName;
                var device_abnormal_remark = trans.Property(EquipRepairBill.DeviceAbnormalRemarkProperty).ColumnMeta.ColumnName;
                var repair_state = trans.Property(EquipRepairBill.RepairStateProperty).ColumnMeta.ColumnName;
                var equip_account_id = trans.Property(EquipRepairBill.EquipAccountIdProperty).ColumnMeta.ColumnName;
                var apply_repair_employee_id = trans.Property(EquipRepairBill.ApplyRepairEmployeeIdProperty).ColumnMeta.ColumnName;
                var apply_repair_date = trans.Property(EquipRepairBill.ApplyRepairDateProperty).ColumnMeta.ColumnName;
                var repair_master_id = trans.Property(EquipRepairBill.RepairMasterIdProperty).ColumnMeta.ColumnName;
                var receive_order_date = trans.Property(EquipRepairBill.ReceiveOrderDateProperty).ColumnMeta.ColumnName;
                var repair_begin_date = trans.Property(EquipRepairBill.RepairBeginDateProperty).ColumnMeta.ColumnName;
                var repair_finish_date = trans.Property(EquipRepairBill.RepairFinishDateProperty).ColumnMeta.ColumnName;
                string invorgid = trans.Property(InvOrgIdExtension.INV_ORG_IDProperty).ColumnMeta.ColumnName;
                string isphantom = trans.Property(PhantomEntityExtension.IS_PHANTOMProperty).ColumnMeta.ColumnName;

                var view = @"(SELECT p.id,p.{1},p.{2},p.{3},p.{4},p.{5},p.{6},p.{7},p.{8} ,p.{9},p.{10},p.{11}
                                  FROM {0} p   WHERE {12} =0 )".FormatArgs(trans.TableMeta.TableName,
                                repair_no, device_abnormal_remark, repair_state, equip_account_id, apply_repair_employee_id, apply_repair_date, repair_master_id, receive_order_date, repair_begin_date, repair_finish_date, invorgid, isphantom);

                Meta.MapView(view).MapAllProperties();
            }
        }
    }
}
