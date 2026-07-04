using SIE.Common.Configs;
using SIE.Domain.Validation;
using SIE.Equipments.Configs;
using System;

namespace SIE.Equipments.Common.Controller
{
    /// <summary>
    /// 审批流程控制器
    /// </summary>
    public class ApprovalController : DomainController
    {
        /// <summary>
        /// 获取审批流程配置(1:设备立卡)
        /// </summary>
        /// <param name="type">(1:设备立卡)</param>
        /// <returns>审批流程配置</returns>
        public virtual ApprovalConfigValue GetApprovalConfigValue(Type type)
        {
            //在只有一个值的情况下使用if else和switch都会产生异味提示,所以先固定类型
            var config = ConfigService.GetConfig(new ApprovalConfig(), type);
            if (config == null)
            {
                throw new ValidationException("未找到审批流程配置,请检查规则配置".L10N());
            }
            return config;
        }
    }
}
