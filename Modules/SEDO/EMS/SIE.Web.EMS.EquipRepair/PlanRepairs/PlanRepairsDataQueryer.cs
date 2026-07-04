using SIE.Common.Configs;
using SIE.Domain.Validation;
using SIE.EMS.EquipRepair.PlanRepairs;
using SIE.Equipments.Configs;
using SIE.Web.Data;
using System;

namespace SIE.Web.EMS.EquipRepair.PlanRepairs
{
    /// <summary>
    /// 查询器
    /// </summary>
    public class PlanRepairsDataQueryer : DataQueryer
    {

        /// <summary>
        /// 获取运行定标
        /// </summary>
        /// <returns></returns>
        public PlanRepair GetPlanRepairs()
        {
            return RT.Service.Resolve<PlanRepairsController>().GetPlanRepair();
        }

        /// <summary>
        /// 获取是否启用审核功能
        /// </summary>
        /// <returns></returns>
        public bool GetEnableApproval()
        {
            var config = ConfigService.GetConfig(new ApprovalConfig(), typeof(PlanRepair));
            if (config == null)
            {
                throw new ValidationException("未找到审批流程配置,请检查规则配置".L10N());
            }
            return config.EnableAudit;
        }
    }
}
