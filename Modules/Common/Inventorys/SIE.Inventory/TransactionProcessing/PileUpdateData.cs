using SIE.Inventory.Piles;
using SIE.Inventory.TransactionProcessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Inventory
{
    /// <summary>
    /// 事务交易更新垛数据
    /// </summary>
    public class PileUpdateData
    {
        /// <summary>
        /// 垛
        /// </summary>
        public double PileId { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 垛号
        /// </summary>
        public string Lpn { get; set; }
       
        /// <summary>
        /// 库存数据
        /// </summary>
        public InvCollectData InvCollectData { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 批次Id
        /// </summary>
        public double LotId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string StorerCode { get; set; }
    }
}
