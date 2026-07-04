using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.ErpCommon.Datas
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class OutReqCreateTransactionData
    {
        /// <summary>
        /// 需求单
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 主表Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 明细Id
        /// </summary>
        public double DetailId { get; set; }

        /// <summary>
        /// 类型(1:出库,2:入库)
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode { get; set; }
    }
}
