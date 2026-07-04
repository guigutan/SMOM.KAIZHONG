using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Warehouses.ItemIoLimits.ApiModels
{
    /// <summary>
    /// 物料收发控制
    /// </summary>
    [Serializable]
    public class ItemIoInfo
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性Id
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPtopName { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 最高存量
        /// </summary>
        public decimal MaxStockQty { get; set; }
    }
}
