using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using System;
using System.Text;

namespace SIE.Fixtures.FixtureDemands.Config
{
    /// <summary>
    /// 工治具需求单配置项
    /// </summary>
    [System.ComponentModel.DisplayName("工治具需求清单配置项")]
    [System.ComponentModel.Description("工治具需求清单配置项")]
    public class FixtureDemandsConfig : ModuleConfig<FixtureDemandsConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly FixtureDemandsConfigValue defaultValue = new FixtureDemandsConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override FixtureDemandsConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 工治具需求单配置项
    /// </summary>
    [RootEntity, Serializable]
    [Label("工治具需求单配置项")]
    public class FixtureDemandsConfigValue : ConfigValue
    {
        #region 启用审批 SwitchApproval
        /// <summary>
        /// 启用审批
        /// </summary>
        [Label("启用审批")]
        public static readonly Property<bool> SwitchApprovalProperty = P<FixtureDemandsConfigValue>.Register(e => e.SwitchApproval);

        /// <summary>
        /// 启用审批
        /// </summary>
        public bool SwitchApproval
        {
            get { return this.GetProperty(SwitchApprovalProperty); }
            set { this.SetProperty(SwitchApprovalProperty, value); }
        }
        #endregion

        #region 审批方式 ApprovalWay
        /// <summary>
        /// 审批方式
        /// </summary>
        [Label("审批方式")]
        public static readonly Property<ApprovalWay> ApprovalWayProperty = P<FixtureDemandsConfigValue>.Register(e => e.ApprovalWay);

        /// <summary>
        /// 审批方式
        /// </summary>
        public ApprovalWay ApprovalWay
        {
            get { return this.GetProperty(ApprovalWayProperty); }
            set { this.SetProperty(ApprovalWayProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return ",是否启用审批:{0}".L10nFormat(this.SwitchApproval ? "yes" : "no");
        }

    }

}
