using SIE.Inventory.Onhands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.InventoryControl.datas
{
    /// <summary>
    /// 库存对照表库存数据
    /// </summary>
    [Serializable]
    public class RealLotOnHand: LotLpnOnhand
    {
        /// <summary>
        /// 对照后的批次信息
        /// </summary>
        public string RealLotCode { get; set; }
    }

    /// <summary>
    /// 分组的Key
    /// </summary>
    public class GroupKey
    {
        /// <summary>
        /// 仓库ID
        /// </summary>
        public object WarehouseId { get; set; }

        /// <summary>
        /// ERP批次
        /// </summary>
        public object RealLotCode { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public object ItemId { get; set; }
    }
}
