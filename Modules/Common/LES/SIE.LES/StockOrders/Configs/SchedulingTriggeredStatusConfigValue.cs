using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.LES.StockOrders.Configs
{
    /// <summary>
    /// 调度触发的备料单状态
    /// </summary>
    [RootEntity, Serializable]
    [Label("调度触发的备料单状态")]
    public class SchedulingTriggeredStatusConfigValue : ConfigValue
    {

        #region 调度触发的备料单状态 TriggeredStatus
        /// <summary>
        /// 调度触发的备料单状态
        /// </summary>
        [Label("调度触发的备料单状态")]
        public static readonly Property<TriggeredStatus> TriggeredStatusProperty = P<SchedulingTriggeredStatusConfigValue>.Register(e => e.TriggeredStatus);

        /// <summary>
        /// 调度触发的备料单状态
        /// </summary>
        public TriggeredStatus TriggeredStatus
        {
            get { return this.GetProperty(TriggeredStatusProperty); }
            set { this.SetProperty(TriggeredStatusProperty, value); }
        }
        #endregion


        /// <summary>
        /// 显示值
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return TriggeredStatus.ToLabel().L10N();
        }
    }
    /// <summary>
    /// 调度触发的备料单状态
    /// </summary>
    public enum TriggeredStatus
    {
        /// <summary>
        /// 已提交
        /// </summary>
       [Label("已提交")]
        Submitted,

        /// <summary>
        /// 待提交
        /// </summary>
        [Label("待提交")]
        Created
    }
}
