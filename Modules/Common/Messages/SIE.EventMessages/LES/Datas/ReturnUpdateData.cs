using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.LES.Datas
{
    /// <summary>
    /// wms退料更新退料申请信息
    /// </summary>
    [Serializable]
    public class ReturnUpdateData
    {
        /// <summary>
        /// 库存组织
        /// </summary>
        public int InvOrgId { get; set; }

        /// <summary>
        /// 退料单号
        /// </summary>
        public string ReturnApplyNo { get; set; }

        /// <summary>
        /// 退料单明细行号
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
        /// 收货数
        /// </summary>
        public decimal ReceiveQty { get; set; }
    }
}
