using SIE.Inventory.Onhands;
using System;

namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// 交易双方参数
    /// </summary>
    [Serializable]
    public class StockTrans
    {
        /// <summary>
        /// 自LPN
        /// </summary>
        private string fromLpn;

        /// <summary>
        /// 自LPN
        /// </summary>
        public string FromLpn
        {
            get
            {
                return fromLpn.IsNullOrEmpty() ? "*" : fromLpn;
            }
            set
            {
                fromLpn = value;
            }
        }

        /// <summary>
        /// 自库位
        /// </summary>
        public double FromLocationId
        {
            get;
            set;
        }

        /// <summary>
        /// 自库存状态
        /// </summary>
        public OnhandState? FromOnhandState { get; set; }

        /// <summary>
        /// 来源货主
        /// </summary>
        public string FromStorerCode { get; set; }

        /// <summary>
        /// 至LPN
        /// </summary>
        private string toLpn;

        /// <summary>
        /// 至LPN
        /// </summary>
        public string ToLpn
        {
            get
            {
                return toLpn.IsNullOrEmpty() ? "*" : toLpn;
            }
            set
            {
                toLpn = value;
            }
        }

        /// <summary>
        /// 至库位
        /// </summary>
        public double ToLocationId
        {
            get;
            set;
        }

        /// <summary>
        /// 至库存状态
        /// </summary>
        public OnhandState? ToOnhandState { get; set; }

        /// <summary>
        /// 交易数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 至仓库
        /// </summary>
        public double? ToWarehouseId
        {
            get;
            set;
        }
      
        /// <summary>
        /// 目标货主，用于委外库存更新，空则等于来源库存货主
        /// </summary>
        public string ToStorerCode { get; set; }
    }
}
