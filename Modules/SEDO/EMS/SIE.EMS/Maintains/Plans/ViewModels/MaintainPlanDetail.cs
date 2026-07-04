using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Maintains.Plans.ViewModels
{
    /// <summary>
    /// 保养计划明细
    /// </summary>
    [Serializable]
    public class MaintainPlanDetail
    {
        /// <summary>
        /// 保养计划列表
        /// </summary>
        public EntityList<MaintainPlan> MaintainPlanList { get; set; }

        /// <summary>
        /// 设备台账ID列表
        /// </summary>
        public List<double> EquipAccountIds { get; set; }
    }
}
