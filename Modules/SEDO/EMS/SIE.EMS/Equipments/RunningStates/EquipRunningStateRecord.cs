using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.RunningStates
{
    /// <summary>
    /// 设备运行状态记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EquipRunningStateRecordCriteria))]
    [Label("设备运行状态记录")]
    public class EquipRunningStateRecord : DataEntity
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<EquipRunningStateRecord>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty
            = P<EquipRunningStateRecord>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 在线状态 DeviceOnLineState
        /// <summary>
        /// 在线状态
        /// </summary>
        [Label("在线状态")]
        public static readonly Property<EquipOnLineState> EquipOnLineStateProperty = P<EquipRunningStateRecord>.Register(e => e.EquipOnLineState);

        /// <summary>
        /// 在线状态
        /// </summary>
        public EquipOnLineState EquipOnLineState
        {
            get { return this.GetProperty(EquipOnLineStateProperty); }
            set { this.SetProperty(EquipOnLineStateProperty, value); }
        }
        #endregion

        #region 运行状态 EquipRunningState
        /// <summary>
        /// 运行状态
        /// </summary>
        [Label("运行状态")]
        public static readonly Property<EquipRunningState> EquipRunningStateProperty = P<EquipRunningStateRecord>.Register(e => e.EquipRunningState);

        /// <summary>
        /// 运行状态
        /// </summary>
        public EquipRunningState EquipRunningState
        {
            get { return this.GetProperty(EquipRunningStateProperty); }
            set { this.SetProperty(EquipRunningStateProperty, value); }
        }
        #endregion

        #region 状态对应时间  AtWhatTime
        /// <summary>
        /// 状态对应时间
        /// </summary>
        [Label("状态对应时间")]
        public static readonly Property<DateTime> AtWhatTimeProperty = P<EquipRunningStateRecord>.Register(e => e.AtWhatTime);

        /// <summary>
        /// 状态对应时间
        /// </summary>
        public DateTime AtWhatTime
        {
            get { return GetProperty(AtWhatTimeProperty); }
            set { SetProperty(AtWhatTimeProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<EquipRunningStateRecord>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<EquipRunningStateRecord>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 设备型号维护 实体配置
    /// </summary>
    internal class EquipRunningStateRecordConfig : EntityConfig<EquipRunningStateRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_STATE_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}