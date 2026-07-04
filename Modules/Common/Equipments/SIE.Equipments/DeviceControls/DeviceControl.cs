using SIE.Common.Configs;
using SIE.Domain;
using SIE.Equipments.DeviceControls.Configs;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.DeviceControls
{
    /// <summary>
    /// 设备控制记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DeviceControlCriteria))]
    [EntityWithConfig(typeof(SmdcUrlConfig))]
    [Label("设备控制记录")]
    public partial class DeviceControl : DataEntity
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<DeviceControl>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<DeviceControl>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 来源 Source
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<string> SourceProperty = P<DeviceControl>.Register(e => e.Source);

        /// <summary>
        /// 来源
        /// </summary>
        public string Source
        {
            get { return GetProperty(SourceProperty); }
            set { SetProperty(SourceProperty, value); }
        }
        #endregion

        #region 是否停机 IsStop
        /// <summary>
        /// 是否停机
        /// </summary>
        [Label("是否停机")]
        public static readonly Property<bool> IsStopProperty = P<DeviceControl>.Register(e => e.IsStop);

        /// <summary>
        /// 是否停机
        /// </summary>
        public bool IsStop
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
        public static readonly Property<DateTime> OpearDateTimeProperty = P<DeviceControl>.Register(e => e.OpearDateTime);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OpearDateTime
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
        public static readonly Property<bool> IsEffectiveProperty = P<DeviceControl>.Register(e => e.IsEffective);

        /// <summary>
        /// 是否生效
        /// </summary>
        public bool IsEffective
        {
            get { return GetProperty(IsEffectiveProperty); }
            set { SetProperty(IsEffectiveProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 设备控制记录 实体配置
    /// </summary>
    internal class DeviceControlConfig : EntityConfig<DeviceControl>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_DEVICE_CONTROL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
