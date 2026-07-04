using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.RunStandards;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Accounts
{
    /// <summary>
    /// 设备台账维修定标
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备台账维修定标")]
    public class EquipAccountRepairStandardBase : DataEntity
    {
        #region 周期量 Amount
        /// <summary>
        /// 周期量
        /// </summary>
        [Label("周期量")]
        public static readonly Property<int> AmountProperty = P<EquipAccountRepairStandardBase>.Register(e => e.Amount);

        /// <summary>
        /// 周期量
        /// </summary>
        public int Amount
        {
            get { return GetProperty(AmountProperty); }
            set { SetProperty(AmountProperty, value); }
        }
        #endregion

        #region 累加数 RoundAmount
        /// <summary>
        /// 累加数
        /// </summary>
        [Label("累加数")]
        public static readonly Property<int> RoundAmountProperty = P<EquipAccountRepairStandardBase>.Register(e => e.RoundAmount);

        /// <summary>
        /// 累加数
        /// </summary>
        public int RoundAmount
        {
            get { return GetProperty(RoundAmountProperty); }
            set { SetProperty(RoundAmountProperty, value); }
        }
        #endregion

        #region 总读数 TotalAmount
        /// <summary>
        /// 总读数
        /// </summary>
        [Label("总读数")]
        public static readonly Property<int> TotalAmountProperty = P<EquipAccountRepairStandardBase>.Register(e => e.TotalAmount);

        /// <summary>
        /// 总读数
        /// </summary>
        public int TotalAmount
        {
            get { return GetProperty(TotalAmountProperty); }
            set { SetProperty(TotalAmountProperty, value); }
        }
        #endregion

        #region 上次执行日期 LastExecuteDate
        /// <summary>
        /// 上次执行日期
        /// </summary>
        [Label("上次执行日期")]
        public static readonly Property<DateTime?> LastExecuteDateProperty = P<EquipAccountRepairStandardBase>.Register(e => e.LastExecuteDate);

        /// <summary>
        /// 上次执行日期
        /// </summary>
        public DateTime? LastExecuteDate
        {
            get { return GetProperty(LastExecuteDateProperty); }
            set { SetProperty(LastExecuteDateProperty, value); }
        }
        #endregion

        #region 下次执行日期 NextExecuteDate
        /// <summary>
        /// 下次执行日期
        /// </summary>
        [Label("下次执行日期")]
        public static readonly Property<DateTime?> NextExecuteDateProperty = P<EquipAccountRepairStandardBase>.Register(e => e.NextExecuteDate);

        /// <summary>
        /// 下次执行日期
        /// </summary>
        public DateTime? NextExecuteDate
        {
            get { return GetProperty(NextExecuteDateProperty); }
            set { SetProperty(NextExecuteDateProperty, value); }
        }
        #endregion

        #region 预警期 LeadTime
        /// <summary>
        /// 预警期
        /// </summary>
        [Label("预警期")]
        public static readonly Property<int> LeadTimeProperty = P<EquipAccountRepairStandardBase>.Register(e => e.LeadTime);

        /// <summary>
        /// 预警期
        /// </summary>
        public int LeadTime
        {
            get { return GetProperty(LeadTimeProperty); }
            set { SetProperty(LeadTimeProperty, value); }
        }
        #endregion

        #region 定标量 RunStandardValue
        /// <summary>
        /// 定标量Id
        /// </summary>
        [Label("定标量")]
        public static readonly IRefIdProperty RunStandardValueIdProperty = P<EquipAccountRepairStandardBase>.RegisterRefId(e => e.RunStandardValueId, ReferenceType.Normal);

        /// <summary>
        /// 定标量Id
        /// </summary>
        public double RunStandardValueId
        {
            get { return (double)GetRefId(RunStandardValueIdProperty); }
            set { SetRefId(RunStandardValueIdProperty, value); }
        }

        /// <summary>
        /// 定标量
        /// </summary>
        public static readonly RefEntityProperty<RunStandardValue> RunStandardValueProperty = P<EquipAccountRepairStandardBase>.RegisterRef(e => e.RunStandardValue, RunStandardValueIdProperty);

        /// <summary>
        /// 定标量
        /// </summary>
        public RunStandardValue RunStandardValue
        {
            get { return GetRefEntity(RunStandardValueProperty); }
            set { SetRefEntity(RunStandardValueProperty, value); }
        }
        #endregion

        #region 定标单位 StandardUnit
        /// <summary>
        /// 定标单位
        /// </summary>
        [Label("定标单位")]
        public static readonly Property<StandardUnit> StandardUnitProperty = P<EquipAccountRepairStandardBase>.Register(e => e.StandardUnit);

        /// <summary>
        /// 定标单位
        /// </summary>
        public StandardUnit StandardUnit
        {
            get { return GetProperty(StandardUnitProperty); }
            set { SetProperty(StandardUnitProperty, value); }
        }
        #endregion

        #region 定标类型 StandardType
        /// <summary>
        /// 定标类型
        /// </summary>
        [Label("定标类型")]
        public static readonly Property<StandardType> StandardTypeProperty = P<EquipAccountRepairStandardBase>.Register(e => e.StandardType);

        /// <summary>
        /// 定标类型
        /// </summary>
        public StandardType StandardType
        {
            get { return GetProperty(StandardTypeProperty); }
            set { SetProperty(StandardTypeProperty, value); }
        }
        #endregion


        #region 定标单号 RunStandardNo	
        /// <summary>
        /// 定标单号
        /// </summary>
        [Label("定标单号")]
        public static readonly Property<string> RunStandardNoProperty = P<EquipAccountRepairStandardBase>.RegisterView(e => e.RunStandardNo, p => p.RunStandardValue.RunStandard.No);

        /// <summary>
        /// 定标单号
        /// </summary>
        public string RunStandardNo
        {
            get { return this.GetProperty(RunStandardNoProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 设备台账维修定标 实体配置
    /// </summary>
    internal class EquipAccountRepairStandardBaseConfig : EntityConfig<EquipAccountRepairStandardBase>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQP_RUN_STD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}