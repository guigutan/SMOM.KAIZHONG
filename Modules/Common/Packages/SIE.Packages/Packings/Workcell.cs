using System;

namespace SIE.Packages.Packings
{
    /// <summary>
    /// 工作单元
    /// </summary>
    [Serializable]
    public class Workcell
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public double? EmployeeId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 货区ID
        /// </summary>
        public double? StorageAreaId { get; set; }

        /// <summary>
        /// 货位ID
        /// </summary>
        public double? StorageLocationId { get; set; }
    }
}
