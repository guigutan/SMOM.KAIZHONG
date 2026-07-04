using SIE.Packages.Boxs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Inventory.Piles.Data
{
    /// <summary>
    /// 垛表数据
    /// </summary>
    [Serializable]
    public class PileData
    {
        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 当前位置
        /// </summary>
        public string CurLocation { get; set; }

        /// <summary>
        /// 体重(KG)
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// 长(CM)
        /// </summary>
        public decimal? Length { get; set; }

        /// <summary>
        /// 宽(CM)
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// 高(CM)
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName { get; set; }
    }

    /// <summary>
    /// LPN状态校验数据
    /// </summary>
    [Serializable]
    public class PileStateData
    {
        /// <summary>
        /// 状态
        /// </summary>
        public BoxState PileState { get; set; }

        /// <summary>
        /// 物料状态
        /// </summary>
        public ItemState? ItemState { get; set; }

        /// <summary>
        /// LPN
        /// </summary>
        public string Code { get; set; }
    }
}
