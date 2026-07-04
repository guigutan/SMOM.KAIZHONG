
using System;

namespace SIE.WorkBenchChartBase.Commons
{
    /// <summary>
    /// 图形接口
    /// </summary>
    public interface IChart :IDisposable
    {

        /// <summary>
        /// 设置时间间隔
        /// </summary>
        /// <param name="timeSpan">时间间隔</param>
        void SetTimerInterval(double timeSpan);
    }
}
