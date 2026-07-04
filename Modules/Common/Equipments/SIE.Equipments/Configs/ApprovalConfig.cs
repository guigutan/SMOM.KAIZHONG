using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.Configs
{
    /// <summary>
    /// 审批流程配置
    /// </summary>
    [System.ComponentModel.DisplayName("审批流程配置")]
    [System.ComponentModel.Description("用于配置审批流程")]
    public class ApprovalConfig : ModuleConfig<ApprovalConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ApprovalConfigValue defaultValue = new ApprovalConfigValue { };

        /// <summary>
        /// 默认值
        /// </summary>
        public override ApprovalConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 审批流程配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("审批流程配置值")]
    public class ApprovalConfigValue : ConfigValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ApprovalConfigValue()
        {
            EnableAudit = true;
            EnableApproval = false;
        }

        #region 是否启用审批 EnableAudit
        /// <summary>
        /// 是否启用审批
        /// </summary>
        [Label("启用审批")]
        public static readonly Property<bool> EnableAuditProperty = P<ApprovalConfigValue>.Register(e => e.EnableAudit);

        /// <summary>
        /// 是否启用审批
        /// </summary>
        public bool EnableAudit
        {
            get { return this.GetProperty(EnableAuditProperty); }
            set { this.SetProperty(EnableAuditProperty, value); }
        }
        #endregion

        #region 是否启用审批流程 EnableApproval
        /// <summary>
        /// 是否启用审批流程
        /// </summary>
        [Label("启用审批流程")]
        public static readonly Property<bool> EnableApprovalProperty = P<ApprovalConfigValue>.Register(e => e.EnableApproval);

        /// <summary>
        /// 是否启用审批流程
        /// </summary>
        public bool EnableApproval
        {
            get { return this.GetProperty(EnableApprovalProperty); }
            set { this.SetProperty(EnableApprovalProperty, value); }
        }
        #endregion
    }
}
