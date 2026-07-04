
using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Maintains.Plans.ViewModels
{
    /// <summary>
    /// 执行数据结果
    /// </summary>
    [Serializable]
    public class ExecuteDataResult
    {
        /// <summary>
        /// 保养计划列表
        /// </summary>
        public EntityList<MaintainPlan> MaintainPlanList { get; set; }

        /// <summary>
        /// 项目列表
        /// </summary>
        public List<EquipAccountMaintainProject> ProjectDetailList { get; set; }
    }
}
