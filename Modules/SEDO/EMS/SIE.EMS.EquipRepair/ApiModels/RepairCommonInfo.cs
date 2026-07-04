using SIE.EMS.EquipRepairs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 维修管理基础
    /// </summary>
    public class RepairCommonInfo
    {
        /// <summary>
        /// 维修单Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 维修单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double? EquipAccountId { get; set; }

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModel { get; set; }

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public double? EquipTypeId { get; set; }

        /// <summary>
        /// 维修状态
        /// </summary>
        public EquipRepairState State { get; set; }
    }
}
