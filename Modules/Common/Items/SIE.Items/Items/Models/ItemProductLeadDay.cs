namespace SIE.Items
{
    /// <summary>
    /// 物料生产提前期
    /// </summary>
    public class ItemProductLeadDay
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public ItemSourceType? ItemSourceType { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public double? ProductModelId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ItemType Type { get; set; }

        /// <summary>
        /// 生产提前期（单位为：天）
        /// </summary>
        public int? ProductLeadDay { get; set; }

        /// <summary>
        /// 采购提前期 （天）
        /// </summary>
        public int? PurchaseLeadTime { get; set; }

        /// <summary>
        /// 物料是否标记
        /// </summary>
        public bool? IsMarked { get; set; }
    }
}