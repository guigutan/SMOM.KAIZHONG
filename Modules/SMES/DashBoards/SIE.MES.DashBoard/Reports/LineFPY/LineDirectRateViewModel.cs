using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.Reports.LineFPY
{
    /// <summary>
    /// 产线报表ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class LineDirectRateViewModel : DirectRateBaseViewModel
    {
        #region 资源Id LineId
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源Id")]
        public static readonly Property<double?> LineIdProperty = P<LineDirectRateViewModel>.Register(e => e.LineId);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? LineId
        {
            get { return this.GetProperty(LineIdProperty); }
            set { this.SetProperty(LineIdProperty, value); }
        }
        #endregion

        #region 资源名称 LineName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        [FieldSetting("资源", FieldArea.RowArea, 1, SummaryType = SummaryType.Custom)]
        public static readonly Property<string> LineNameProperty = P<LineDirectRateViewModel>.Register(e => e.LineName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string LineName
        {
            get { return this.GetProperty(LineNameProperty); }
            set { this.SetProperty(LineNameProperty, value); }
        }
        #endregion

        #region 资源直通率设置 LineDirectRate
        /// <summary>
        /// 资源直通率设置Id
        /// </summary>
        [Label("资源直通率设置")]
        public static readonly IRefIdProperty LineDirectRateIdProperty =
            P<LineDirectRateViewModel>.RegisterRefId(e => e.LineDirectRateId, ReferenceType.Normal);

        /// <summary>
        /// 资源直通率设置Id
        /// </summary>
        public double? LineDirectRateId
        {
            get { return (double?)this.GetRefNullableId(LineDirectRateIdProperty); }
            set { this.SetRefNullableId(LineDirectRateIdProperty, value); }
        }

        /// <summary>
        /// 资源直通率设置
        /// </summary>
        public static readonly RefEntityProperty<LineFpySetting> LineDirectRateProperty =
            P<LineDirectRateViewModel>.RegisterRef(e => e.LineDirectRate, LineDirectRateIdProperty);

        /// <summary>
        /// 资源直通率设置
        /// </summary>
        public LineFpySetting LineDirectRate
        {
            get { return this.GetRefEntity(LineDirectRateProperty); }
            set { this.SetRefEntity(LineDirectRateProperty, value); }
        }
        #endregion

        #region 班次Id ShiftId
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次Id")]
        public static readonly Property<double?> ShiftIdProperty = P<LineDirectRateViewModel>.Register(e => e.ShiftId);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double? ShiftId
        {
            get { return this.GetProperty(ShiftIdProperty); }
            set { this.SetProperty(ShiftIdProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次
        /// </summary>
        [Label("班次")]
        [FieldSetting("班次", FieldArea.RowArea, 1, SummaryType = SummaryType.Custom)]
        public static readonly Property<string> ShiftProperty = P<LineDirectRateViewModel>.Register(e => e.Shift);

        /// <summary>
        /// 班次
        /// </summary>
        public string Shift
        {
            get { return this.GetProperty(ShiftProperty); }
            set { this.SetProperty(ShiftProperty, value); }
        }
        #endregion

        #region 工序Id ProcessId
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序Id")]
        public static readonly Property<double?> ProcessIdProperty = P<LineDirectRateViewModel>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return this.GetProperty(ProcessIdProperty); }
            set { this.SetProperty(ProcessIdProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessProperty = P<LineDirectRateViewModel>.Register(e => e.Process);

        /// <summary>
        /// 工序
        /// </summary>
        public string Process
        {
            get { return this.GetProperty(ProcessProperty); }
            set { this.SetProperty(ProcessProperty, value); }
        }
        #endregion
    }
}
