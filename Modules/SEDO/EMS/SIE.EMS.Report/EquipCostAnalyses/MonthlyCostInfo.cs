using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Report.EquipCostAnalyses
{
    /// <summary>
    /// 月度成本分析
    /// </summary>
    [RootEntity, Serializable]
    [Label("月度成本分析")]
    public class MonthlyCostInfo : ViewModel
    {
        #region 成本项目 FirstColumn
        /// <summary>
        /// 成本项目
        /// </summary>
        [Label("成本项目")]
        public static readonly Property<string> CostItemProperty = P<MonthlyCostInfo>.Register(e => e.CostItem);

        /// <summary>
        /// 
        /// </summary>
        public string CostItem
        {
            get { return this.GetProperty(CostItemProperty); }
            set { this.SetProperty(CostItemProperty, value); }
        }
        #endregion

        #region 1月 January
        /// <summary>
        /// 1月
        /// </summary>
        [Label("1月")]
        public static readonly Property<decimal> JanuaryProperty = P<MonthlyCostInfo>.Register(e => e.January);

        /// <summary>
        /// 1月
        /// </summary>
        public decimal January
        {
            get { return this.GetProperty(JanuaryProperty); }
            set { this.SetProperty(JanuaryProperty, value); }
        }
        #endregion

        #region 2月 February
        /// <summary>
        /// 2月
        /// </summary>
        [Label("2月")]
        public static readonly Property<decimal> FebruaryProperty = P<MonthlyCostInfo>.Register(e => e.February);

        /// <summary>
        /// 2月
        /// </summary>
        public decimal February
        {
            get { return this.GetProperty(FebruaryProperty); }
            set { this.SetProperty(FebruaryProperty, value); }
        }
        #endregion

        #region 3月 March
        /// <summary>
        /// 3月
        /// </summary>
        [Label("3月")]
        public static readonly Property<decimal> MarchProperty = P<MonthlyCostInfo>.Register(e => e.March);

        /// <summary>
        /// 3月
        /// </summary>
        public decimal March
        {
            get { return this.GetProperty(MarchProperty); }
            set { this.SetProperty(MarchProperty, value); }
        }
        #endregion

        #region 4月 April
        /// <summary>
        /// 4月
        /// </summary>
        [Label("4月")]
        public static readonly Property<decimal> AprilProperty = P<MonthlyCostInfo>.Register(e => e.April);

        /// <summary>
        /// 4月
        /// </summary>
        public decimal April
        {
            get { return this.GetProperty(AprilProperty); }
            set { this.SetProperty(AprilProperty, value); }
        }
        #endregion

        #region 5月 May
        /// <summary>
        /// 4月
        /// </summary>
        [Label("5月")]
        public static readonly Property<decimal> MayProperty = P<MonthlyCostInfo>.Register(e => e.May);

        /// <summary>
        /// 5月
        /// </summary>
        public decimal May
        {
            get { return this.GetProperty(MayProperty); }
            set { this.SetProperty(MayProperty, value); }
        }
        #endregion

        #region 6月 June
        /// <summary>
        /// 6月
        /// </summary>
        [Label("6月")]
        public static readonly Property<decimal> JuneProperty = P<MonthlyCostInfo>.Register(e => e.June);

        /// <summary>
        /// 6月
        /// </summary>
        public decimal June
        {
            get { return this.GetProperty(JuneProperty); }
            set { this.SetProperty(JuneProperty, value); }
        }
        #endregion

        #region 7月 March
        /// <summary>
        /// 4月
        /// </summary>
        [Label("7月")]
        public static readonly Property<decimal> JulyProperty = P<MonthlyCostInfo>.Register(e => e.July);

        /// <summary>
        /// 7月
        /// </summary>
        public decimal July
        {
            get { return this.GetProperty(JulyProperty); }
            set { this.SetProperty(JulyProperty, value); }
        }
        #endregion

        #region 8月 August
        /// <summary>
        /// 8月
        /// </summary>
        [Label("8月")]
        public static readonly Property<decimal> AugustProperty = P<MonthlyCostInfo>.Register(e => e.August);

        /// <summary>
        /// 8月
        /// </summary>
        public decimal August
        {
            get { return this.GetProperty(AugustProperty); }
            set { this.SetProperty(AugustProperty, value); }
        }
        #endregion

        #region 9月 September
        /// <summary>
        /// 9月
        /// </summary>
        [Label("9月")]
        public static readonly Property<decimal> SeptemberProperty = P<MonthlyCostInfo>.Register(e => e.September);

        /// <summary>
        /// 9月
        /// </summary>
        public decimal September
        {
            get { return this.GetProperty(SeptemberProperty); }
            set { this.SetProperty(SeptemberProperty, value); }
        }
        #endregion

        #region 10月 October
        /// <summary>
        /// 10月
        /// </summary>
        [Label("10月")]
        public static readonly Property<decimal> OctoberProperty = P<MonthlyCostInfo>.Register(e => e.October);

        /// <summary>
        /// 10月
        /// </summary>
        public decimal October
        {
            get { return this.GetProperty(OctoberProperty); }
            set { this.SetProperty(OctoberProperty, value); }
        }
        #endregion

        #region 11月 November
        /// <summary>
        /// 11月
        /// </summary>
        [Label("11月")]
        public static readonly Property<decimal> NovemberProperty = P<MonthlyCostInfo>.Register(e => e.November);

        /// <summary>
        /// 11月
        /// </summary>
        public decimal November
        {
            get { return this.GetProperty(NovemberProperty); }
            set { this.SetProperty(NovemberProperty, value); }
        }
        #endregion

        #region 12月 December
        /// <summary>
        /// 12月
        /// </summary>
        [Label("12月")]
        public static readonly Property<decimal> DecemberProperty = P<MonthlyCostInfo>.Register(e => e.December);

        /// <summary>
        /// 12月
        /// </summary>
        public decimal December
        {
            get { return this.GetProperty(DecemberProperty); }
            set { this.SetProperty(DecemberProperty, value); }
        }
        #endregion
    }
}
