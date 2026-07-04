using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.DeviceControls
{
    /// <summary>
    /// 设备控制记录查询实体
    /// </summary>
    [Label("设备控制记录查询实体")]
    [QueryEntity, Serializable]
    public class DeviceControlCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeviceControlCriteria()
        {
            OpearDateTime = new DateRange();
            OpearDateTime.DateTimePart = DateTimePart.Date;
            OpearDateTime.DateRangeType = DateRangeType.Today;
        }

        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<DeviceControlCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)GetRefNullableId(EquipAccountIdProperty); }
            set { SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        [Label("设备台账")]
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<DeviceControlCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 来源 SourceControl
        /// <summary>
        /// 来源Id
        /// </summary>
        [Label("来源")]
        public static readonly IRefIdProperty SourceControlIdProperty = P<DeviceControlCriteria>.RegisterRefId(e => e.SourceControlId, ReferenceType.Normal);

        /// <summary>
        /// 来源Id
        /// </summary>
        public double? SourceControlId
        {
            get { return (double?)GetRefNullableId(SourceControlIdProperty); }
            set { SetRefNullableId(SourceControlIdProperty, value); }
        }

        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly RefEntityProperty<SourceControl> SourceControlProperty = P<DeviceControlCriteria>.RegisterRef(e => e.SourceControl, SourceControlIdProperty);

        /// <summary>
        /// 来源
        /// </summary>
        public SourceControl SourceControl
        {
            get { return GetRefEntity(SourceControlProperty); }
            set { SetRefEntity(SourceControlProperty, value); }
        }
        #endregion

        #region 是否停机 IsStop
        /// <summary>
        /// 是否停机
        /// </summary>
        [Label("是否停机")]
        public static readonly Property<bool?> IsStopProperty = P<DeviceControlCriteria>.Register(e => e.IsStop);

        /// <summary>
        /// 是否停机
        /// </summary>
        public bool? IsStop
        {
            get { return GetProperty(IsStopProperty); }
            set { SetProperty(IsStopProperty, value); }
        }
        #endregion

        #region 操作时间 OpearDateTime
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateRange> OpearDateTimeProperty = P<DeviceControlCriteria>.Register(e => e.OpearDateTime);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateRange OpearDateTime
        {
            get { return GetProperty(OpearDateTimeProperty); }
            set { SetProperty(OpearDateTimeProperty, value); }
        }
        #endregion

        #region 是否生效 IsEffective
        /// <summary>
        /// 是否生效
        /// </summary>
        [Label("是否生效")]
        public static readonly Property<bool?> IsEffectiveProperty = P<DeviceControlCriteria>.Register(e => e.IsEffective);

        /// <summary>
        /// 是否生效
        /// </summary>
        public bool? IsEffective
        {
            get { return GetProperty(IsEffectiveProperty); }
            set { SetProperty(IsEffectiveProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询实体 获取查询结果
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DeviceControlController>().GetDeviceControls(this);
        }
    }
}