using SIE.EventMessages.Common.Traces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Traces
{
    /// <summary>
    /// 产品检验记录追溯查询实体
    /// </summary>
    [Serializable]
    public class TraceInfoForProductInspectCriteria
    {
        /// <summary>
        /// 生产通用报表Id
        /// </summary>
        public double WipProductVersionId { get; set; }

        /// <summary>
        /// 分页信息
        /// </summary>
        public PagingInfo PagingInfo { get; set; }
    }
}
