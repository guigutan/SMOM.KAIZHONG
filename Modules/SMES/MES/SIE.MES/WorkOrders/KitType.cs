using SIE.ObjectModel;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 齐套状态
    /// </summary>
    public enum KitType
    {
        /// <summary>
        /// 缺料
        /// </summary>
        [Label("缺料")]
        LackMaterial,

        /// <summary>
        /// 物料齐套
        /// </summary>
        [Label("物料齐套")]
        Kitting,
    }
}