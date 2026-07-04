using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Report.EquipmentMixReport
{
    /// <summary>
    /// 设备综合统计报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EquipmentMixReportMonViewModelCriteria))]
    [Label("设备综合统计报表")]
    public class EquipmentMixReportMonViewModel : ViewModel
    {
        #region 首列 FirstColumn
        /// <summary>
        /// 首列
        /// </summary>
        [Label("")]
        public static readonly Property<string> FirstColumnProperty = P<EquipmentMixReportMonViewModel>.Register(e => e.FirstColumn);

        /// <summary>
        /// 
        /// </summary>
        public string FirstColumn
        {
            get { return this.GetProperty(FirstColumnProperty); }
            set { this.SetProperty(FirstColumnProperty, value); }
        }
        #endregion

        #region 1月 January
        /// <summary>
        /// 1月
        /// </summary>
        [Label("1月")]
        public static readonly Property<string> JanuaryProperty = P<EquipmentMixReportMonViewModel>.Register(e => e.January);

        /// <summary>
        /// 1月
        /// </summary>
        public string January
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
        public static readonly Property<string> FebruaryProperty = P<EquipmentMixReportMonViewModel>.Register(e => e.February);

        /// <summary>
        /// 2月
        /// </summary>
        public string February
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
        public static readonly Property<string> MarchProperty = P<EquipmentMixReportMonViewModel>.Register(e => e.March);

        /// <summary>
        /// 3月
        /// </summary>
        public string March
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
        public static readonly Property<string> AprilProperty = P<EquipmentMixReportMonViewModel>.Register(e => e.April);

        /// <summary>
        /// 4月
        /// </summary>
        public string April
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
        public static readonly Property<string> MayProperty = P<EquipmentMixReportMonViewModel>.Register(e => e.May);

        /// <summary>
        /// 5月
        /// </summary>
        public string May
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
        public static readonly Property<string> JuneProperty = P<EquipmentMixReportMonViewModel>.Register(e => e.June);

        /// <summary>
        /// 6月
        /// </summary>
        public string June
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
        public static readonly Property<string> JulyProperty = P<EquipmentMixReportMonViewModel>.Register(e => e.July);

        /// <summary>
        /// 7月
        /// </summary>
        public string July
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
        public static readonly Property<string> AugustProperty = P<EquipmentMixReportMonViewModel>.Register(e => e.August);

        /// <summary>
        /// 8月
        /// </summary>
        public string August
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
        public static readonly Property<string> SeptemberProperty = P<EquipmentMixReportMonViewModel>.Register(e => e.September);

        /// <summary>
        /// 9月
        /// </summary>
        public string September
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
        public static readonly Property<string> OctoberProperty = P<EquipmentMixReportMonViewModel>.Register(e => e.October);

        /// <summary>
        /// 10月
        /// </summary>
        public string October
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
        public static readonly Property<string> NovemberProperty = P<EquipmentMixReportMonViewModel>.Register(e => e.November);

        /// <summary>
        /// 11月
        /// </summary>
        public string November
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
        public static readonly Property<string> DecemberProperty = P<EquipmentMixReportMonViewModel>.Register(e => e.December);

        /// <summary>
        /// 12月
        /// </summary>
        public string December
        {
            get { return this.GetProperty(DecemberProperty); }
            set { this.SetProperty(DecemberProperty, value); }
        }
        #endregion

    }
}
