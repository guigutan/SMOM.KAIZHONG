using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.RunningStates
{
    /// <summary>
    /// 设备运行状态记录的查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备运行状态记录的查询实体")]
    public class EquipRunningStateRecordCriteria : Criteria
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<EquipRunningStateRecordCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty
            = P<EquipRunningStateRecordCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

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
        public static readonly Property<EquipOnLineState?> EquipOnLineStateProperty = P<EquipRunningStateRecordCriteria>.Register(e => e.EquipOnLineState);

        /// <summary>
        /// 在线状态
        /// </summary>
        public EquipOnLineState? EquipOnLineState
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
        public static readonly Property<EquipRunningState?> EquipRunningStateProperty = P<EquipRunningStateRecordCriteria>.Register(e => e.EquipRunningState);

        /// <summary>
        /// 运行状态
        /// </summary>
        public EquipRunningState? EquipRunningState
        {
            get { return this.GetProperty(EquipRunningStateProperty); }
            set { this.SetProperty(EquipRunningStateProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询设备运行状态记录
        /// </summary>
        /// <returns>设备运行状态记录列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EquipController>().QueryEquipRunningStateRecords(this);
        }
    }
}
