using SIE.MES.WIP;
using SIE.Threading;
using System;
using System.Threading.Tasks;

namespace SIE.MES.Statistics.WIP
{
    /// <summary>
    /// 采集统计监听器
    /// </summary>
    public class WipStatisticsListenter
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static WipStatisticsListenter Instance { get; } = new WipStatisticsListenter();

        /// <summary>
        /// 启动监听
        /// </summary>
        public void Start()
        {
            RT.EventBus.Subscribe<WipCollectedEvent>(this, e =>
            {
                //以下代码执行在另一线程中。
                Task.Run(new Action(() =>
                {
                    WipCollectedManager.Instance.AddWipCollectedData(e);
                }).WithCurrentThreadContext());
            });
        }
    }
}