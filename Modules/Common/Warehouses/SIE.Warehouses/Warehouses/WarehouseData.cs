namespace SIE.Warehouses
{
    /// <summary>
    /// 仓库信息
    /// </summary>
    public class WarehouseData
    {
        /// <summary>
        /// 仓库id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public double UserId { get; set; }
    }

    /// <summary>
    /// 库区信息
    /// </summary>
    public class StorageAreaData
    {
        /// <summary>
        /// 库区Id
        /// </summary>
        public double AreaId { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 库区名称
        /// </summary>
        public string AreaName { get; set; }
    }

    /// <summary>
    /// 库位信息
    /// </summary>
    public class LocationData
    {
        /// <summary>
        /// ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 库区ID
        /// </summary>
        public double AreaId { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 库区名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 重量限制
        /// </summary>
        public decimal? WeightLimit { get; set; }

        /// <summary>
        /// 体积限制
        /// </summary>
        public decimal? VolumeLimit { get; set; }
    }

    /// <summary>
    /// 简易仓库信息
    /// </summary>
    public class SimpleWarehouseData
    {
        /// <summary>
        /// 仓库ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// 简易库位信息
    /// </summary>
    public class SimpleStorageLocData
    {
        /// <summary>
        /// 库位
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string Code { get; set; }
    }
}
