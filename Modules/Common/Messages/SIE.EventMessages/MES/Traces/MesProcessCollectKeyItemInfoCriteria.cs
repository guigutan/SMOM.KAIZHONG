using SIE.Domain;
using SIE.EventMessages.Common.Traces;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Traces
{
    /// <summary>
    /// 反向追溯-Mes关键件采集信息查询实体
    /// </summary>
    [Serializable]
    public class MesProcessCollectKeyItemInfoCriteria
    {
        /// <summary>
        /// 工序采集Id
        /// </summary>
        public double ReportProcessId { get; set; }
        /// <summary>
        /// 分页信息
        /// </summary>
        public PagingInfo PagingInfo { get; set; }
    }
}
