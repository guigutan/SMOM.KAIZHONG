using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.Configs
{
    /// <summary>
    /// 按库存组织配置产前准备异常后是否触发安灯预警
    /// </summary>
    [DisplayName("按库存组织配置产前准备异常后是否触发安灯预警")]
    [Description("按库存组织配置产前准备异常后是否触发安灯预警")]
    public class AndonManageWarningConfig: ModuleConfig<AndonManageWarningConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly AndonManageWarningConfigValue defaultValue = new AndonManageWarningConfigValue() { IsWarning = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override AndonManageWarningConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
