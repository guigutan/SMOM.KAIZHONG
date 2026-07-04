using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.ViewModels
{
    /// <summary>
    /// 选择预约月台ViewModel
    /// </summary>
    [Serializable, RootEntity]
    [Label("选择预约月台")]
    [DisplayMember(nameof(AppointTimeDisplay))]
    public class SelectAppointDockViewModel : ViewModel
    {
        #region 预约开始时间 StartDate
        /// <summary>
        /// 预约开始时间
        /// </summary>
        [Label("预约开始时间")]
        public static readonly Property<DateTime> StartDateProperty = P<SelectAppointDockViewModel>.Register(e => e.StartDate);

        /// <summary>
        /// 预约开始时间
        /// </summary>
        public DateTime StartDate
        {
            get { return this.GetProperty(StartDateProperty); }
            set { this.SetProperty(StartDateProperty, value); }
        }
        #endregion

        #region 预约结束时间 EndDate
        /// <summary>
        /// 预约结束时间
        /// </summary>
        [Label("预约结束时间")]
        public static readonly Property<DateTime> EndDateProperty = P<SelectAppointDockViewModel>.Register(e => e.EndDate);

        /// <summary>
        /// 预约结束时间
        /// </summary>
        public DateTime EndDate
        {
            get { return this.GetProperty(EndDateProperty); }
            set { this.SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 预约时段显示 AppointTimeDisplay
        /// <summary>
        /// 预约时段显示
        /// </summary>
        [Label("预约时段")]
        public static readonly Property<string> AppointTimeDisplayProperty = P<SelectAppointDockViewModel>.Register(e => e.AppointTimeDisplay);

        /// <summary>
        /// 预约时段显示
        /// </summary>
        public string AppointTimeDisplay
        {
            get { return this.GetProperty(AppointTimeDisplayProperty); }
            set { this.SetProperty(AppointTimeDisplayProperty, value); }
        }
        #endregion

        #region 预计占用时间 AppointUseTime
        /// <summary>
        /// 预计占用时间
        /// </summary>
        [Label("预计占用时间")]
        public static readonly Property<double> AppointUseTimeProperty = P<SelectAppointDockViewModel>.Register(e => e.AppointUseTime);

        /// <summary>
        /// 预计占用时间
        /// </summary>
        public double AppointUseTime
        {
            get { return this.GetProperty(AppointUseTimeProperty); }
            set { this.SetProperty(AppointUseTimeProperty, value); }
        }
        #endregion

        #region 月台剩余最大时间 MaxRestTime
        /// <summary>
        /// 预计占用时间
        /// </summary>
        [Label("预计占用时间")]
        public static readonly Property<double> MaxRestTimeProperty = P<SelectAppointDockViewModel>.Register(e => e.MaxRestTime);

        /// <summary>
        /// 预计占用时间
        /// </summary>
        public double MaxRestTime
        {
            get { return this.GetProperty(MaxRestTimeProperty); }
            set { this.SetProperty(MaxRestTimeProperty, value); }
        }
        #endregion
    }
}