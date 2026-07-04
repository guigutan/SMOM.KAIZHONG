using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialReturnApplys.ApiModels
{
    /// <summary>
    /// 退料数量信息
    /// </summary>
    [Serializable]
    public class MaterialReturnQtyInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WoId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 退料数
        /// </summary>
        public decimal ReturnQty { get; set; }
    }
}
