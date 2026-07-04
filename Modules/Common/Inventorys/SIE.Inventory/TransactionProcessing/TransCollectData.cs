using SIE.Inventory.Transactions;
using System.Collections.Generic;

namespace SIE.Inventory.TransactionProcessing
{
    /// <summary>
    /// 事务交易数据
    /// </summary>
    public class TransCollectData
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public TransCollectData()
        {
            baseTransactionData = new BaseTransactionData();
            StockTransList = new List<StockTrans>();
        }

        /// <summary>
        /// 物料id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 批次编码
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 交易双方参数
        /// </summary>
        public List<StockTrans> StockTransList { get; set; }

        /// <summary>
        /// 调拨源数量
        /// </summary>
        public bool IsFromAllottedQty { get; set; }

        /// <summary>
        /// 调拨目标数量
        /// </summary>
        public bool IsToAllottedQty { get; set; }

        /// <summary>
        /// 交易记录相关数据
        /// </summary>
        public BaseTransactionData baseTransactionData { get; set; }
    }
}
