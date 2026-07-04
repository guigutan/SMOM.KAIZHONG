using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.AlarmStates
{
    /// <summary>
    /// 报警统计查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("报警统计查询实体")]
    public partial class AlarmCountCriteria : Criteria
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<AlarmCountCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

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
            = P<AlarmCountCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
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
        public static readonly IRefIdProperty EquipModelIdProperty = P<AlarmCountCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<AlarmCountCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

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
        public static readonly IRefIdProperty EquipTypeIdProperty = P<AlarmCountCriteria>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<AlarmCountCriteria>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return GetRefEntity(EquipTypeProperty); }
            set { SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 报警时间  AlarmTime
        /// <summary>
        /// 报警时间
        /// </summary>
        [Label("报警时间")]
        public static readonly Property<DateRange> AlarmTimeProperty = P<AlarmCountCriteria>.Register(e => e.AlarmTime);

        /// <summary>
        /// 报警时间
        /// </summary>
        public DateRange AlarmTime
        {
            get { return GetProperty(AlarmTimeProperty); }
            set { SetProperty(AlarmTimeProperty, value); }
        }
        #endregion




        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
		{
            return RT.Service.Resolve<AlarmController>().CriteriaAlarmCount(this);
        }
    }
}
