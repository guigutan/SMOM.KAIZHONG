using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing.Model
{
    /// <summary>
    /// 发货修改数据
    /// </summary>
    [Serializable]
    public class OutboundModifyData
    {

        /// <summary>
        /// 发货确认明细Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 流程单号
        /// </summary>
        public string FlowNo { get; set; }

        /// <summary>
        /// 发货人
        /// </summary>
        public string Outer { get; set; }

        /// <summary>
        /// 接收工厂
        /// </summary>
        public string OutFactory { get; set; }

        /// <summary>
        /// 发出工厂
        /// </summary>
        public string InitiatorFactory { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public string DelveryDate { get; set; }

        /// <summary>
        /// 框数
        /// </summary>
        public decimal Qty { get; set; }

        public List<OutboundModifyDetailData> detailDatas { get; set; } = new List<OutboundModifyDetailData>();
    }

    /// <summary>
    /// 标签数据
    /// </summary>
    [Serializable]
    public class OutboundModifyDetailData
    {
        /// <summary>
        /// 发货确认，工序标签明细数据Id
        /// </summary>
        public double SnDetailId { get; set; }


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
