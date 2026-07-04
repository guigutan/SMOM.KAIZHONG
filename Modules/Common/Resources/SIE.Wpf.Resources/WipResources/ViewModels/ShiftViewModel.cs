using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Wpf.Resources.WipResources.ViewModels
{
    /// <summary>
    /// 班次信息实体
    /// </summary>
    [RootEntity]
    public class ShiftViewModel : ViewModel
    {
        #region 开始时间 BeginDate
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime> BeginDateProperty = P<ShiftViewModel>.Register(e => e.BeginDate);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate
        {
            get { return this.GetProperty(BeginDateProperty); }
            set { this.SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 结束时间 EndDate
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime> EndDateProperty = P<ShiftViewModel>.Register(e => e.EndDate);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate
        {
            get { return this.GetProperty(EndDateProperty); }
            set { this.SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 时长 WorkHour
        /// <summary>
        /// 时长
        /// </summary>
        [Label("时长(小时)")]
        public static readonly Property<double> WorkHourProperty = P<ShiftViewModel>.Register(e => e.WorkHour);

        /// <summary>
        /// 时长
        /// </summary>
        public double WorkHour
        {
            get { return this.GetProperty(WorkHourProperty); }
            set { this.SetProperty(WorkHourProperty, value); }
        }
        #endregion

        #region 班次名称 ShiftName
        /// <summary>
        /// 班次名称
        /// </summary>
        [Label("班次")]
        public static readonly Property<string> ShiftNameProperty = P<ShiftViewModel>.Register(e => e.ShiftName);

        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName
        {
            get { return this.GetProperty(ShiftNameProperty); }
            set { this.SetProperty(ShiftNameProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 班次信息模型视图配置
    /// </summary>
    internal class ShiftViewModelViewConfig : WPFViewConfig<ShiftViewModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.BeginDate).UseTimeEditor();
            View.Property(p => p.EndDate).UseTimeEditor();
            View.Property(p => p.WorkHour);
            View.Property(p => p.ShiftName);
        }
    }
}