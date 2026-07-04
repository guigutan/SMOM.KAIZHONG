using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.MES.QTimes.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Configs
{
    /// <summary>
    /// QT调度获取条码时间范围配置项
    /// </summary>
    [RootEntity,Serializable]
    [Label("QT调度获取条码时间范围配置项")]
    public class QTimeJobTimeRangeConfigValue : ConfigValue
    {
        #region 采集时间 CollectTime
        /// <summary>
        /// 采集时间
        /// </summary>
        [Label("采集时间")]
        public static readonly Property<QTimeConfigTimeRange?> CollectTimeProperty = P<QTimeJobTimeRangeConfigValue>.Register(e => e.CollectTime);

        /// <summary>
        /// 采集时间
        /// </summary>
        public QTimeConfigTimeRange? CollectTime
        {
            get { return this.GetProperty(CollectTimeProperty); }
            set { this.SetProperty(CollectTimeProperty, value); }
        }
        #endregion

        #region 天 Day
        /// <summary>
        /// 天
        /// </summary>
        [Label("天")]
        public static readonly Property<int> DayProperty = P<QTimeJobTimeRangeConfigValue>.Register(e => e.Day);

        /// <summary>
        /// 天
        /// </summary>
        public int Day
        {
            get { return this.GetProperty(DayProperty); }
            set { this.SetProperty(DayProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示名称
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            return CollectTime?.ToLabel().L10N();
        }
    }
}
