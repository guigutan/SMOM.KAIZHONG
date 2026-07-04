using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.LES.Datas
{
    /// <summary>
    /// 发运订单更新备料需求单Asn单号
    /// </summary>
    [Serializable]
    public class ShippingUpdateSoNoData
    {
        /// <summary>
        /// 库存组织
        /// </summary>
        public int InvOrgId { get; set; }

        /// <summary>
        /// 来源类型(0-工单  1备料单需求)
        /// </summary>
        public int SourceType { get; set; }

        /// <summary>
        /// 对应行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        /// 发运订单号
        /// </summary>
        public string ShippingOrderNo { get; set; }
    }
}
