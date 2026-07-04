using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.AlarmStates
{
    /// <summary>
    ///报警统计
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AlarmCountCriteria))]
    [Label("报警统计")]
    public class AlarmCount : ViewModel
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Required]
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<AlarmCount>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<AlarmCount>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 报警总数 AlarmSum
        /// <summary>
        /// 报警总数
        /// </summary>
        [Label("报警总数")]
        public static readonly Property<int> AlarmSumProperty = P<AlarmCount>.Register(e => e.AlarmSum);

        /// <summary>
        /// 报警总数
        /// </summary>
        public int AlarmSum
        {
            get { return this.GetProperty(AlarmSumProperty); }
            set { this.SetProperty(AlarmSumProperty, value); }
        }
        #endregion

        #region 紧急 Serious
        /// <summary>
        /// 紧急
        /// </summary>
        [Label("紧急")]
        public static readonly Property<int> SeriousProperty = P<AlarmCount>.Register(e => e.Serious);

        /// <summary>
        /// 紧急
        /// </summary>
        public int Serious
        {
            get { return this.GetProperty(SeriousProperty); }
            set { this.SetProperty(SeriousProperty, value); }
        }
        #endregion

        #region 严重 Major
        /// <summary>
        /// 严重
        /// </summary>
        [Label("严重")]
        public static readonly Property<int> MajorProperty = P<AlarmCount>.Register(e => e.Major);

        /// <summary>
        /// 严重
        /// </summary>
        public int Major
        {
            get { return this.GetProperty(MajorProperty); }
            set { this.SetProperty(MajorProperty, value); }
        }
        #endregion

        #region 一般 Medium
        /// <summary>
        /// 一般
        /// </summary>
        [Label("一般")]
        public static readonly Property<int> MediumProperty = P<AlarmCount>.Register(e => e.Medium);

        /// <summary>
        /// 一般
        /// </summary>
        public int Medium
        {
            get { return this.GetProperty(MediumProperty); }
            set { this.SetProperty(MediumProperty, value); }
        }
        #endregion

        #region 轻微 Minor
        /// <summary>
        /// 轻微
        /// </summary>
        [Label("轻微")]
        public static readonly Property<int> MinorProperty = P<AlarmCount>.Register(e => e.Minor);

        /// <summary>
        /// 轻微
        /// </summary>
        public int Minor
        {
            get { return this.GetProperty(MinorProperty); }
            set { this.SetProperty(MinorProperty, value); }
        }
        #endregion

        #region 提示 Info
        /// <summary>
        /// 提示
        /// </summary>
        [Label("提示")]
        public static readonly Property<int> InfoProperty = P<AlarmCount>.Register(e => e.Info);

        /// <summary>
        /// 提示
        /// </summary>
        public int Info
        {
            get { return this.GetProperty(InfoProperty); }
            set { this.SetProperty(InfoProperty, value); }
        }
        #endregion

        #region 开启状态 Alarm
        /// <summary>
        /// 开启状态
        /// </summary>
        [Label("开启状态")]
        public static readonly Property<int> AlarmProperty = P<AlarmCount>.Register(e => e.Alarm);

        /// <summary>
        /// 开启状态
        /// </summary>
        public int Alarm
        {
            get { return this.GetProperty(AlarmProperty); }
            set { this.SetProperty(AlarmProperty, value); }
        }
        #endregion

        #region 关闭状态 Close
        /// <summary>
        /// 关闭状态
        /// </summary>
        [Label("关闭状态")]
        public static readonly Property<int> CloseProperty = P<AlarmCount>.Register(e => e.Close);

        /// <summary>
        /// 关闭状态
        /// </summary>
        public int Close
        {
            get { return this.GetProperty(CloseProperty); }
            set { this.SetProperty(CloseProperty, value); }
        }
        #endregion

        #region 设备编码 AlarmEquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> AlarmEquipAccountCodeProperty = P<AlarmCount>.Register(e => e.AlarmEquipAccountCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string AlarmEquipAccountCode
        {
            get { return this.GetProperty(AlarmEquipAccountCodeProperty); }
            set { this.SetProperty(AlarmEquipAccountCodeProperty, value); }
        }
        #endregion

        #region 设备名称 AlarmEquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> AlarmEquipAccountNameProperty = P<AlarmCount>.Register(e => e.AlarmEquipAccountName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string AlarmEquipAccountName
        {
            get { return this.GetProperty(AlarmEquipAccountNameProperty); }
            set { this.SetProperty(AlarmEquipAccountNameProperty, value); }
        }
        #endregion

        #region 设备型号编码 AlarmEquipModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> AlarmEquipModelCodeProperty = P<AlarmCount>.Register(e => e.AlarmEquipModelCode);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string AlarmEquipModelCode
        {
            get { return this.GetProperty(AlarmEquipModelCodeProperty); }
            set { this.SetProperty(AlarmEquipModelCodeProperty, value); }
        }
        #endregion

        #region 设备型号名称 AlarmEquipModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> AlarmEquipModelNameProperty = P<AlarmCount>.Register(e => e.AlarmEquipModelName);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string AlarmEquipModelName
        {
            get { return this.GetProperty(AlarmEquipModelNameProperty); }
            set { this.SetProperty(AlarmEquipModelNameProperty, value); }
        }
        #endregion

        #region 设备类型编码 AlarmEquipTypeeCode
        /// <summary>
        /// 设备类型编码
        /// </summary>
        [Label("设备类型编码")]
        public static readonly Property<string> AlarmEquipTypeeCodeProperty = P<AlarmCount>.Register(e => e.AlarmEquipTypeeCode);

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public string AlarmEquipTypeeCode
        {
            get { return this.GetProperty(AlarmEquipTypeeCodeProperty); }
            set { this.SetProperty(AlarmEquipTypeeCodeProperty, value); }
        }
        #endregion

        #region 设备类型名称 AlarmEquipTypeeName
        /// <summary>
        /// 设备类型名称
        /// </summary>
        [Label("设备类型名称")]
        public static readonly Property<string> AlarmEquipTypeeNameProperty = P<AlarmCount>.Register(e => e.AlarmEquipTypeeName);

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string AlarmEquipTypeeName
        {
            get { return this.GetProperty(AlarmEquipTypeeNameProperty); }
            set { this.SetProperty(AlarmEquipTypeeNameProperty, value); }
        }
        #endregion

    }
}