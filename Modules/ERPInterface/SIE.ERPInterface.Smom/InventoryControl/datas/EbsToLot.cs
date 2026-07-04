using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.InventoryControl.datas
{
    /// <summary>
    /// ERP批次对照WMS批次
    /// </summary>
    public enum EbsToLot
    {
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        ToLotCode = 1,

        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        ToProductionBatch = 2,

        /// <summary>
        /// 批次属性8
        /// </summary>
        [Label("批次属性8")]
        ToLotAtt8 = 3,

        /// <summary>
        /// 批次属性9
        /// </summary>
        [Label("批次属性9")]
        ToLotAtt9 = 4,

        /// <summary>
        /// 批次属性10
        /// </summary>
        [Label("批次属性10")]
        ToLotAtt10 = 5,

        /// <summary>
        /// 批次属性11
        /// </summary>
        [Label("批次属性11")]
        ToLotAtt11 = 6,

        /// <summary>
        /// 批次属性12
        /// </summary>
        [Label("批次属性12")]
        ToLotAtt12 = 7,
    }
}
