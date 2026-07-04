using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.LES.Datas
{
    /// <summary>
    /// 发料更新备料明细实体
    /// </summary>
    [Serializable]
    public class ShippingUpdateData
    {
        /// <summary>
        /// 库存组织
        /// </summary>
        public int InvOrgId { get; set; }

        /// <summary>
        /// 备料单号
        /// </summary>
        public string MaterialPreNo { get; set; }

        /// <summary>
        /// 备料明细行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 发料数
        /// </summary>
        public decimal ShippingQty { get; set; }

        /// <summary>
        /// 取消数
        /// </summary>
        public decimal CancelQty { get; set; }
    }
}
