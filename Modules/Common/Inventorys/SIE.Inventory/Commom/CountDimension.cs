using SIE.ObjectModel;

namespace SIE.Inventory.Commom
{
    /// <summary>
    /// 盘点细度
    /// </summary>
    public enum CountDimension
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Label("默认")]
        Default = 0,
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        Warehouse = 10,
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        Area = 20,
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        Location = 30,
        /// <summary>
        /// 仓库+批次
        /// </summary>
        [Label("仓库+批次")]
        WarehouseLot = 40,
        /// <summary>
        /// 库位+批次
        /// </summary>
        [Label("库位+批次")]
        LocationLot = 50,
    }
}