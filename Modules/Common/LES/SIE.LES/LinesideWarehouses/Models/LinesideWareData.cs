using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.LinesideWarehouses.Models
{
    /// <summary>
    /// 线边仓基础信息类
    /// </summary>
    [Serializable]
    public class LinesideWareBaseData
    {
        /// <summary>
        /// 线边仓id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 产线id
        /// </summary>
        public double? WipResouceId { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double StorageLocationId { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName { get; set; }
    }
}
