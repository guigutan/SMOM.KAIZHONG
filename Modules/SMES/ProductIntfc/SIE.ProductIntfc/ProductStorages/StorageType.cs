using SIE.ObjectModel;

namespace SIE.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 入库类型
    /// </summary>
    public enum StorageType
    {
        /// <summary>
        /// 成品
        /// </summary>
        [Label("成品")]
        Product,

        /// <summary>
        /// 半成品
        /// </summary>
        [Label("半成品")]
        SemiFinished,
    }
}