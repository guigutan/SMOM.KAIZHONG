using SIE.EMS.Equipments.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Checks.ApiModels
{
    /// <summary>
    /// 设备点检计划信息
    /// </summary>
    [Serializable]
    public class EquipCheckPlanInfo
    {
        /// <summary>
        /// 设备信息
        /// </summary>
        public EquipInfo EquipInfo { get; set; }

        /// <summary>
        /// 点检计划集合
        /// </summary>
        public List<CheckPlanInfo> CheckPlans { get; } = new List<CheckPlanInfo>();
    }
}