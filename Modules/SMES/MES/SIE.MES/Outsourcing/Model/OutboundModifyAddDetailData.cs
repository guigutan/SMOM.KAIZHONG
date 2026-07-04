using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing.Model
{
    /// <summary>
    /// 新添加明细标签
    /// </summary>
    [Serializable]
    public class OutboundModifyAddDetailData
    {
        /// <summary>
        /// 委外发货明细Id
        /// </summary>
        public double ProcessingOutboundId { get; set; }

        /// <summary>
        /// 标签数量
        /// </summary>
        public decimal SnQty { get; set; }

        /// <summary>
        /// 工序标签
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }
    }
}
