using SIE.Core.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Report.MttrAndMtbfReports
{
    /// <summary>
    /// MTTR/MTBF统计报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(MttrAndMtbfReportViewModelCriteria))]
    [Label("MTTR/MTBF统计报表")]
    public class MttrAndMtbfReportViewModel : ViewModel
    {
        #region 统计项 StatiItem
        /// <summary>
        /// 统计项
        /// </summary>
        [Label("统计项")]
        public static readonly Property<string> StatiItemProperty = P<MttrAndMtbfReportViewModel>.Register(e => e.StatiItem);

        /// <summary>
        /// 统计项
        /// </summary>
        public string StatiItem
        {
            get { return this.GetProperty(StatiItemProperty); }
            set { this.SetProperty(StatiItemProperty, value); }
        }
        #endregion

        #region 1月 One
        /// <summary>
        /// 1月
        /// </summary>
        [Label("1月")]
        public static readonly Property<decimal> OneProperty = P<MttrAndMtbfReportViewModel>.Register(e => e.One);

        /// <summary>
        /// 1月
        /// </summary>
        public decimal One
        {
            get { return GetProperty(OneProperty); }
            set { SetProperty(OneProperty, value); }
        }
        #endregion

        #region 2月 Two
        /// <summary>
        /// 2月
        /// </summary>
        [Label("2月")]
        public static readonly Property<decimal> TwoProperty = P<MttrAndMtbfReportViewModel>.Register(e => e.Two);

        /// <summary>
        /// 2月
        /// </summary>
        public decimal Two
        {
            get { return GetProperty(TwoProperty); }
            set { SetProperty(TwoProperty, value); }
        }
        #endregion

        #region 3月 Three
        /// <summary>
        /// 3月
        /// </summary>
        [Label("3月")]
        public static readonly Property<decimal> ThreeProperty = P<MttrAndMtbfReportViewModel>.Register(e => e.Three);

        /// <summary>
        /// 3月
        /// </summary>
        public decimal Three
        {
            get { return GetProperty(ThreeProperty); }
            set { SetProperty(ThreeProperty, value); }
        }
        #endregion

        #region 4月 Four
        /// <summary>
        /// 4月
        /// </summary>
        [Label("4月")]
        public static readonly Property<decimal> FourProperty = P<MttrAndMtbfReportViewModel>.Register(e => e.Four);

        /// <summary>
        /// 4月
        /// </summary>
        public decimal Four
        {
            get { return GetProperty(FourProperty); }
            set { SetProperty(FourProperty, value); }
        }
        #endregion

        #region 5月 Five
        /// <summary>
        /// 5月
        /// </summary>
        [Label("5月")]
        public static readonly Property<decimal> FiveProperty = P<MttrAndMtbfReportViewModel>.Register(e => e.Five);

        /// <summary>
        /// 5月
        /// </summary>
        public decimal Five
        {
            get { return GetProperty(FiveProperty); }
            set { SetProperty(FiveProperty, value); }
        }
        #endregion

        #region 6月 Six
        /// <summary>
        /// 6月
        /// </summary>
        [Label("6月")]
        public static readonly Property<decimal> SixProperty = P<MttrAndMtbfReportViewModel>.Register(e => e.Six);

        /// <summary>
        /// 6月
        /// </summary>
        public decimal Six
        {
            get { return GetProperty(SixProperty); }
            set { SetProperty(SixProperty, value); }
        }
        #endregion

        #region 7月 Seven
        /// <summary>
        /// 7月
        /// </summary>
        [Label("7月")]
        public static readonly Property<decimal> SevenProperty = P<MttrAndMtbfReportViewModel>.Register(e => e.Seven);

        /// <summary>
        /// 7月
        /// </summary>
        public decimal Seven
        {
            get { return GetProperty(SevenProperty); }
            set { SetProperty(SevenProperty, value); }
        }
        #endregion

        #region 8月 Eight
        /// <summary>
        /// 8月
        /// </summary>
        [Label("8月")]
        public static readonly Property<decimal> EightProperty = P<MttrAndMtbfReportViewModel>.Register(e => e.Eight);

        /// <summary>
        /// 8月
        /// </summary>
        public decimal Eight
        {
            get { return GetProperty(EightProperty); }
            set { SetProperty(EightProperty, value); }
        }
        #endregion

        #region 9月 Nine
        /// <summary>
        /// 9月
        /// </summary>
        [Label("9月")]
        public static readonly Property<decimal> NineProperty = P<MttrAndMtbfReportViewModel>.Register(e => e.Nine);

        /// <summary>
        /// 9月
        /// </summary>
        public decimal Nine
        {
            get { return GetProperty(NineProperty); }
            set { SetProperty(NineProperty, value); }
        }
        #endregion

        #region 10月 Ten
        /// <summary>
        /// 10月
        /// </summary>
        [Label("10月")]
        public static readonly Property<decimal> TenProperty = P<MttrAndMtbfReportViewModel>.Register(e => e.Ten);

        /// <summary>
        /// 10月
        /// </summary>
        public decimal Ten
        {
            get { return GetProperty(TenProperty); }
            set { SetProperty(TenProperty, value); }
        }
        #endregion

        #region 11月 Eleven
        /// <summary>
        /// 11月
        /// </summary>
        [Label("11月")]
        public static readonly Property<decimal> ElevenProperty = P<MttrAndMtbfReportViewModel>.Register(e => e.Eleven);

        /// <summary>
        /// 11月
        /// </summary>
        public decimal Eleven
        {
            get { return GetProperty(ElevenProperty); }
            set { SetProperty(ElevenProperty, value); }
        }
        #endregion

        #region 12月 Twelve
        /// <summary>
        /// 12月
        /// </summary>
        [Label("12月")]
        public static readonly Property<decimal> TwelveProperty = P<MttrAndMtbfReportViewModel>.Register(e => e.Twelve);

        /// <summary>
        /// 12月
        /// </summary>
        public decimal Twelve
        {
            get { return GetProperty(TwelveProperty); }
            set { SetProperty(TwelveProperty, value); }
        }
        #endregion
    }
}
