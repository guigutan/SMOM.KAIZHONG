using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing.Model
{
    /// <summary>
    /// 发货确认数据
    /// </summary>
    [Serializable]
    public class OutboundConfirmData
    {

        /// <summary>
        /// 发货明细Id
        /// </summary>
        public double OutboundId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// 工序标签
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 委外工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 接收工厂
        /// </summary>
        public string OutFactory { get; set; }

        /// <summary>
        /// 发出工厂
        /// </summary>
        public string InitiatorFactory { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDate { get; set; }
    }
}
