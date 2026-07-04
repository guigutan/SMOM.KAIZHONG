using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.Reports.Datas
{
    /// <summary>
    /// 工单占用数据
    /// </summary>
    [Serializable]
    public class WoDemandInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 接收数
        /// </summary>
        public decimal ReceivedQty { get; set; }

        /// <summary>
        /// 挪入数
        /// </summary>
        public decimal MovedInQty { get; set; }

        /// <summary>
        /// 上料数
        /// </summary>
        public decimal FeedQty { get; set; }

        /// <summary>
        /// 挪出数
        /// </summary>
        public decimal MovedOutQty { get; set; }

        /// <summary>
        /// 正常退料在途
        /// </summary>
        public decimal ReturnQtyInTransit { get; set; }

        /// <summary>
        /// 不良退料在途
        /// </summary>
        public decimal NgReturnQtyInTransit { get; set; }

        /// <summary>
        /// 退料数
        /// </summary>
        public decimal ReturnQty { get; set; }

        /// <summary>
        /// 不良退料数
        /// </summary>
        public decimal NgReturnQty { get; set; }
    }
}
