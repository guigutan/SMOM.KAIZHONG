using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// 类型
    /// </summary>
    public enum FunctionType
    {
        /// <summary>
        /// 入库
        /// </summary>
        [Label("入库")]
        Receipt = 0,

        /// <summary>
        /// 出库
        /// </summary>
        [Label("出库")]
        Shipment = 1,

        /// <summary>
        /// 库内
        /// </summary>
        [Label("库内")]
        Inventory = 2,

        /// <summary>
        /// 生产
        /// </summary>
        [Label("生产")]
        Product = 3,

        /// <summary>
        /// 其他
        /// </summary>
        [Label("其他")]
        Other = 4,

    }
}
