namespace SIE.WorkBenchChartBase
{
    /// <summary>
    /// 车间变更事件
    /// 组件通过监听改事件做数据切换刷新
    /// </summary>
    public class WorkShopChangedEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="workShopId">车间ID</param>
        public WorkShopChangedEvent(double workShopId)
        {
            WorkShopId = workShopId;
        }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double WorkShopId { get; set; }
    }
}