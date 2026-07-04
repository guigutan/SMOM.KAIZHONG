using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Core.Barcodes
{
    /// <summary>
    /// 条码类型
    /// </summary>
    public enum BarcodeType
    {
        /// <summary>
        /// 生产条码
        /// </summary>
        [Label("生产条码")]
        [Category("Single")]
        SN = 1,

        /// <summary>
        /// 客户条码
        /// </summary>
        [Label("客户条码")]
        [Category("Single")]
        CSN = 2,

        /// <summary>
        /// 生产周转箱条码
        /// </summary>
        [Label("生产周转箱条码")]
        [Category("Single")]
        TurnoverBox = 3,

        /// <summary>
        /// 组件条码
        /// </summary>
        [Label("组件条码")]
        [Category("Single")]
        KeyLabel = 4,

        /// <summary>
        /// 批次条码
        /// </summary>
        [Label("批次条码")]
        [Category("Batch")]
        BatchBarocde = 5,

        /// <summary>
        /// 载具号
        /// </summary>
        [Label("载具号")]
        [Category("Batch")]
        ContainerNo = 6,

        /// <summary>
        /// 拼板码
        /// </summary>
        [Label("拼板码")]
        CombinedCode = 7
    }
}