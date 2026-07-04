using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.ErpCommon.Datas
{
    /// <summary>
    /// 发货确认事务上传数据
    /// </summary>
    [Serializable]
    public class OutboundConfirmTransactionData
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// 单据Id
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 唯一值
        /// </summary>
        public string Zuid { get; set; }

    }
}
