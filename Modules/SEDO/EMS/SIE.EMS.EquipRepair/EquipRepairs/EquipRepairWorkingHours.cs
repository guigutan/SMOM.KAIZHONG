using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.EquipRepairs.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备维修工时
    /// </summary>
    [RootEntity, Serializable]
    [Label("维修工时")]
    public partial class EquipRepairWorkingHours : DataEntity
    {
        #region 设备维修单 EquipRepairBill
        /// <summary>
        /// 设备维修单Id
        /// </summary>
        [Label("设备维修单")]
        public static readonly IRefIdProperty EquipRepairBillIdProperty =
            P<EquipRepairWorkingHours>.RegisterRefId(e => e.EquipRepairBillId, ReferenceType.Parent);

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
            P<EquipRepairWorkingHours>.RegisterRef(e => e.EquipRepairBill, EquipRepairBillIdProperty);

        /// <summary>
        /// 设备维修单
        /// </summary>
        public EquipRepairBill EquipRepairBill
        {
            get { return this.GetRefEntity(EquipRepairBillProperty); }
            set { this.SetRefEntity(EquipRepairBillProperty, value); }
        }
        #endregion

        #region 维修人员 Repairer
        /// <summary>
        /// 维修人员Id
        /// </summary>
        [Label("维修人员")]
        public static readonly IRefIdProperty RepairerIdProperty =
            P<EquipRepairWorkingHours>.RegisterRefId(e => e.RepairerId, ReferenceType.Normal);

        /// <summary>
        /// 维修人员Id
        /// </summary>
        public double? RepairerId
        {
            get { return (double?)this.GetRefNullableId(RepairerIdProperty); }
            set { this.SetRefNullableId(RepairerIdProperty, value); }
        }

        /// <summary>
        /// 维修人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> RepairerProperty =
            P<EquipRepairWorkingHours>.RegisterRef(e => e.Repairer, RepairerIdProperty);

        /// <summary>
        /// 维修人员
        /// </summary>
        public Employee Repairer
        {
            get { return this.GetRefEntity(RepairerProperty); }
            set { this.SetRefEntity(RepairerProperty, value); }
        }
        #endregion

        #region 开始时间 BeginTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime?> BeginTimeProperty = P<EquipRepairWorkingHours>.Register(e => e.BeginTime);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime
        {
            get { return this.GetProperty(BeginTimeProperty); }
            set { this.SetProperty(BeginTimeProperty, value); }
        }
        #endregion

        #region 结束时间 EndTime
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime?> EndTimeProperty = P<EquipRepairWorkingHours>.Register(e => e.EndTime);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get { return this.GetProperty(EndTimeProperty); }
            set { this.SetProperty(EndTimeProperty, value); }
        }
        #endregion

        #region 是否维修责任人 IsRepairMaster
        /// <summary>
        /// 是否维修责任人
        /// </summary>
        [Label("维修责任人")]
        public static readonly Property<bool> IsRepairMasterProperty = P<EquipRepairWorkingHours>.Register(e => e.IsRepairMaster);

        /// <summary>
        /// 是否维修责任人
        /// </summary>
        public bool IsRepairMaster
        {
            get { return this.GetProperty(IsRepairMasterProperty); }
            set { this.SetProperty(IsRepairMasterProperty, value); }
        }
        #endregion

        #region 是否当前维修人 IsRepairEmployee        
        /// <summary>
        /// 是否当前维修人（如果维修单已经转派，则是否当前维修人为False）
        /// </summary>
        [Label("当前维修人")]
        public static readonly Property<bool> IsRepairEmployeeProperty = P<EquipRepairWorkingHours>.Register(e => e.IsRepairEmployee);

        /// <summary>
        /// 是否当前维修人
        /// </summary>
        public bool IsRepairEmployee
        {
            get { return this.GetProperty(IsRepairEmployeeProperty); }
            set { this.SetProperty(IsRepairEmployeeProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 维修人员名称 RepairerNameView
        /// <summary>
        /// 维修人员名称
        /// </summary>
        [Label("维修人员名称")]
        public static readonly Property<string> RepairerNameViewProperty = P<EquipRepairWorkingHours>.RegisterView(e => e.RepairerNameView, p => p.Repairer.Name);

        /// <summary>
        /// 维修人员名称
        /// </summary>
        public string RepairerNameView
        {
            get { return this.GetProperty(RepairerNameViewProperty); }
        }
        #endregion

        #region 维修单状态 RepairBillState
        /// <summary>
        /// 维修单状态
        /// </summary>
        [Label("维修单状态")]
        public static readonly Property<EquipRepairState> RepairBillStateProperty = P<EquipRepairWorkingHours>.RegisterView(e => e.RepairBillState, p => p.EquipRepairBill.RepairState);

        /// <summary>
        /// 维修单状态
        /// </summary>
        public EquipRepairState RepairBillState
        {
            get { return this.GetProperty(RepairBillStateProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 设备维修工时 实体配置
    /// </summary>
    internal class EquipRepairWorkingHoursConfig : EntityConfig<EquipRepairWorkingHours>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new HandlerRule
            {
                Handler = (o, e) =>
                {
                    var workingHours = o.CastTo<EquipRepairWorkingHours>();
                    var errMsg = string.Empty;
                    if (workingHours.BeginTime.HasValue && workingHours.EndTime.HasValue)
                    {
                        if (workingHours.BeginTime.Value > workingHours.EndTime.Value)
                            errMsg = "开始时间不能大于结束时间".L10N();
                    }
                    if (errMsg.IsNotEmpty())
                        e.BrokenDescription = errMsg;
                }
            });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_REPAIR_WH").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
