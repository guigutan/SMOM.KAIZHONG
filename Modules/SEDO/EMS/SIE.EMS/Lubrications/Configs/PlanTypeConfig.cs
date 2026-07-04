using SIE.Common.Configs;

namespace SIE.EMS.Lubrications.Configs
{
    /// <summary>
    /// 计划规则
    /// </summary>
    [System.ComponentModel.DisplayName("计划规则")]
    [System.ComponentModel.Description("生成润滑记录的计划规则")]
    public class PlanTypeConfig : ModuleConfig<PlanTypeConfigValue>
    {
        /// <summary>
        /// 生成润滑记录的计划规则
        /// </summary>
        readonly PlanTypeConfigValue defaultValue = new PlanTypeConfigValue {};

        /// <summary>
        /// 默认值
        /// </summary>
        public override PlanTypeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
