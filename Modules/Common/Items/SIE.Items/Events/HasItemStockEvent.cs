namespace SIE.Items.Events
{
    /// <summary>
    /// 物料是否存在库存接受对象
    /// </summary>
    public class HasItemStockEvent
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double IetmId { get; set; }
    }
}
