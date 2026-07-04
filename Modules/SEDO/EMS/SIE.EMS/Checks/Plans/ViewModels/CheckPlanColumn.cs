using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Checks.Plans.ViewModels
{
    /// <summary>
    /// 点检计划主表列信息
    /// </summary>
    [RootEntity, Serializable]
    public class CheckPlanColumn : ViewModel
    {
        #region 日期号 DayNum
        /// <summary>
        /// 日期号
        /// </summary>
        [Label("日期号")]
        public static readonly Property<int> DayNumProperty = P<CheckPlanColumn>.Register(e => e.DayNum);

        /// <summary>
        /// 日期号
        /// </summary>
        public int DayNum
        {
            get { return this.GetProperty(DayNumProperty); }
            set { this.SetProperty(DayNumProperty, value); }
        }
        #endregion

        #region 开始时间 BeginTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<string> BeginTimeProperty = P<CheckPlanColumn>.Register(e => e.BeginTime);

        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime
        {
            get { return this.GetProperty(BeginTimeProperty); }
            set { this.SetProperty(BeginTimeProperty, value); }
        }
        #endregion

        #region 结束时间 EndTime
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<string> EndTimeProperty = P<CheckPlanColumn>.Register(e => e.EndTime);

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime
        {
            get { return this.GetProperty(EndTimeProperty); }
            set { this.SetProperty(EndTimeProperty, value); }
        }
        #endregion

        #region 班次名称 ShiftName
        /// <summary>
        /// 班次名称
        /// </summary>
        [Label("班次名称")]
        public static readonly Property<string> ShiftNameProperty = P<CheckPlanColumn>.Register(e => e.ShiftName);

        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName
        {
            get { return this.GetProperty(ShiftNameProperty); }
            set { this.SetProperty(ShiftNameProperty, value); }
        }
        #endregion

        #region 列名 ColumnName
        /// <summary>
        /// 列名
        /// </summary>
        [Label("列名")]
        public static readonly Property<string> ColumnNameProperty = P<CheckPlanColumn>.Register(e => e.ColumnName);

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName
        {
            get { return this.GetProperty(ColumnNameProperty); }
            set { this.SetProperty(ColumnNameProperty, value); }
        }
        #endregion

        #region 执行状态 ExeState
        /// <summary>
        /// 执行状态
        /// </summary>
        [Label("执行状态")]
        public static readonly Property<int?> ExeStateProperty = P<CheckPlanColumn>.Register(e => e.ExeState);

        /// <summary>
        /// 执行状态
        /// </summary>
        public int? ExeState
        {
            get { return this.GetProperty(ExeStateProperty); }
            set { this.SetProperty(ExeStateProperty, value); }
        }
        #endregion

        #region 执行结果 ExeResult
        /// <summary>
        /// 执行结果
        /// </summary>
        [Label("执行结果")]
        public static readonly Property<string> ExeResultProperty = P<CheckPlanColumn>.Register(e => e.ExeResult);

        /// <summary>
        /// 执行结果
        /// </summary>
        public string ExeResult
        {
            get { return this.GetProperty(ExeResultProperty); }
            set { this.SetProperty(ExeResultProperty, value); }
        }
        #endregion
    }
}
