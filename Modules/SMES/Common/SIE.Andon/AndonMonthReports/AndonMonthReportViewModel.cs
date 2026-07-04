using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.AndonMonthReports
{
    /// <summary>
    /// 安灯月度报表
    /// </summary>

    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AndonMonthViewModelCriteria))]
    [Label("安灯统计报表")]
    public class AndonMonthReportViewModel : ViewModel
    {

        #region 汇总维度 SummaryDimension
        /// <summary>
        /// 汇总维度
        /// </summary>
        [Label("汇总维度")]
        public static readonly Property<string> SummaryDimensionProperty = P<AndonMonthReportViewModel>.Register(e => e.SummaryDimension);

        /// <summary>
        /// 汇总维度
        /// </summary>
        public string SummaryDimension
        {
            get { return this.GetProperty(SummaryDimensionProperty); }
            set { this.SetProperty(SummaryDimensionProperty, value); }
        }
        #endregion


        #region 汇总维度标头 SummaryDimensionTitle
        /// <summary>
        /// 汇总维度标头
        /// </summary>
        [Label("汇总维度标头")]
        public static readonly Property<string> SummaryDimensionTitleProperty = P<AndonMonthReportViewModel>.Register(e => e.SummaryDimensionTitle);

        /// <summary>
        /// 汇总维度标头
        /// </summary>
        public string SummaryDimensionTitle
        {
            get { return this.GetProperty(SummaryDimensionTitleProperty); }
            set { this.SetProperty(SummaryDimensionTitleProperty, value); }
        }
        #endregion

        #region 分组名称标题 GroupNameTitle
        /// <summary>
        /// 分组名称标题
        /// </summary>
        [Label("分组名称标题")]
        public static readonly Property<string> GroupNameTitleProperty = P<AndonMonthReportViewModel>.Register(e => e.GroupNameTitle);

        /// <summary>
        /// 分组名称标题
        /// </summary>
        public string GroupNameTitle
        {
            get { return this.GetProperty(GroupNameTitleProperty); }
            set { this.SetProperty(GroupNameTitleProperty, value); }
        }
        #endregion


        #region 分组名称 GroupName
        /// <summary>
        /// 分组名称
        /// </summary>
        [Label("分组名称")]
        public static readonly Property<string> GroupNameProperty = P<AndonMonthReportViewModel>.Register(e => e.GroupName);

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName
        {
            get { return this.GetProperty(GroupNameProperty); }
            set { this.SetProperty(GroupNameProperty, value); }
        }
        #endregion

        #region 1月份 

        #region 安灯次数 AndonNum
        /// <summary>
        /// 安灯次数
        /// </summary>
        [Label("安灯次数")]
        public static readonly Property<decimal> AndonNum1Property = P<AndonMonthReportViewModel>.Register(e => e.AndonNum1);

        /// <summary>
        /// 安灯次数
        /// </summary>
        public decimal AndonNum1
        {
            get { return this.GetProperty(AndonNum1Property); }
            set { this.SetProperty(AndonNum1Property, value); }
        }
        #endregion

        #region 安灯时长 AndonTime
        /// <summary>
        /// 安灯时长
        /// </summary>
        [Label("安灯时长")]
        public static readonly Property<decimal> AndonTime1Property = P<AndonMonthReportViewModel>.Register(e => e.AndonTime1);

        /// <summary>
        /// 安灯时长
        /// </summary>
        public decimal AndonTime1
        {
            get { return this.GetProperty(AndonTime1Property); }
            set { this.SetProperty(AndonTime1Property, value); }
        }
        #endregion

        #region 停线次数 AndonStopNum
        /// <summary>
        /// 停线次数
        /// </summary>
        [Label("停线次数")]
        public static readonly Property<decimal> AndonStopNum1Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopNum1);

        /// <summary>
        /// 停线次数
        /// </summary>
        public decimal AndonStopNum1
        {
            get { return this.GetProperty(AndonStopNum1Property); }
            set { this.SetProperty(AndonStopNum1Property, value); }
        }
        #endregion

        #region 停线时长 AndonStopLine
        /// <summary>
        /// 停线时长
        /// </summary>
        [Label("停线时长")]
        public static readonly Property<decimal> AndonStopLine1Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopLine1);

        /// <summary>
        /// 停线时长
        /// </summary>
        public decimal AndonStopLine1
        {
            get { return this.GetProperty(AndonStopLine1Property); }
            set { this.SetProperty(AndonStopLine1Property, value); }
        }
        #endregion

        #region 安灯名称变更率 TriggerAccuracy
        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        [Label("安灯名称变更率(%)")]
        public static readonly Property<decimal> TriggerAccuracy1Property = P<AndonMonthReportViewModel>.Register(e => e.TriggerAccuracy1);

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public decimal TriggerAccuracy1
        {
            get { return this.GetProperty(TriggerAccuracy1Property); }
            set { this.SetProperty(TriggerAccuracy1Property, value); }
        }
        #endregion

        #endregion

        #region 2月份 

        #region 安灯次数 AndonNum
        /// <summary>
        /// 安灯次数
        /// </summary>
        [Label("安灯次数")]
        public static readonly Property<decimal> AndonNum2Property = P<AndonMonthReportViewModel>.Register(e => e.AndonNum2);

        /// <summary>
        /// 安灯次数
        /// </summary>
        public decimal AndonNum2
        {
            get { return this.GetProperty(AndonNum2Property); }
            set { this.SetProperty(AndonNum2Property, value); }
        }
        #endregion

        #region 安灯时长 AndonTime
        /// <summary>
        /// 安灯时长
        /// </summary>
        [Label("安灯时长")]
        public static readonly Property<decimal> AndonTime2Property = P<AndonMonthReportViewModel>.Register(e => e.AndonTime2);

        /// <summary>
        /// 安灯时长
        /// </summary>
        public decimal AndonTime2
        {
            get { return this.GetProperty(AndonTime2Property); }
            set { this.SetProperty(AndonTime2Property, value); }
        }
        #endregion

        #region 停线次数 AndonStopNum
        /// <summary>
        /// 停线次数
        /// </summary>
        [Label("停线次数")]
        public static readonly Property<decimal> AndonStopNum2Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopNum2);

        /// <summary>
        /// 停线次数
        /// </summary>
        public decimal AndonStopNum2
        {
            get { return this.GetProperty(AndonStopNum2Property); }
            set { this.SetProperty(AndonStopNum2Property, value); }
        }
        #endregion

        #region 停线时长 AndonStopLine
        /// <summary>
        /// 停线时长
        /// </summary>
        [Label("停线时长")]
        public static readonly Property<decimal> AndonStopLine2Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopLine2);

        /// <summary>
        /// 停线时长
        /// </summary>
        public decimal AndonStopLine2
        {
            get { return this.GetProperty(AndonStopLine2Property); }
            set { this.SetProperty(AndonStopLine2Property, value); }
        }
        #endregion

        #region 安灯名称变更率 TriggerAccuracy
        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        [Label("安灯名称变更率(%)")]
        public static readonly Property<decimal> TriggerAccuracy2Property = P<AndonMonthReportViewModel>.Register(e => e.TriggerAccuracy2);

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public decimal TriggerAccuracy2
        {
            get { return this.GetProperty(TriggerAccuracy2Property); }
            set { this.SetProperty(TriggerAccuracy2Property, value); }
        }
        #endregion

        #endregion

        #region 3月份 

        #region 安灯次数 AndonNum
        /// <summary>
        /// 安灯次数
        /// </summary>
        [Label("安灯次数")]
        public static readonly Property<decimal> AndonNum3Property = P<AndonMonthReportViewModel>.Register(e => e.AndonNum3);

        /// <summary>
        /// 安灯次数
        /// </summary>
        public decimal AndonNum3
        {
            get { return this.GetProperty(AndonNum3Property); }
            set { this.SetProperty(AndonNum3Property, value); }
        }
        #endregion

        #region 安灯时长 AndonTime
        /// <summary>
        /// 安灯时长
        /// </summary>
        [Label("安灯时长")]
        public static readonly Property<decimal> AndonTime3Property = P<AndonMonthReportViewModel>.Register(e => e.AndonTime3);

        /// <summary>
        /// 安灯时长
        /// </summary>
        public decimal AndonTime3
        {
            get { return this.GetProperty(AndonTime3Property); }
            set { this.SetProperty(AndonTime3Property, value); }
        }
        #endregion

        #region 停线次数 AndonStopNum
        /// <summary>
        /// 停线次数
        /// </summary>
        [Label("停线次数")]
        public static readonly Property<decimal> AndonStopNum3Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopNum3);

        /// <summary>
        /// 停线次数
        /// </summary>
        public decimal AndonStopNum3
        {
            get { return this.GetProperty(AndonStopNum3Property); }
            set { this.SetProperty(AndonStopNum3Property, value); }
        }
        #endregion

        #region 停线时长 AndonStopLine
        /// <summary>
        /// 停线时长
        /// </summary>
        [Label("停线时长")]
        public static readonly Property<decimal> AndonStopLine3Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopLine3);

        /// <summary>
        /// 停线时长
        /// </summary>
        public decimal AndonStopLine3
        {
            get { return this.GetProperty(AndonStopLine3Property); }
            set { this.SetProperty(AndonStopLine3Property, value); }
        }
        #endregion

        #region 安灯名称变更率 TriggerAccuracy
        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        [Label("安灯名称变更率(%)")]
        public static readonly Property<decimal> TriggerAccuracy3Property = P<AndonMonthReportViewModel>.Register(e => e.TriggerAccuracy3);

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public decimal TriggerAccuracy3
        {
            get { return this.GetProperty(TriggerAccuracy3Property); }
            set { this.SetProperty(TriggerAccuracy3Property, value); }
        }
        #endregion

        #endregion

        #region 4月份 

        #region 安灯次数 AndonNum
        /// <summary>
        /// 安灯次数
        /// </summary>
        [Label("安灯次数")]
        public static readonly Property<decimal> AndonNum4Property = P<AndonMonthReportViewModel>.Register(e => e.AndonNum4);

        /// <summary>
        /// 安灯次数
        /// </summary>
        public decimal AndonNum4
        {
            get { return this.GetProperty(AndonNum4Property); }
            set { this.SetProperty(AndonNum4Property, value); }
        }
        #endregion

        #region 安灯时长 AndonTime
        /// <summary>
        /// 安灯时长
        /// </summary>
        [Label("安灯时长")]
        public static readonly Property<decimal> AndonTime4Property = P<AndonMonthReportViewModel>.Register(e => e.AndonTime4);

        /// <summary>
        /// 安灯时长
        /// </summary>
        public decimal AndonTime4
        {
            get { return this.GetProperty(AndonTime4Property); }
            set { this.SetProperty(AndonTime4Property, value); }
        }
        #endregion

        #region 停线次数 AndonStopNum
        /// <summary>
        /// 停线次数
        /// </summary>
        [Label("停线次数")]
        public static readonly Property<decimal> AndonStopNum4Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopNum4);

        /// <summary>
        /// 停线次数
        /// </summary>
        public decimal AndonStopNum4
        {
            get { return this.GetProperty(AndonStopNum4Property); }
            set { this.SetProperty(AndonStopNum4Property, value); }
        }
        #endregion

        #region 停线时长 AndonStopLine
        /// <summary>
        /// 停线时长
        /// </summary>
        [Label("停线时长")]
        public static readonly Property<decimal> AndonStopLine4Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopLine4);

        /// <summary>
        /// 停线时长
        /// </summary>
        public decimal AndonStopLine4
        {
            get { return this.GetProperty(AndonStopLine4Property); }
            set { this.SetProperty(AndonStopLine4Property, value); }
        }
        #endregion

        #region 安灯名称变更率 TriggerAccuracy
        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        [Label("安灯名称变更率(%)")]
        public static readonly Property<decimal> TriggerAccuracy4Property = P<AndonMonthReportViewModel>.Register(e => e.TriggerAccuracy4);

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public decimal TriggerAccuracy4
        {
            get { return this.GetProperty(TriggerAccuracy4Property); }
            set { this.SetProperty(TriggerAccuracy4Property, value); }
        }
        #endregion

        #endregion

        #region 5月份 

        #region 安灯次数 AndonNum
        /// <summary>
        /// 安灯次数
        /// </summary>
        [Label("安灯次数")]
        public static readonly Property<decimal> AndonNum5Property = P<AndonMonthReportViewModel>.Register(e => e.AndonNum5);

        /// <summary>
        /// 安灯次数
        /// </summary>
        public decimal AndonNum5
        {
            get { return this.GetProperty(AndonNum5Property); }
            set { this.SetProperty(AndonNum5Property, value); }
        }
        #endregion

        #region 安灯时长 AndonTime
        /// <summary>
        /// 安灯时长
        /// </summary>
        [Label("安灯时长")]
        public static readonly Property<decimal> AndonTime5Property = P<AndonMonthReportViewModel>.Register(e => e.AndonTime5);

        /// <summary>
        /// 安灯时长
        /// </summary>
        public decimal AndonTime5
        {
            get { return this.GetProperty(AndonTime5Property); }
            set { this.SetProperty(AndonTime5Property, value); }
        }
        #endregion

        #region 停线次数 AndonStopNum
        /// <summary>
        /// 停线次数
        /// </summary>
        [Label("停线次数")]
        public static readonly Property<decimal> AndonStopNum5Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopNum5);

        /// <summary>
        /// 停线次数
        /// </summary>
        public decimal AndonStopNum5
        {
            get { return this.GetProperty(AndonStopNum5Property); }
            set { this.SetProperty(AndonStopNum5Property, value); }
        }
        #endregion

        #region 停线时长 AndonStopLine
        /// <summary>
        /// 停线时长
        /// </summary>
        [Label("停线时长")]
        public static readonly Property<decimal> AndonStopLine5Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopLine5);

        /// <summary>
        /// 停线时长
        /// </summary>
        public decimal AndonStopLine5
        {
            get { return this.GetProperty(AndonStopLine5Property); }
            set { this.SetProperty(AndonStopLine5Property, value); }
        }
        #endregion

        #region 安灯名称变更率 TriggerAccuracy
        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        [Label("安灯名称变更率(%)")]
        public static readonly Property<decimal> TriggerAccuracy5Property = P<AndonMonthReportViewModel>.Register(e => e.TriggerAccuracy5);

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public decimal TriggerAccuracy5
        {
            get { return this.GetProperty(TriggerAccuracy5Property); }
            set { this.SetProperty(TriggerAccuracy5Property, value); }
        }
        #endregion

        #endregion

        #region 6月份 

        #region 安灯次数 AndonNum
        /// <summary>
        /// 安灯次数
        /// </summary>
        [Label("安灯次数")]
        public static readonly Property<decimal> AndonNum6Property = P<AndonMonthReportViewModel>.Register(e => e.AndonNum6);

        /// <summary>
        /// 安灯次数
        /// </summary>
        public decimal AndonNum6
        {
            get { return this.GetProperty(AndonNum6Property); }
            set { this.SetProperty(AndonNum6Property, value); }
        }
        #endregion

        #region 安灯时长 AndonTime
        /// <summary>
        /// 安灯时长
        /// </summary>
        [Label("安灯时长")]
        public static readonly Property<decimal> AndonTime6Property = P<AndonMonthReportViewModel>.Register(e => e.AndonTime6);

        /// <summary>
        /// 安灯时长
        /// </summary>
        public decimal AndonTime6
        {
            get { return this.GetProperty(AndonTime6Property); }
            set { this.SetProperty(AndonTime6Property, value); }
        }
        #endregion

        #region 停线次数 AndonStopNum
        /// <summary>
        /// 停线次数
        /// </summary>
        [Label("停线次数")]
        public static readonly Property<decimal> AndonStopNum6Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopNum6);

        /// <summary>
        /// 停线次数
        /// </summary>
        public decimal AndonStopNum6
        {
            get { return this.GetProperty(AndonStopNum6Property); }
            set { this.SetProperty(AndonStopNum6Property, value); }
        }
        #endregion

        #region 停线时长 AndonStopLine
        /// <summary>
        /// 停线时长
        /// </summary>
        [Label("停线时长")]
        public static readonly Property<decimal> AndonStopLine6Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopLine6);

        /// <summary>
        /// 停线时长
        /// </summary>
        public decimal AndonStopLine6
        {
            get { return this.GetProperty(AndonStopLine6Property); }
            set { this.SetProperty(AndonStopLine6Property, value); }
        }
        #endregion

        #region 安灯名称变更率 TriggerAccuracy
        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        [Label("安灯名称变更率(%)")]
        public static readonly Property<decimal> TriggerAccuracy6Property = P<AndonMonthReportViewModel>.Register(e => e.TriggerAccuracy6);

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public decimal TriggerAccuracy6
        {
            get { return this.GetProperty(TriggerAccuracy6Property); }
            set { this.SetProperty(TriggerAccuracy6Property, value); }
        }
        #endregion

        #endregion

        #region 7月份 

        #region 安灯次数 AndonNum
        /// <summary>
        /// 安灯次数
        /// </summary>
        [Label("安灯次数")]
        public static readonly Property<decimal> AndonNum7Property = P<AndonMonthReportViewModel>.Register(e => e.AndonNum7);

        /// <summary>
        /// 安灯次数
        /// </summary>
        public decimal AndonNum7
        {
            get { return this.GetProperty(AndonNum7Property); }
            set { this.SetProperty(AndonNum7Property, value); }
        }
        #endregion

        #region 安灯时长 AndonTime
        /// <summary>
        /// 安灯时长
        /// </summary>
        [Label("安灯时长")]
        public static readonly Property<decimal> AndonTime7Property = P<AndonMonthReportViewModel>.Register(e => e.AndonTime7);

        /// <summary>
        /// 安灯时长
        /// </summary>
        public decimal AndonTime7
        {
            get { return this.GetProperty(AndonTime7Property); }
            set { this.SetProperty(AndonTime7Property, value); }
        }
        #endregion

        #region 停线次数 AndonStopNum
        /// <summary>
        /// 停线次数
        /// </summary>
        [Label("停线次数")]
        public static readonly Property<decimal> AndonStopNum7Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopNum7);

        /// <summary>
        /// 停线次数
        /// </summary>
        public decimal AndonStopNum7
        {
            get { return this.GetProperty(AndonStopNum7Property); }
            set { this.SetProperty(AndonStopNum7Property, value); }
        }
        #endregion

        #region 停线时长 AndonStopLine
        /// <summary>
        /// 停线时长
        /// </summary>
        [Label("停线时长")]
        public static readonly Property<decimal> AndonStopLine7Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopLine7);

        /// <summary>
        /// 停线时长
        /// </summary>
        public decimal AndonStopLine7
        {
            get { return this.GetProperty(AndonStopLine7Property); }
            set { this.SetProperty(AndonStopLine7Property, value); }
        }
        #endregion

        #region 安灯名称变更率 TriggerAccuracy
        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        [Label("安灯名称变更率(%)")]
        public static readonly Property<decimal> TriggerAccuracy7Property = P<AndonMonthReportViewModel>.Register(e => e.TriggerAccuracy7);

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public decimal TriggerAccuracy7
        {
            get { return this.GetProperty(TriggerAccuracy7Property); }
            set { this.SetProperty(TriggerAccuracy7Property, value); }
        }
        #endregion

        #endregion

        #region 8月份 

        #region 安灯次数 AndonNum
        /// <summary>
        /// 安灯次数
        /// </summary>
        [Label("安灯次数")]
        public static readonly Property<decimal> AndonNum8Property = P<AndonMonthReportViewModel>.Register(e => e.AndonNum8);

        /// <summary>
        /// 安灯次数
        /// </summary>
        public decimal AndonNum8
        {
            get { return this.GetProperty(AndonNum8Property); }
            set { this.SetProperty(AndonNum8Property, value); }
        }
        #endregion

        #region 安灯时长 AndonTime
        /// <summary>
        /// 安灯时长
        /// </summary>
        [Label("安灯时长")]
        public static readonly Property<decimal> AndonTime8Property = P<AndonMonthReportViewModel>.Register(e => e.AndonTime8);

        /// <summary>
        /// 安灯时长
        /// </summary>
        public decimal AndonTime8
        {
            get { return this.GetProperty(AndonTime8Property); }
            set { this.SetProperty(AndonTime8Property, value); }
        }
        #endregion

        #region 停线次数 AndonStopNum
        /// <summary>
        /// 停线次数
        /// </summary>
        [Label("停线次数")]
        public static readonly Property<decimal> AndonStopNum8Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopNum8);

        /// <summary>
        /// 停线次数
        /// </summary>
        public decimal AndonStopNum8
        {
            get { return this.GetProperty(AndonStopNum8Property); }
            set { this.SetProperty(AndonStopNum8Property, value); }
        }
        #endregion

        #region 停线时长 AndonStopLine
        /// <summary>
        /// 停线时长
        /// </summary>
        [Label("停线时长")]
        public static readonly Property<decimal> AndonStopLine8Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopLine8);

        /// <summary>
        /// 停线时长
        /// </summary>
        public decimal AndonStopLine8
        {
            get { return this.GetProperty(AndonStopLine8Property); }
            set { this.SetProperty(AndonStopLine8Property, value); }
        }
        #endregion

        #region 安灯名称变更率 TriggerAccuracy
        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        [Label("安灯名称变更率(%)")]
        public static readonly Property<decimal> TriggerAccuracy8Property = P<AndonMonthReportViewModel>.Register(e => e.TriggerAccuracy8);

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public decimal TriggerAccuracy8
        {
            get { return this.GetProperty(TriggerAccuracy8Property); }
            set { this.SetProperty(TriggerAccuracy8Property, value); }
        }
        #endregion

        #endregion

        #region 9月份 

        #region 安灯次数 AndonNum
        /// <summary>
        /// 安灯次数
        /// </summary>
        [Label("安灯次数")]
        public static readonly Property<decimal> AndonNum9Property = P<AndonMonthReportViewModel>.Register(e => e.AndonNum9);

        /// <summary>
        /// 安灯次数
        /// </summary>
        public decimal AndonNum9
        {
            get { return this.GetProperty(AndonNum9Property); }
            set { this.SetProperty(AndonNum9Property, value); }
        }
        #endregion

        #region 安灯时长 AndonTime
        /// <summary>
        /// 安灯时长
        /// </summary>
        [Label("安灯时长")]
        public static readonly Property<decimal> AndonTime9Property = P<AndonMonthReportViewModel>.Register(e => e.AndonTime9);

        /// <summary>
        /// 安灯时长
        /// </summary>
        public decimal AndonTime9
        {
            get { return this.GetProperty(AndonTime9Property); }
            set { this.SetProperty(AndonTime9Property, value); }
        }
        #endregion

        #region 停线次数 AndonStopNum
        /// <summary>
        /// 停线次数
        /// </summary>
        [Label("停线次数")]
        public static readonly Property<decimal> AndonStopNum9Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopNum9);

        /// <summary>
        /// 停线次数
        /// </summary>
        public decimal AndonStopNum9
        {
            get { return this.GetProperty(AndonStopNum9Property); }
            set { this.SetProperty(AndonStopNum9Property, value); }
        }
        #endregion

        #region 停线时长 AndonStopLine
        /// <summary>
        /// 停线时长
        /// </summary>
        [Label("停线时长")]
        public static readonly Property<decimal> AndonStopLine9Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopLine9);

        /// <summary>
        /// 停线时长
        /// </summary>
        public decimal AndonStopLine9
        {
            get { return this.GetProperty(AndonStopLine9Property); }
            set { this.SetProperty(AndonStopLine9Property, value); }
        }
        #endregion

        #region 安灯名称变更率 TriggerAccuracy
        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        [Label("安灯名称变更率(%)")]
        public static readonly Property<decimal> TriggerAccuracy9Property = P<AndonMonthReportViewModel>.Register(e => e.TriggerAccuracy9);

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public decimal TriggerAccuracy9
        {
            get { return this.GetProperty(TriggerAccuracy9Property); }
            set { this.SetProperty(TriggerAccuracy9Property, value); }
        }
        #endregion

        #endregion

        #region 10月份 

        #region 安灯次数 AndonNum
        /// <summary>
        /// 安灯次数
        /// </summary>
        [Label("安灯次数")]
        public static readonly Property<decimal> AndonNum10Property = P<AndonMonthReportViewModel>.Register(e => e.AndonNum10);

        /// <summary>
        /// 安灯次数
        /// </summary>
        public decimal AndonNum10
        {
            get { return this.GetProperty(AndonNum10Property); }
            set { this.SetProperty(AndonNum10Property, value); }
        }
        #endregion

        #region 安灯时长 AndonTime
        /// <summary>
        /// 安灯时长
        /// </summary>
        [Label("安灯时长")]
        public static readonly Property<decimal> AndonTime10Property = P<AndonMonthReportViewModel>.Register(e => e.AndonTime10);

        /// <summary>
        /// 安灯时长
        /// </summary>
        public decimal AndonTime10
        {
            get { return this.GetProperty(AndonTime10Property); }
            set { this.SetProperty(AndonTime10Property, value); }
        }
        #endregion

        #region 停线次数 AndonStopNum
        /// <summary>
        /// 停线次数
        /// </summary>
        [Label("停线次数")]
        public static readonly Property<decimal> AndonStopNum10Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopNum10);

        /// <summary>
        /// 停线次数
        /// </summary>
        public decimal AndonStopNum10
        {
            get { return this.GetProperty(AndonStopNum10Property); }
            set { this.SetProperty(AndonStopNum10Property, value); }
        }
        #endregion

        #region 停线时长 AndonStopLine
        /// <summary>
        /// 停线时长
        /// </summary>
        [Label("停线时长")]
        public static readonly Property<decimal> AndonStopLine10Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopLine10);

        /// <summary>
        /// 停线时长
        /// </summary>
        public decimal AndonStopLine10
        {
            get { return this.GetProperty(AndonStopLine10Property); }
            set { this.SetProperty(AndonStopLine10Property, value); }
        }
        #endregion

        #region 安灯名称变更率 TriggerAccuracy
        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        [Label("安灯名称变更率(%)")]
        public static readonly Property<decimal> TriggerAccuracy10Property = P<AndonMonthReportViewModel>.Register(e => e.TriggerAccuracy10);

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public decimal TriggerAccuracy10
        {
            get { return this.GetProperty(TriggerAccuracy10Property); }
            set { this.SetProperty(TriggerAccuracy10Property, value); }
        }
        #endregion

        #endregion

        #region 11月份 

        #region 安灯次数 AndonNum
        /// <summary>
        /// 安灯次数
        /// </summary>
        [Label("安灯次数")]
        public static readonly Property<decimal> AndonNum11Property = P<AndonMonthReportViewModel>.Register(e => e.AndonNum11);

        /// <summary>
        /// 安灯次数
        /// </summary>
        public decimal AndonNum11
        {
            get { return this.GetProperty(AndonNum11Property); }
            set { this.SetProperty(AndonNum11Property, value); }
        }
        #endregion

        #region 安灯时长 AndonTime
        /// <summary>
        /// 安灯时长
        /// </summary>
        [Label("安灯时长")]
        public static readonly Property<decimal> AndonTime11Property = P<AndonMonthReportViewModel>.Register(e => e.AndonTime11);

        /// <summary>
        /// 安灯时长
        /// </summary>
        public decimal AndonTime11
        {
            get { return this.GetProperty(AndonTime11Property); }
            set { this.SetProperty(AndonTime11Property, value); }
        }
        #endregion

        #region 停线次数 AndonStopNum
        /// <summary>
        /// 停线次数
        /// </summary>
        [Label("停线次数")]
        public static readonly Property<decimal> AndonStopNum11Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopNum11);

        /// <summary>
        /// 停线次数
        /// </summary>
        public decimal AndonStopNum11
        {
            get { return this.GetProperty(AndonStopNum11Property); }
            set { this.SetProperty(AndonStopNum11Property, value); }
        }
        #endregion

        #region 停线时长 AndonStopLine
        /// <summary>
        /// 停线时长
        /// </summary>
        [Label("停线时长")]
        public static readonly Property<decimal> AndonStopLine11Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopLine11);

        /// <summary>
        /// 停线时长
        /// </summary>
        public decimal AndonStopLine11
        {
            get { return this.GetProperty(AndonStopLine11Property); }
            set { this.SetProperty(AndonStopLine11Property, value); }
        }
        #endregion

        #region 安灯名称变更率 TriggerAccuracy
        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        [Label("安灯名称变更率(%)")]
        public static readonly Property<decimal> TriggerAccuracy11Property = P<AndonMonthReportViewModel>.Register(e => e.TriggerAccuracy11);

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public decimal TriggerAccuracy11
        {
            get { return this.GetProperty(TriggerAccuracy11Property); }
            set { this.SetProperty(TriggerAccuracy11Property, value); }
        }
        #endregion

        #endregion

        #region 12月份 

        #region 安灯次数 AndonNum
        /// <summary>
        /// 安灯次数
        /// </summary>
        [Label("安灯次数")]
        public static readonly Property<decimal> AndonNum12Property = P<AndonMonthReportViewModel>.Register(e => e.AndonNum12);

        /// <summary>
        /// 安灯次数
        /// </summary>
        public decimal AndonNum12
        {
            get { return this.GetProperty(AndonNum12Property); }
            set { this.SetProperty(AndonNum12Property, value); }
        }
        #endregion

        #region 安灯时长 AndonTime
        /// <summary>
        /// 安灯时长
        /// </summary>
        [Label("安灯时长")]
        public static readonly Property<decimal> AndonTime12Property = P<AndonMonthReportViewModel>.Register(e => e.AndonTime12);

        /// <summary>
        /// 安灯时长
        /// </summary>
        public decimal AndonTime12
        {
            get { return this.GetProperty(AndonTime12Property); }
            set { this.SetProperty(AndonTime12Property, value); }
        }
        #endregion

        #region 停线次数 AndonStopNum
        /// <summary>
        /// 停线次数
        /// </summary>
        [Label("停线次数")]
        public static readonly Property<decimal> AndonStopNum12Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopNum12);

        /// <summary>
        /// 停线次数
        /// </summary>
        public decimal AndonStopNum12
        {
            get { return this.GetProperty(AndonStopNum12Property); }
            set { this.SetProperty(AndonStopNum12Property, value); }
        }
        #endregion

        #region 停线时长 AndonStopLine
        /// <summary>
        /// 停线时长
        /// </summary>
        [Label("停线时长")]
        public static readonly Property<decimal> AndonStopLine12Property = P<AndonMonthReportViewModel>.Register(e => e.AndonStopLine12);

        /// <summary>
        /// 停线时长
        /// </summary>
        public decimal AndonStopLine12
        {
            get { return this.GetProperty(AndonStopLine12Property); }
            set { this.SetProperty(AndonStopLine12Property, value); }
        }
        #endregion

        #region 安灯名称变更率 TriggerAccuracy
        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        [Label("安灯名称变更率(%)")]
        public static readonly Property<decimal> TriggerAccuracy12Property = P<AndonMonthReportViewModel>.Register(e => e.TriggerAccuracy12);

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public decimal TriggerAccuracy12
        {
            get { return this.GetProperty(TriggerAccuracy12Property); }
            set { this.SetProperty(TriggerAccuracy12Property, value); }
        }
        #endregion

        #endregion
    }
}
