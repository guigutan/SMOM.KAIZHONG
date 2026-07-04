using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.WMS.Inv
{
    /// <summary>
    /// 库存调整接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultIInvCountInterface))]
    public interface IInvCount
    {
        /// <summary>
        /// 获取调整物料
        /// </summary>
        /// <returns></returns>
        List<double> GetCountItem(double whId);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefaultIInvCountInterface : IInvCount
    {
        /// <summary>
        /// 获取调整物料
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<double> GetCountItem(double whId)
        {
            return new List<double>();
        }
    }
}
