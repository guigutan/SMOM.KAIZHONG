using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Configs
{
    /// <summary>
    /// 拣货时拆标，是否自动扫描标签
    /// </summary>
    [System.ComponentModel.DisplayName("拣货时拆标，是否自动扫描标签")]
    [System.ComponentModel.Description("拣货时拆标，是否自动扫描标签")]
    public class AfterSpliteAutoScanConfig : GlobalConfig<AfterSpliteAutoScanConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly AfterSpliteAutoScanConfigValue defaultValue = new AfterSpliteAutoScanConfigValue { AfterSpliteAutoScanType= AfterSpliteAutoScanType.NoScan};

        /// <summary>
        /// 默认值
        /// </summary>
        public override AfterSpliteAutoScanConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 拣货时拆标，是否自动扫描标签
    /// </summary>
    [RootEntity, Serializable]
    [Label("拣货时拆标，是否自动扫描标签")]
    public class AfterSpliteAutoScanConfigValue : ConfigValue
    {
        /// <summary>
        /// 拣货时拆标，是否自动扫描标签
        /// </summary>
        [Label("拣货时拆标，是否自动扫描标签")]
        public static readonly Property<AfterSpliteAutoScanType> AfterSpliteAutoScanTypeProperty = P<AfterSpliteAutoScanConfigValue>.Register(e => e.AfterSpliteAutoScanType);

        /// <summary>
        /// 拣货时拆标，是否自动扫描标签
        /// </summary>
        public AfterSpliteAutoScanType AfterSpliteAutoScanType
        {
            get { return this.GetProperty(AfterSpliteAutoScanTypeProperty); }
            set { this.SetProperty(AfterSpliteAutoScanTypeProperty, value); }
        }

        /// <summary>
        /// 启用接口配置
        /// </summary>
        /// <returns>启用接口配置</returns>
        public override string Display()
        {
            return AfterSpliteAutoScanType.ToLabel().L10N();
        }
    }
}
