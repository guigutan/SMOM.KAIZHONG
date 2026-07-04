namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 标签信息
    /// </summary>
    public class PackageLabelData
    {
        /// <summary>
        /// 标签Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 标签编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// 物料包装规则Id
        /// </summary>
        public double ItemPackageRuleId { get; set; }

        /// <summary>
        /// 主单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 包装类型
        /// </summary>
        public string PackageUnitName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
