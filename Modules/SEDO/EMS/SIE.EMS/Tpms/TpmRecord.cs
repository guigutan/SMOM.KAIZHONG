using SIE.Common.Configs;
using SIE.Domain;
using SIE.EMS.Tpms.Configs;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.Tpms
{
    /// <summary>
    /// TPM操作记录
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [ConditionQueryType(typeof(TpmRecordCriteria))]
    [EntityWithConfig(typeof(TpmScoreNoConfig))]
    [Label("TPM操作记录")]
    public partial class TpmRecord : DataEntity
    {
        #region 执行时间 ExecutionTime
        /// <summary>
        /// 执行时间
        /// </summary>
        [Required]
        [Label("执行时间")]
        public static readonly Property<DateTime> ExecutionTimeProperty = P<TpmRecord>.Register(e => e.ExecutionTime);

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecutionTime
        {
            get { return GetProperty(ExecutionTimeProperty); }
            set { SetProperty(ExecutionTimeProperty, value); }
        }
        #endregion

        #region TPM单号 TpmNo
        /// <summary>
        /// TPM单号
        /// </summary>
        [Required]
        [Label("TPM单号")]
        public static readonly Property<string> TpmNoProperty = P<TpmRecord>.Register(e => e.TpmNo);

        /// <summary>
        /// TPM单号
        /// </summary>
        public string TpmNo
        {
            get { return GetProperty(TpmNoProperty); }
            set { SetProperty(TpmNoProperty, value); }
        }
        #endregion        

        #region 总评分 TotalScore
        /// <summary>
        /// 总评分
        /// </summary>
        [Required]
        [Label("总评分")]
        public static readonly Property<int> TotalScoreProperty = P<TpmRecord>.Register(e => e.TotalScore);

        /// <summary>
        /// 总评分
        /// </summary>
        public int TotalScore
        {
            get { return GetProperty(TotalScoreProperty); }
            set { SetProperty(TotalScoreProperty, value); }
        }
        #endregion

        #region 评分人 ScorerName
        /// <summary>
        /// 评分人
        /// </summary>
        [Label("评分人")]
        public static readonly Property<string> ScorerNameProperty = P<TpmRecord>.Register(e => e.ScorerName);

        /// <summary>
        /// 评分人
        /// </summary>
        public string ScorerName
        {
            get { return GetProperty(ScorerNameProperty); }
            set { SetProperty(ScorerNameProperty, value); }
        }
        #endregion

        #region 设备 Equip
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipmentIdProperty = P<TpmRecord>.RegisterRefId(e => e.EquipmentId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipmentId
        {
            get { return (double)GetRefId(EquipmentIdProperty); }
            set { SetRefId(EquipmentIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipmentProperty = P<TpmRecord>.RegisterRef(e => e.Equipment, EquipmentIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect Equipment
        {
            get { return GetRefEntity(EquipmentProperty); }
            set { SetRefEntity(EquipmentProperty, value); }
        }
        #endregion

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组")]
        public static readonly IRefIdProperty WorkGroupIdProperty = P<TpmRecord>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double WorkGroupId
        {
            get { return (double)GetRefId(WorkGroupIdProperty); }
            set { SetRefId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> WorkGroupProperty = P<TpmRecord>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public WorkGroup WorkGroup
        {
            get { return GetRefEntity(WorkGroupProperty); }
            set { SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion        

        #region 评分明细 DetailList
        /// <summary>
        /// 评分明细
        /// </summary>
        public static readonly ListProperty<EntityList<TpmRecordDetail>> DetailListProperty = P<TpmRecord>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 评分明细
        /// </summary>
        public EntityList<TpmRecordDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 视图属性

        #region 设备编码 EquipTypeCode
        /// <summary>
        /// 设备类型编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipTypeCodeProperty = P<TpmRecord>.RegisterView(e => e.EquipTypeCode, p => p.Equipment.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipTypeCode
        {
            get { return GetProperty(EquipTypeCodeProperty); }
            set { SetProperty(EquipTypeCodeProperty, value); }
        }
        #endregion

        #region 机台号 MachineNo
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> MachineNoProperty = P<TpmRecord>.RegisterView(e => e.MachineNo, p => p.Equipment.Name);

        /// <summary>
        /// 机台号
        /// </summary>
        public string MachineNo
        {
            get { return GetProperty(MachineNoProperty); }
            set { SetProperty(MachineNoProperty, value); }
        }
        #endregion        

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<TpmRecord>.RegisterView(e => e.WorkShopName, p => p.Equipment.WorkShop.Name);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName
        {
            get { return GetProperty(WorkShopNameProperty); }
            set { SetProperty(WorkShopNameProperty, value); }
        }
        #endregion        

        #region 工序 ProcessName
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<TpmRecord>.RegisterView(e => e.ProcessName, p => p.Equipment.Process.Name);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion                

        #endregion
    }

    /// <summary>
    /// TPM操作记录 实体配置
    /// </summary>
    internal class TpmRecordConfig : EntityConfig<TpmRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_TPM_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}