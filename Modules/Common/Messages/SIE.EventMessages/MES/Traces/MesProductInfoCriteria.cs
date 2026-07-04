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
    /// 来料检验追溯信息
    /// </summary>
    [Serializable]
    public class MesProductInfoCriteria
    {
        /// <summary>
        /// 产品编码Id
        /// </summary>
        public double ProductId { get; set; }
        /// <summary>
        /// Sn
        /// </summary>
        public string ProductSn { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDateStart { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDateEnd { get; set; }
        /// <summary>
        /// 分页信息
        /// </summary>
        public PagingInfo PagingInfo { get; set; }
    }
}
