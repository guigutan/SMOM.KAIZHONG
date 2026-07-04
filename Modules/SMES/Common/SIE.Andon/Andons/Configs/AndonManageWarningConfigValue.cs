using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.Configs
{
    /// <summary>
    /// 按库存组织配置产前准备异常后是否触发安灯预警
    /// </summary>
    [RootEntity, Serializable]
    [Label("按库存组织配置产前准备异常后是否触发安灯预警")]
    public class AndonManageWarningConfigValue: ConfigValue
    {
        #region 是否预警 IsWarning
        /// <summary>
        /// 是否预警
        /// </summary>
        [Label("是否预警")]
        public static readonly Property<YesNo?> IsWarningProperty = P<AndonManageWarningConfigValue>.Register(e => e.IsWarning);

        /// <summary>
        /// 是否预警
        /// </summary>
        public YesNo? IsWarning
        {
            get { return this.GetProperty(IsWarningProperty); }
            set { this.SetProperty(IsWarningProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示值
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            if (this.IsWarning == null)
            {
                return "NIL";
            }
            return this.IsWarning.ToLabel();
        }
    }
}
