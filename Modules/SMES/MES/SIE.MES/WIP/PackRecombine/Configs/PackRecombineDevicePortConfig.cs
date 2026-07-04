using SIE.Common.Configs;
using SIE.MES.WIP.Configs;
using System.ComponentModel;

namespace SIE.MES.WIP.PackRecombine.Configs
{
    /// <summary>
    /// 设置端口类型
    /// </summary>
    [DisplayName("设置端口类型")]
    [Description("扫描枪或其它设备连接电脑的端口类型")]
    public class PackRecombineDevicePortConfig : ModuleConfig<DevicePortConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override DevicePortConfigValue DefaultValue { get; } = new DevicePortConfigValue { DevicePort = DevicePort.USB };
    }
}