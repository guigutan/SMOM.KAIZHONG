using SIE.Services;
using System.Collections.Generic;

namespace SIE.EventMessages.WMS.Shipment
{
    /// <summary>
    /// MES需求取消WMS发运单接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultIIsStartLes))]
    public interface IIsStartLes
    {
        /// <summary>
        /// 启用LES
        /// </summary>
        /// <returns></returns>
        bool IsStartLes();
    }

    /// <summary>
    /// MES需求取消WMS发运单接口默认实现
    /// </summary>
    public class DefaultIIsStartLes : IIsStartLes
    {
        /// <summary>
        /// 启用LES
        /// </summary>
        /// <returns></returns>
        public bool IsStartLes()
        {
            return false;
        }
    }
}
