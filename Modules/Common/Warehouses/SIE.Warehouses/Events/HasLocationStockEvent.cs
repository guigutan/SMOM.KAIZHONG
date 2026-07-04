namespace SIE.Warehouses.Events
{
    /// <summary>
    /// 库位是否存在库存接受对象
    /// </summary>
    public class HasLocationStockEvent
    {
        /// <summary>
        /// 库位ID
        /// </summary>
        public double LocId { get; set; }
    }
}
