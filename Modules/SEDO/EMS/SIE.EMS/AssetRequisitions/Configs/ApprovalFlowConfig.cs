using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.AssetRequisitions.Configs
{
    /// <summary>
    /// 审批流程配置
    /// </summary>
    [System.ComponentModel.DisplayName("审批流程配置")]
    [System.ComponentModel.Description("用于配置审批流程")]
    public class ApprovalFlowConfig : ModuleConfig<ApprovalFlowConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ApprovalFlowConfigValue defaultValue = new ApprovalFlowConfigValue { IsEnableApproval = false, IsEnableApprovalFlow = false };

        /// <summary>
        /// 默认值
        /// </summary>
        public override ApprovalFlowConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 审批流程配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("审批流程配置实体")]
    public class ApprovalFlowConfigValue : ConfigValue
    {
        #region 是否启用审批 IsEnableApproval
        /// <summary>
        /// 是否启用审批
        /// </summary>
        [Label("启用审批")]
        public static readonly Property<bool> IsEnableApprovalProperty = P<ApprovalFlowConfigValue>.Register(e => e.IsEnableApproval);

        /// <summary>
        /// 是否启用审批
        /// </summary>
        public bool IsEnableApproval
        {
            get { return this.GetProperty(IsEnableApprovalProperty); }
            set { this.SetProperty(IsEnableApprovalProperty, value); }
        }
        #endregion

        #region 是否启用审批流程 IsEnableApprovalFlow
        /// <summary>
        /// 是否启用审批流程
        /// </summary>
        [Label("启用审批流程")]
        public static readonly Property<bool> IsEnableApprovalFlowProperty = P<ApprovalFlowConfigValue>.Register(e => e.IsEnableApprovalFlow);

        /// <summary>
        /// 是否启用审批流程
        /// </summary>
        public bool IsEnableApprovalFlow
        {
            get { return this.GetProperty(IsEnableApprovalFlowProperty); }
            set { this.SetProperty(IsEnableApprovalFlowProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return (IsEnableApproval ? "启用审批；".L10N() : "不启用审批；".L10N())+(IsEnableApprovalFlow ? "启用审批流程".L10N() : "不启用审批流程".L10N());
        }
    }
}
