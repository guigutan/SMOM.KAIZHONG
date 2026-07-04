using SIE.EventMessages.WMS.Traces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.QMS.Traces
{
    /// <summary>
    /// 品质追溯信息接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultTrace))]
    public interface ITrace
    {
        /// <summary>
        /// 品质追溯-来料检验信息
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        TraceInfoForQms GetTraceInfoForIqc(TraceInfoForIqcCriteria criteria);

        /// <summary>
        /// 产品品质追溯-产品检验信息
        /// </summary>
        /// <param name = "criteria" > 查询条件 </param >
        /// <returns></returns>
        TraceInfoForQms GetTraceInfoForQms(TraceInfoForProductQmsCriteria criteria);

    }

    /// <summary>
    /// 品质追溯信息接口的默认实现
    /// </summary>
    class DefaultTrace : ITrace
    {
        public TraceInfoForQms GetTraceInfoForIqc(TraceInfoForIqcCriteria criteria)
        {
            return new TraceInfoForQms();
        }

        public TraceInfoForQms GetTraceInfoForQms(TraceInfoForProductQmsCriteria criteria)
        {
            return new TraceInfoForQms();
        }
    }
}
