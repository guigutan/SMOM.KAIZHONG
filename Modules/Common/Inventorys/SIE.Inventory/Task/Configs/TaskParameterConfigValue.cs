using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Task.Configs
{
    /// <summary>
    /// 任务管理配置项内容
    /// </summary>
    [RootEntity, Serializable]
    public class TaskParameterConfigValue : ConfigValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskParameterConfigValue()
        {
            ExecuteTimeout = 60;
            NotGetTimeout = 60;
            UntreatedTimeout = 60;
            UrgentMaxCount = 5;
        }

        #region 执行超时标准(Min) ExecuteTimeout
        /// <summary>
        /// 执行超时标准(Min)
        /// </summary>
        [Label("执行超时标准(Min)")]
        [MinValue(1)]
        public static readonly Property<decimal> ExecuteTimeoutProperty = P<TaskParameterConfigValue>.Register(e => e.ExecuteTimeout);

        /// <summary>
        /// 执行超时标准(Min)
        /// </summary>
        public decimal ExecuteTimeout
        {
            get { return this.GetProperty(ExecuteTimeoutProperty); }
            set { this.SetProperty(ExecuteTimeoutProperty, value); }
        }
        #endregion

        #region 超时未领取（Min） NotGetTimeout
        /// <summary>
        /// 超时未领取（Min）
        /// </summary>
        [Label("超时未领取(Min)")]
        [MinValue(1)]
        public static readonly Property<decimal> NotGetTimeoutProperty = P<TaskParameterConfigValue>.Register(e => e.NotGetTimeout);

        /// <summary>
        /// 超时未领取（Min）
        /// </summary>
        public decimal NotGetTimeout
        {
            get { return this.GetProperty(NotGetTimeoutProperty); }
            set { this.SetProperty(NotGetTimeoutProperty, value); }
        }
        #endregion

        #region 超时未处理（Min） UntreatedTimeout
        /// <summary>
        /// 超时未处理（Min）
        /// </summary>
        [Label("超时未处理(Min)")]
        [MinValue(1)]
        public static readonly Property<decimal> UntreatedTimeoutProperty = P<TaskParameterConfigValue>.Register(e => e.UntreatedTimeout);

        /// <summary>
        /// 超时未处理（Min）
        /// </summary>
        public decimal UntreatedTimeout
        {
            get { return this.GetProperty(UntreatedTimeoutProperty); }
            set { this.SetProperty(UntreatedTimeoutProperty, value); }
        }
        #endregion

        #region 刷新周期(Min) RefreshCycle
        /// <summary>
        /// 刷新周期(Min)
        /// </summary>
        [Label("刷新周期(Min)")]
        [MinValue(1)]
        public static readonly Property<decimal> RefreshCycleProperty = P<TaskParameterConfigValue>.Register(e => e.RefreshCycle);

        /// <summary>
        /// 刷新周期(Min)
        /// </summary>
        public decimal RefreshCycle
        {
            get { return this.GetProperty(RefreshCycleProperty); }
            set { this.SetProperty(RefreshCycleProperty, value); }
        }
        #endregion

        #region 加急任务上限 UrgentMaxCount
        /// <summary>
        /// 加急任务上限
        /// </summary>
        [Label("加急任务上限")]
        [MinValue(0)]
        public static readonly Property<int> UrgentMaxCountProperty = P<TaskParameterConfigValue>.Register(e => e.UrgentMaxCount);

        /// <summary>
        /// 加急任务上限
        /// </summary>
        public int UrgentMaxCount
        {
            get { return this.GetProperty(UrgentMaxCountProperty); }
            set { this.SetProperty(UrgentMaxCountProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return "执行超时 {0}; 未领取 {1}; 未处理 {2};  加急个数 {3};"
               .L10nFormat(ExecuteTimeout, NotGetTimeout, UntreatedTimeout, UrgentMaxCount);
        }
    }
}
