using SIE.Core.Common;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 库位简单实体类
    /// </summary>
    [Serializable]
    public class StorageLocationData : EntityBaseData
    {
        /// <summary>
        /// 暂存库位
        /// </summary>
        public bool IsTemporary { get; set; }

        /// <summary>
        /// 存储库位
        /// </summary>
        public bool IsLayIn { get; set; }

        /// <summary>
        /// 是否拣货
        /// </summary>
        public bool IsPick { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string Name { get; set; }
    }
}
