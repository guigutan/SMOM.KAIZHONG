using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.Maintains.Controller;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;

namespace SIE.EMS.Maintains.Records
{
    /// <summary>
    /// 设备保养记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备保养记录查询实体")]
    public partial class MaintainRecordCriteria : Criteria
    {
        #region 保养单号 MaintainNo
        /// <summary>
        /// 保养单号
        /// </summary>
        [Label("保养单号")]
        public static readonly Property<string> MaintainNoProperty = P<MaintainRecordCriteria>.Register(e => e.MaintainNo);

        /// <summary>
        /// 保养单号
        /// </summary>
        public string MaintainNo
        {
            get { return this.GetProperty(MaintainNoProperty); }
            set { this.SetProperty(MaintainNoProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<MaintainRecordCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)GetRefNullableId(EquipAccountIdProperty); }
            set { SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty
            = P<MaintainRecordCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 设备名称 MachineName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> MachineNameProperty = P<MaintainRecordCriteria>.Register(e => e.MachineName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string MachineName
        {
            get { return GetProperty(MachineNameProperty); }
            set { SetProperty(MachineNameProperty, value); }
        }
        #endregion

        #region 车间 Workshop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkshopIdProperty = P<MaintainRecordCriteria>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkshopId
        {
            get { return (double?)GetRefNullableId(WorkshopIdProperty); }
            set { SetRefNullableId(WorkshopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkshopProperty = P<MaintainRecordCriteria>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return GetRefEntity(WorkshopProperty); }
            set { SetRefEntity(WorkshopProperty, value); }
        }
        #endregion

        #region 产线名称 ResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> ResourceNameProperty = P<MaintainRecordCriteria>.Register(e => e.ResourceName);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<MaintainRecordCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<MaintainRecordCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 计划保养日期 PlanBeginDate
        /// <summary>
        /// 计划保养日期
        /// </summary>
        [Label("计划保养日期")]
        public static readonly Property<DateRange> PlanMaintainDateProperty = P<MaintainRecordCriteria>.Register(e => e.PlanMaintainDate);

        /// <summary>
        /// 计划保养日期
        /// </summary>
        public DateRange PlanMaintainDate
        {
            get { return this.GetProperty(PlanMaintainDateProperty); }
            set { this.SetProperty(PlanMaintainDateProperty, value); }
        }
        #endregion


        #region 保养状态 ExeState
        /// <summary>
        /// 保养状态
        /// </summary>
        [Label("保养状态")]
        public static readonly Property<MaintExeState?> ExeStateProperty = P<MaintainRecordCriteria>.Register(e => e.ExeState);

        /// <summary>
        /// 保养状态
        /// </summary>
        public MaintExeState? ExeState
        {
            get { return GetProperty(ExeStateProperty); }
            set { SetProperty(ExeStateProperty, value); }
        }
        #endregion

        #region 执行结果 ExeResult
        /// <summary>
        /// 执行结果
        /// </summary>
        [Label("执行结果")]
        public static readonly Property<ExeResult?> ExeResultProperty = P<MaintainRecordCriteria>.Register(e => e.ExeResult);

        /// <summary>
        /// 执行结果
        /// </summary>
        public ExeResult? ExeResult
        {
            get { return this.GetProperty(ExeResultProperty); }
            set { this.SetProperty(ExeResultProperty, value); }
        }
        #endregion

        #region 确认结果 ConfirmResult
        /// <summary>
        /// 确认结果
        /// </summary>
        [Label("确认结果")]
        public static readonly Property<ConfirmResult?> ConfirmResultProperty = P<MaintainRecordCriteria>.Register(e => e.ConfirmResult);

        /// <summary>
        /// 确认结果
        /// </summary>
        public ConfirmResult? ConfirmResult
        {
            get { return GetProperty(ConfirmResultProperty); }
            set { SetProperty(ConfirmResultProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<MaintainController>().QueryMaintainPlanLog(this);
        }
    }
}
