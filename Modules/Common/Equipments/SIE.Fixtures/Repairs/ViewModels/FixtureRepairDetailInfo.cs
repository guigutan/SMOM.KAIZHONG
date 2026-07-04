using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Fixtures.Repairs.ViewModels
{
    /// <summary>
    /// 工治具明细报修-添加
    /// </summary>
    [Serializable]
    public class FixtureRepairDetailInfo
    {
        /// <summary>
        /// 报修前状态
        /// </summary>
        public RepairBeforeState RepairBeforeState { get; set; }
        /// <summary>
        /// 工单
        /// </summary>
        public double? WorkOrderId { get; set; }
        /// <summary>
        /// 工单Display
        /// </summary>
        public string WorkOrderId_Display { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public double? FixtureWarehouseId { get; set; }
        /// <summary>
        /// 仓库Display
        /// </summary>
        public string FixtureWarehouseId_Display { get; set; }
        /// <summary>
        /// 库位
        /// </summary>
        public double? FixtureStorageLocationId { get; set; }
        /// <summary>
        /// 库位Display
        /// </summary>
        public string FixtureStorageLocationId_Display { get; set; }
        /// <summary>
        /// 库位名称
        /// </summary>
        public string FixtureStorageLocationName { get; set; }
    }
}
