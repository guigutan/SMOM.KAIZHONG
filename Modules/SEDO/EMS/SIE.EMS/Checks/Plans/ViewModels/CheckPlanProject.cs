using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.Equipments.EquipAccounts;
using System.Collections.Generic;

namespace SIE.EMS.Checks.Plans.ViewModels
{
    /// <summary>
    /// 点检计划项目
    /// </summary>
    public class CheckPlanProject
    {
        /// <summary>
        /// 点检计划列表
        /// </summary>
        public EntityList<CheckPlan> CheckPlanList { get; set; }

        /// <summary>
        /// 点检计划ViewModel列表
        /// </summary>
        public AddCheckPlanViewModel AddCheckPlan { get; set; }

        /// <summary>
        /// 设备台账列表
        /// </summary>
        public List<double> EquipAccountsIds { get; set; }

        /// <summary>
        /// 设备台账信息(简单信息)
        /// </summary>
        public List<BaseDataInfo> EquipAccountList { get; set; }

        /// <summary>
        /// 设备台账点检项目列表
        /// </summary>
        public EntityList<EquipAccountCheckProject> ProjectDetailList { get; set; }
    }
}
