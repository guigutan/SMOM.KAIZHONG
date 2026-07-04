using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialReceives.APIModel
{
    /// <summary>
    /// 接收信息
    /// </summary>
    [Serializable]
    public class MaterialReceInfo
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
        /// 数量(接收数/待收数)
        /// </summary>
        public decimal Qty { get; set; }
    }
}
