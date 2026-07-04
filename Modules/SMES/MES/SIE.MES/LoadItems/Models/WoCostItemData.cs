using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.LoadItems.Models
{
    /// <summary>
    /// 工单耗用单基础数据
    /// </summary>
    [Serializable]
    public class WoCostItemBaseData
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料拓展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料拓展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
