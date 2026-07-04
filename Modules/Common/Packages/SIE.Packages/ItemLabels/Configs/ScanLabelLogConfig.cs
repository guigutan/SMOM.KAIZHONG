using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.ItemLabels.Configs
{
    /// <summary>
    /// 扫描标签日志开关配置项
    /// </summary>
    [System.ComponentModel.DisplayName("扫描标签日志开关配置项")]
    [System.ComponentModel.Description("扫描标签日志开关配置项")]
    public class ScanLabelLogConfig : GlobalConfig<ScanLabelLogConfigValue>
    {
        /// <summary>
        /// 扫描标签日志配置默认值
        /// </summary>
        readonly ScanLabelLogConfigValue defaultValue = new ScanLabelLogConfigValue { IsLogScanLabel = false };

        /// <summary>
        /// 获取默认值
        /// </summary>
        public override ScanLabelLogConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 扫描标签日志配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("扫描标签日志开关配置项")]

    public class ScanLabelLogConfigValue : ConfigValue
    {
        #region 记录扫描标签日志 IsLogScanLabel
        /// <summary>
        /// 记录扫描标签日志
        /// </summary>
        [Label("记录扫描标签日志")]
        public static readonly Property<bool> IsLogScanLabelProperty = P<ScanLabelLogConfigValue>.Register(e => e.IsLogScanLabel);

        /// <summary>
        /// 记录扫描标签日志
        /// </summary>
        public bool IsLogScanLabel
        {
            get { return this.GetProperty(IsLogScanLabelProperty); }
            set { this.SetProperty(IsLogScanLabelProperty, value); }
        }
        #endregion

    }

}
