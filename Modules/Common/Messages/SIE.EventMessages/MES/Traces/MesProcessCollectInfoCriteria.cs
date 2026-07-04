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
    /// 反向追溯-Mes工序采集信息查询实体
    /// </summary>
    [Serializable]
    public class MesProcessCollectInfoCriteria
    {
        /// <summary>
        /// 产品版本Id
        /// </summary>
        public double ProductVersionId { get; set; }
        /// <summary>
        /// 分页信息
        /// </summary>
        public PagingInfo PagingInfo { get; set; }
    }
}
