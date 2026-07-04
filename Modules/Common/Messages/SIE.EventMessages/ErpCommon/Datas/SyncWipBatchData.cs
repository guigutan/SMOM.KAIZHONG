using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.ErpCommon.Datas
{
    [Serializable]
    public class SyncWipBatchData
    {
        public double Id { get; set; }

        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }
    }
}
