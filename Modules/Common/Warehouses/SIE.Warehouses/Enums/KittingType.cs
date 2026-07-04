using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Warehouses.Enums
{
    /// <summary>
    /// 齐套情况
    /// </summary>
    public enum KittingType
    {
        /// <summary>
        /// 库存齐套
        /// </summary>
        [Label("库存齐套")]
        StoreFull = 0,

        /// <summary>
        /// 在制齐套
        /// </summary>
        [Label("在制齐套")]
        MakingFull = 1,

        /// <summary>
        /// 采购齐套
        /// </summary>
        [Label("采购齐套")]
        PurchareFull = 2,

        /// <summary>
        /// 缺货
        /// </summary>
        [Label("缺货")]
        Scarce = 3,

        /// <summary>
        /// 调拨齐套
        /// </summary>
        [Label("调拨齐套")]
        AllocateFull = 4,
    }
}
