using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.AlarmStates
{
    /// <summary>
    /// 报警明细查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("报警明细查询实体")]
    public partial class EquipAlarmRecordCriteria : Criteria
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<EquipAlarmRecordCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty
            = P<EquipAlarmRecordCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 设备型号维护 EquipModel
        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty = P<EquipAlarmRecordCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double?)GetRefId(EquipModelIdProperty); }
            set { SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<EquipAlarmRecordCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty EquipTypeIdProperty = P<EquipAlarmRecordCriteria>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double? EquipTypeId
        {
            get { return (double?)GetRefId(EquipTypeIdProperty); }
            set { SetRefId(EquipTypeIdProperty, value); }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<EquipAlarmRecordCriteria>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return GetRefEntity(EquipTypeProperty); }
            set { SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 报警级别 AlarmLevel
        /// <summary>
        /// 报警级别
        /// </summary>
        [Label("报警级别")]
        public static readonly Property<AlarmLevel?> AlarmLevelProperty = P<EquipAlarmRecordCriteria>.Register(e => e.AlarmLevel);

        /// <summary>
        /// 报警级别
        /// </summary>
        public AlarmLevel? AlarmLevel
        {
            get { return this.GetProperty(AlarmLevelProperty); }
            set { this.SetProperty(AlarmLevelProperty, value); }
        }
        #endregion

        #region 报警类型 AlarmType
        /// <summary>
        /// 报警类型
        /// </summary>
        [Label("报警类型")]
        public static readonly Property<string> AlarmTypeProperty = P<EquipAlarmRecordCriteria>.Register(e => e.AlarmType);

        /// <summary>
        /// 报警类型
        /// </summary>
        public string AlarmType
        {
            get { return this.GetProperty(AlarmTypeProperty); }
            set { this.SetProperty(AlarmTypeProperty, value); }
        }
        #endregion

        #region 报警状态 AlarmState
        /// <summary>
        /// 报警状态
        /// </summary>
        [Label("报警状态")]
        public static readonly Property<AlarmState?> AlarmStateProperty = P<EquipAlarmRecordCriteria>.Register(e => e.AlarmState);

        /// <summary>
        /// 报警状态
        /// </summary>
        public AlarmState? AlarmState
        {
            get { return this.GetProperty(AlarmStateProperty); }
            set { this.SetProperty(AlarmStateProperty, value); }
        }
        #endregion

        #region 报警内容  AlarmContent
        /// <summary>
        /// 报警内容
        /// </summary>
        [Label("报警内容")]
        public static readonly Property<string> AlarmContentProperty = P<EquipAlarmRecordCriteria>.Register(e => e.AlarmContent);

        /// <summary>
        /// 报警内容
        /// </summary>
        public string AlarmContent
        {
            get { return GetProperty(AlarmContentProperty); }
            set { SetProperty(AlarmContentProperty, value); }
        }
        #endregion

        #region 报警时间  AlarmTime
        /// <summary>
        /// 报警时间
        /// </summary>
        [Label("报警时间")]
        public static readonly Property<DateRange> AlarmTimeProperty = P<EquipAlarmRecordCriteria>.Register(e => e.AlarmTime);

        /// <summary>
        /// 报警时间
        /// </summary>
        public DateRange AlarmTime
        {
            get { return GetProperty(AlarmTimeProperty); }
            set { SetProperty(AlarmTimeProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 设备名称 ViewEquipAccountName（可写入）
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> ViewEquipAccountNameProperty = P<EquipAlarmRecordCriteria>.RegisterView(e => e.ViewEquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string ViewEquipAccountName
        {
            get { return GetProperty(ViewEquipAccountNameProperty); }
            set { SetProperty(ViewEquipAccountNameProperty, value); }
        }
        #endregion
        #endregion




        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
		{
            return RT.Service.Resolve<AlarmController>().CriteriaAlarmDetail(this);
        }
    }
}
