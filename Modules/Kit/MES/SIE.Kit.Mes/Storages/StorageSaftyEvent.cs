namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 安全库存事件
    /// </summary>
    public class StorageSaftyEvent
    {
        /// <summary>
        /// 产线库存
        /// </summary>
        public StorageSafty StorageSafty { get; set; }
    }

    /// <summary>
    /// 缺料事件
    /// </summary>
    public class StorageStarvingEvent
    {
        /// <summary>
        /// 库存数量
        /// </summary>
        public decimal SaftyQty { get; set; }

        /// <summary>
        /// 缺料数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 投入数量
        /// </summary>
        public decimal DeliveryQty { get; set; }

        /// <summary>
        /// 产线库存ID
        /// </summary>
        public double StorageAreaId { get; set; }

        /// <summary>
        /// 货区类型
        /// </summary>
        public StorageAreaType AreaType { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }
    }
}