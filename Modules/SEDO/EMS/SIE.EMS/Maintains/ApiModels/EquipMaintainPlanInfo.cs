using SIE.EMS.Equipments.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Maintains.ApiModels
{
    /// <summary>
    /// 设备保养计划信息
    /// </summary>
    [Serializable]
    public class EquipMaintainPlanInfo
    {
        /// <summary>
        /// 设备信息
        /// </summary>
        public EquipInfo EquipInfo { get; set; }

        /// <summary>
        /// 保养计划集合
        /// </summary>
        public List<MaintainPlanInfo> MaintainPlans { get; } = new List<MaintainPlanInfo>();
    }
}