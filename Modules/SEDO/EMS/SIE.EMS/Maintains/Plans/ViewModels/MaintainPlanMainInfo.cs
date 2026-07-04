using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.Equipments.EquipAccounts;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Maintains.Plans.ViewModels
{
    /// <summary>
    /// 保养计划主信息
    /// </summary>
    [Serializable]
    public class MaintainPlanMainInfo
    {
        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccount EquipAccount { get; set; }

        /// <summary>
        /// 保养计划明细信息列表
        /// </summary>
        public List<MaintainPlanDetailInfo> MaintainPlanDetailInfos { get; set; } = new List<MaintainPlanDetailInfo>();
    }

    /// <summary>
    /// 保养计划明细信息
    /// </summary>
    [Serializable]
    public class MaintainPlanDetailInfo
    {
        /// <summary>
        /// 保养计划
        /// </summary>
        public MaintainPlan MaintainPlan { get; set; }

        /// <summary>
        /// 项目周期
        /// </summary>
        public decimal? ProjectCycle { get; set; }
    }

    /// <summary>
    /// 保养计划返回信息
    /// </summary>
    [Serializable]
    public class MaintainPlanResultInfo
    {
        /// <summary>
        /// 保养计划ViewModel列表
        /// </summary>
        public EntityList<MaintainPlanViewModel> Data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}
