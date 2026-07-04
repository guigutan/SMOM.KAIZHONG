using SIE.Domain;
using SIE.EMS.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.RunStandards
{
    /// <summary>
    /// 定标量
    /// </summary>
    [ChildEntity, Serializable]
    [Label("定标量")]
    public partial class RunStandardValue : DataEntity
    {
        #region 周期量 Amount
        /// <summary>
        /// 周期量
        /// </summary>
        [Label("周期量")]
        public static readonly Property<int> AmountProperty = P<RunStandardValue>.Register(e => e.Amount);

        /// <summary>
        /// 周期量
        /// </summary>
        public int Amount
        {
            get { return GetProperty(AmountProperty); }
            set { SetProperty(AmountProperty, value); }
        }
        #endregion

        #region 累加数 AmountOfRound
        /// <summary>
        /// 累加数
        /// </summary>
        [Label("累加数")]
        public static readonly Property<int> AmountOfRoundProperty = P<RunStandardValue>.Register(e => e.AmountOfRound);

        /// <summary>
        /// 累加数
        /// </summary>
        public int AmountOfRound
        {
            get { return GetProperty(AmountOfRoundProperty); }
            set { SetProperty(AmountOfRoundProperty, value); }
        }
        #endregion

        #region 总读数 TotalAmount
        /// <summary>
        /// 总读数
        /// </summary>
        [Label("总读数")]
        public static readonly Property<int> TotalAmountProperty = P<RunStandardValue>.Register(e => e.TotalAmount);

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
        public static readonly Property<DateTime?> LastExecuteDateProperty = P<RunStandardValue>.Register(e => e.LastExecuteDate);

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
        public static readonly Property<DateTime?> NextExecuteDateProperty = P<RunStandardValue>.Register(e => e.NextExecuteDate);

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
        public static readonly Property<int> LeadTimeProperty = P<RunStandardValue>.Register(e => e.LeadTime);

        /// <summary>
        /// 预警期
        /// </summary>
        public int LeadTime
        {
            get { return GetProperty(LeadTimeProperty); }
            set { SetProperty(LeadTimeProperty, value); }
        }
        #endregion

        #region  StandardUnit
        /// <summary>
        /// 
        /// </summary>
        [Label("单位")]
        public static readonly Property<StandardUnit> StandardUnitProperty = P<RunStandardValue>.Register(e => e.StandardUnit);

        /// <summary>
        /// 
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
        public static readonly Property<StandardType> StandardTypeProperty = P<RunStandardValue>.Register(e => e.StandardType);

        /// <summary>
        /// 定标类型
        /// </summary>
        public StandardType StandardType
        {
            get { return GetProperty(StandardTypeProperty); }
            set { SetProperty(StandardTypeProperty, value); }
        }
        #endregion

        #region 定标量 RunStandard
        /// <summary>
        /// 定标量Id
        /// </summary>
        [Label("定标量")]
        public static readonly IRefIdProperty RunStandardIdProperty = P<RunStandardValue>.RegisterRefId(e => e.RunStandardId, ReferenceType.Parent);

        /// <summary>
        /// 定标量Id
        /// </summary>
        public double RunStandardId
        {
            get { return (double)GetRefId(RunStandardIdProperty); }
            set { SetRefId(RunStandardIdProperty, value); }
        }

        /// <summary>
        /// 定标量
        /// </summary>
        public static readonly RefEntityProperty<RunStandard> RunStandardProperty = P<RunStandardValue>.RegisterRef(e => e.RunStandard, RunStandardIdProperty);

        /// <summary>
        /// 定标量
        /// </summary>
        public RunStandard RunStandard
        {
            get { return GetRefEntity(RunStandardProperty); }
            set { SetRefEntity(RunStandardProperty, value); }
        }
        #endregion


        #region 定标单号 RunStandardNo
        /// <summary>
        /// 定标单号
        /// </summary>
        [Label("定标单号")]
        public static readonly Property<string> RunStandardNoProperty = P<RunStandardValue>.RegisterView(e => e.RunStandardNo, p => p.RunStandard.No);

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
    /// 定标量 实体配置
    /// </summary>
    internal class RunStandardValueConfig : EntityConfig<RunStandardValue>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_RUN_STD_VALUE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}