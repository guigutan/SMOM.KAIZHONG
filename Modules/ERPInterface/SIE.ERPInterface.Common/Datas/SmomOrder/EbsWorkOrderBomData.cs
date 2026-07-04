using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.SmomOrder
{
    /// <summary>
    /// Ebs工单bom信息
    /// </summary>
    [Serializable]
    public class EbsWorkOrderBomData
    {
        /// <summary>
        /// Erp工单bom主键
        /// </summary>
        public string ErpKey { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal RequireQty { get; set; }

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal SingleQty { get; set; }

        /// <summary>
        /// 是否反冲物料
        /// </summary>
        public bool IsRecoilItem { get; set; }

        /// <summary>
        /// 是否虚拟物料
        /// </summary>
        public bool IsVritualItem { get; set; }

        /// <summary>
        /// 是否按单标识
        /// </summary>
        public bool IsByBill { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
