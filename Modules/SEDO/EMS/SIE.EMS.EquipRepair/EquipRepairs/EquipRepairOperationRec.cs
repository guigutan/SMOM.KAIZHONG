using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备维修单操作记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("操作记录")]
    public partial class EquipRepairOperationRec : DataEntity
    {
        #region 设备维修单 EquipRepairBill
        /// <summary>
        /// 设备维修单Id
        /// </summary>
        [Label("设备维修单")]
        public static readonly IRefIdProperty EquipRepairBillIdProperty =
            P<EquipRepairOperationRec>.RegisterRefId(e => e.EquipRepairBillId, ReferenceType.Parent);

        /// <summary>
        /// 设备维修单Id
        /// </summary>
        public double EquipRepairBillId
        {
            get { return (double)this.GetRefId(EquipRepairBillIdProperty); }
            set { this.SetRefId(EquipRepairBillIdProperty, value); }
        }

        /// <summary>
        /// 设备维修单
        /// </summary>
        public static readonly RefEntityProperty<EquipRepairBill> EquipRepairBillProperty =
            P<EquipRepairOperationRec>.RegisterRef(e => e.EquipRepairBill, EquipRepairBillIdProperty);

        /// <summary>
        /// 设备维修单
        /// </summary>
        public EquipRepairBill EquipRepairBill
        {
            get { return this.GetRefEntity(EquipRepairBillProperty); }
            set { this.SetRefEntity(EquipRepairBillProperty, value); }
        }
        #endregion

        #region 操作类型 OperationType
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<RepairOperationType> OperationTypeProperty = P<EquipRepairOperationRec>.Register(e => e.OperationType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public RepairOperationType OperationType
        {
            get { return this.GetProperty(OperationTypeProperty); }
            set { this.SetProperty(OperationTypeProperty, value); }
        }
        #endregion

        #region 操作人 Operationer
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperationerIdProperty =
            P<EquipRepairOperationRec>.RegisterRefId(e => e.OperationerId, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperationerId
        {
            get { return (double)this.GetRefId(OperationerIdProperty); }
            set { this.SetRefId(OperationerIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperationerProperty =
            P<EquipRepairOperationRec>.RegisterRef(e => e.Operationer, OperationerIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Operationer
        {
            get { return this.GetRefEntity(OperationerProperty); }
            set { this.SetRefEntity(OperationerProperty, value); }
        }
        #endregion

        #region 操作时间 OperationDate
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateTime> OperationDateProperty = P<EquipRepairOperationRec>.Register(e => e.OperationDate);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationDate
        {
            get { return this.GetProperty(OperationDateProperty); }
            set { this.SetProperty(OperationDateProperty, value); }
        }
        #endregion

        #region 原维修责任人 OriginalRepairMaster
        /// <summary>
        /// 原维修责任人Id
        /// </summary>
        [Label("原维修责任人")]
        public static readonly IRefIdProperty OriginalRepairMasterIdProperty =
            P<EquipRepairOperationRec>.RegisterRefId(e => e.OriginalRepairMasterId, ReferenceType.Normal);

        /// <summary>
        /// 原维修责任人Id
        /// </summary>
        public double? OriginalRepairMasterId
        {
            get { return (double?)this.GetRefNullableId(OriginalRepairMasterIdProperty); }
            set { this.SetRefNullableId(OriginalRepairMasterIdProperty, value); }
        }

        /// <summary>
        /// 原维修责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OriginalRepairMasterProperty =
            P<EquipRepairOperationRec>.RegisterRef(e => e.OriginalRepairMaster, OriginalRepairMasterIdProperty);

        /// <summary>
        /// 原维修责任人
        /// </summary>
        public Employee OriginalRepairMaster
        {
            get { return this.GetRefEntity(OriginalRepairMasterProperty); }
            set { this.SetRefEntity(OriginalRepairMasterProperty, value); }
        }
        #endregion

        #region 原维修人员 OriginalRepairer
        /// <summary>
        /// 原维修人员
        /// </summary>
        [Label("原维修人员")]
        public static readonly Property<string> OriginalRepairerProperty = P<EquipRepairOperationRec>.Register(e => e.OriginalRepairer);

        /// <summary>
        /// 原维修人员
        /// </summary>
        public string OriginalRepairer
        {
            get { return this.GetProperty(OriginalRepairerProperty); }
            set { this.SetProperty(OriginalRepairerProperty, value); }
        }
        #endregion

        #region 交机确认结果 HandoverConfirmResult
        /// <summary>
        /// 交机确认结果
        /// </summary>
        [Label("交机确认结果")]
        public static readonly Property<HandoverConfirmResult?> HandoverConfirmResultProperty = P<EquipRepairOperationRec>.Register(e => e.HandoverConfirmResult);

        /// <summary>
        /// 交机确认结果
        /// </summary>
        public HandoverConfirmResult? HandoverConfirmResult
        {
            get { return this.GetProperty(HandoverConfirmResultProperty); }
            set { this.SetProperty(HandoverConfirmResultProperty, value); }
        }
        #endregion

        #region 工程确认结果 EngineerConfirmResult
        /// <summary>
        /// 工程确认结果
        /// </summary>
        [Label("工程确认结果")]
        public static readonly Property<EngineerConfirmResult?> EngineerConfirmResultProperty = P<EquipRepairOperationRec>.Register(e => e.EngineerConfirmResult);

        /// <summary>
        /// 工程确认结果
        /// </summary>
        public EngineerConfirmResult? EngineerConfirmResult
        {
            get { return this.GetProperty(EngineerConfirmResultProperty); }
            set { this.SetProperty(EngineerConfirmResultProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<EquipRepairOperationRec>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 间隔时间(小时) IntervalTime
        /// <summary>
        /// 间隔时间(小时)
        /// </summary>
        [Label("间隔时间(小时)")]
        public static readonly Property<decimal?> IntervalTimeProperty = P<EquipRepairOperationRec>.Register(e => e.IntervalTime);

        /// <summary>
        /// 间隔时间(小时)
        /// </summary>
        public decimal? IntervalTime
        {
            get { return this.GetProperty(IntervalTimeProperty); }
            set { this.SetProperty(IntervalTimeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 设备维修单操作记录 实体配置
    /// </summary>
    internal class EquipRepairOperationRecConfig : EntityConfig<EquipRepairOperationRec>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(EquipRepairOperationRec.OriginalRepairerProperty, new StringLengthRangeRule() { Max = 2000 });
            rules.AddRule(EquipRepairOperationRec.RemarkProperty, new StringLengthRangeRule() { Max = 2000 });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_REPAIR_OP_REC").MapAllProperties();
            Meta.Property(EquipRepairOperationRec.OriginalRepairerProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EquipRepairOperationRec.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
