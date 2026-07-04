using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs.Enums;
using System;

namespace SIE.EMS.API.APIModels
{
    /// <summary>
    /// 维修单汇总概况
    /// </summary>
    [Serializable]
    public class RepairBillsOverviewResult
    {
        /// <summary>
        /// 维修状态
        /// </summary>
        public EquipRepairState State { get; set; }

        /// <summary>
        /// 维修状态名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 前端对应的跳转路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 状态对应的数量
        /// </summary>
        public int Count { get; set; }
    }
}