using SIE.ControlChart.SpcUtils;
using SIE.Core.Common.Service;
using System;

namespace SIE.ControlChart.ChartDatas
{
    /// <summary>
    /// 控制图参数初始化
    /// </summary>
    [Services.Service(FallbackType = typeof(ChartCalConstInitializer))]
    public interface IChartCalConstInitializer
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        ChartCalConst Initialize();
    }


    /// <summary>
    /// 接口的默认实现
    /// </summary>
    class ChartCalConstInitializer : DomainService, IChartCalConstInitializer
    {
        /// <summary>
        /// 默认实现
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ChartCalConst Initialize()
        {
            return new ChartCalConst();
        }
    }

}
