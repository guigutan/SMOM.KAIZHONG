using SIE.Common.Schdules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Job
{
    /// <summary>
    /// 接口日志删除调度
    /// </summary>
    [RootEntity, Serializable]
    public class InterfaceLogParameter : JobParameter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InterfaceLogParameter()
        {
            Day = 0;
        }

        #region 提前天数 Day
        /// <summary>
        /// 提前天数
        /// </summary>
        [Label("提前天数")]
        public static readonly Property<int> DayProperty = P<InterfaceLogParameter>.Register(e => e.Day);

        /// <summary>
        /// 提前天数
        /// </summary>
        public int Day
        {
            get { return this.GetProperty(DayProperty); }
            set { this.SetProperty(DayProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// Web接口日志删除参数视图配置
    /// </summary>
    class InterfaceLogParameterWebViewConfig : WebViewConfig<InterfaceLogParameter>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Day).Show();
        }
    }

    /// <summary>
    /// WPF接口日志删除参数视图
    /// </summary>
    class InterfaceLogParameterWPFViewConfig : WPFViewConfig<InterfaceLogParameter>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Day).Show();
        }
    }
}
