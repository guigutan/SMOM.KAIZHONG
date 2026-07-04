using SIE.Common.Configs;

namespace SIE.Dock.DockAppoints.Configs
{
    /// <summary>
    /// 月台预约号生成规则
    /// </summary>
    [System.ComponentModel.DisplayName("月台预约号生成规则")]
    [System.ComponentModel.Description("月台预约号生成规则")]
    public class DockAppointNoConfig : ModuleConfig<DockAppointNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly DockAppointNoConfigValue defaultValue = new DockAppointNoConfigValue { NumberRule = null, PrintTemplate = null, MaxAppointTime = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override DockAppointNoConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}