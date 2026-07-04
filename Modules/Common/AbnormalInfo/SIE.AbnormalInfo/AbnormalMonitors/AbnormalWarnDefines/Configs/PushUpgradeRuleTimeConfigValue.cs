using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors.Configs
{
    /// <summary>
    /// 推送升级机制循环次数配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("推送升级机制循环次数配置值")]
    public class PushUpgradeRuleTimeConfigValue : ConfigValue
    {
        #region 循环次数 CyclicTimes
        /// <summary>
        /// 循环次数
        /// </summary>
        [Label("循环次数")]
        public static readonly Property<int?> CyclicTimesProperty = P<PushUpgradeRuleTimeConfigValue>.Register(e => e.CyclicTimes);

        /// <summary>
        /// 循环次数
        /// </summary>
        public int? CyclicTimes
        {
            get { return GetProperty(CyclicTimesProperty); }
            set { SetProperty(CyclicTimesProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            if (CyclicTimes == null)
                return "3";
            return CyclicTimes?.ToString();
        }
    }
}
