using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 库存调拨接口数据
    /// </summary>
    [Serializable]
    public class InventoryAllocateData : ErpInfoData
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 订单类型120直接调拨 121两步调拨
        /// </summary>
        public int OrderType { get; set; }
       
        /// <summary>
        /// 事务日期
        /// </summary>
        public DateTime TransDate { get; set; }
         
        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        ///  来源仓库编码
        /// </summary>
        public string SourceWHCode { get; set; }

        /// <summary>
        /// 目标仓库编码
        /// </summary>
        public string TargetWHCode { get; set; }

        /// <summary>
        /// 在途库位编码(默认PickTo)
        /// </summary>
        public string PickToCode { get; set; }

        /// <summary>
        /// 货主编码(默认*)
        /// </summary>
        public string ShipperCode { get; set; }

        /// <summary>
        /// 需求列表
        /// </summary>
        public List<InventoryAllocateRequireData> RequireList { get; set; } = new List<InventoryAllocateRequireData>();
    }
}
