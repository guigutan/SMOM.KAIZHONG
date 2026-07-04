using SIE.Domain;
using SIE.EventMessages.Common.Traces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.QMS.Traces
{
    /// <summary>
    /// 来料检验追溯信息查询实体
    /// </summary>
    [Serializable]
    public class TraceInfoForIqcCriteria
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        public PagingInfo PagingInfo { get; set; }

        /// <summary>
        /// Asn明细Id
        /// </summary>
        public List<double> AsnDetailIds { get; set; }
    }
}
