using SIE.Common.Configs;

namespace SIE.MES.WIP.Configs
{
    /// <summary>
    /// 换料后原关键件处理方式配置项
    /// </summary>
    [System.ComponentModel.DisplayName("换料后原关键件处理方式配置项")]
    [System.ComponentModel.Description("换料后原关键件处理方式配置项")]
    public class ChangeItemHandleMethodConfig : ModuleConfig<ChangeItemHandleMethodConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override ChangeItemHandleMethodConfigValue DefaultValue => new ChangeItemHandleMethodConfigValue()
        { 
            HandleMethod= Repairs.ChangeItemHandleMethod.Scrap
        };
    }
}
