using SIE.LES.MaterialPreparations.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialPreparations.ApiModels
{
    /// <summary>
    /// 备料明细信息
    /// </summary>
    [Serializable]
    public class MaterialPreDetailInfo
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 主表Id
        /// </summary>
        public double MaterialPreId { get; set; }

        /// <summary>
        /// 备料需求单号
        /// </summary>
        public string MaterialPreNo { get; set; }

        /// <summary>
        /// 主表工单Id
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
        /// 备料明细状态
        /// </summary>
        public PrepareDetailStatus PreDetailStatus { get; set; }

        /// <summary>
        /// 本次备料数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 取消数
        /// </summary>
        public decimal CancelQty { get; set; }

        /// <summary>
        /// 发料数
        /// </summary>
        public decimal ShippingQty { get; set; }

        /// <summary>
        /// 拒收数
        /// </summary>
        public decimal RefuseQty { get; set; }

        /// <summary>
        /// 接收数
        /// </summary>
        public decimal ReceiveQty { get; set; }
    }
}
