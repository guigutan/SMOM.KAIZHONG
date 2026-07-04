using SIE.Common.Configs;
using SIE.Domain.Validation;
using SIE.Equipments.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EarlierStage.Common.Controller
{
    /// <summary>
    /// 预算项目审批流程控制器
    /// </summary>
    public class EarlierStageApprovalController : DomainController
    {
        /// <summary>
        /// 获取审批流程配置
        /// </summary>
        /// <returns>审批流程配置</returns>
        public virtual ApprovalConfigValue GetApprovalConfigValue(Type type)
        {
            var config = ConfigService.GetConfig(new ApprovalConfig(), type);
            if (config == null)
            {
                throw new ValidationException("未找到审批流程配置,请检查规则配置".L10N());
            }
            return config;
        }
    }
}
