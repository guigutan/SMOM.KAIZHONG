using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Checks.Plans.ViewModels
{
    /// <summary>
    /// 点检计划主信息
    /// </summary>
    [Serializable]
    public class CheckPlanMainInfo
    {
        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccount EquipAccount { get; set; }

        /// <summary>
        /// 点检计划明细信息列表
        /// </summary>
        public List<CheckPlanDetailInfo> CheckPlanDetailInfos { get; set; } = new List<CheckPlanDetailInfo>();
    }

    /// <summary>
    /// 点检计划明细信息
    /// </summary>
    [Serializable]
    public class CheckPlanDetailInfo
    {
        /// <summary>
        /// 点检计划
        /// </summary>
        public CheckPlan CheckPlan { get; set; }

        /// <summary>
        /// 项目周期
        /// </summary>
        public decimal? ProjectCycle { get; set; }
    }

    /// <summary>
    /// 点检计划返回信息
    /// </summary>
    [Serializable]
    public class CheckPlanResultInfo
    {
        /// <summary>
        /// 点检计划ViewModel列表
        /// </summary>
        public EntityList<CheckPlanViewModel> Data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}
