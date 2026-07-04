namespace SIE.Warehouses
{
    /// <summary>
    /// 包装分配库位排序数据
    /// </summary>
    public class PackageAssignData
    {
        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocationCode { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public double StorageLocationId { get; set; }

        /// <summary>
        /// 排序依据1
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 排序依据2
        /// </summary>
        public int RouteNo { get; set; }

        /// <summary>
        /// 排序依据3
        /// </summary>
        public int Deepth { get; set; }
    }

    /// <summary>
    /// 最终排序结果
    /// </summary>
    public class StorageLocationSort
    {
        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocationCode { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public double StorageLocationId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 最深库位
        /// </summary>
        public bool IsMaxDeep { get; set; }

        /// <summary>
        /// 巷道号
        /// </summary>
        public int RouteNo { get; set; }

    }
}
