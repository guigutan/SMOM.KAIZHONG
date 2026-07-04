using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Linq;

namespace SIE.MES.WIP.Configs
{
    /// <summary>
    /// 端口类型配置
    /// </summary>
    [System.ComponentModel.DisplayName("设置端口类型")]
    [System.ComponentModel.Description("扫描枪或其它设备连接电脑的端口类型")]
    public class DevicePortConfig : ModuleCategoryConfig<ResourceStation, DevicePortConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override DevicePortConfigValue DefaultValue { get; } = new DevicePortConfigValue { DevicePort = DevicePort.USB };
    }

    /// <summary>
    /// 端口类型值配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("串口参数值")]
    public class DevicePortConfigValue : ConfigValue
    {
        /// <summary>
        /// 设备端口
        /// </summary>
        [Label("设备端口")]
        public static readonly Property<DevicePort> DevicePortProperty = P<DevicePortConfigValue>.Register(e => e.DevicePort);

        /// <summary>
        /// 设备端口
        /// </summary>
        public DevicePort DevicePort
        {
            get { return this.GetProperty(DevicePortProperty); }
            set { this.SetProperty(DevicePortProperty, value); }
        }

        /// <summary>
        /// 显示所有属性的名称
        /// </summary>
        /// <returns>返回所有属性的名称</returns>
        public override string Display()
        {
            string msg = null;
            var thisType = GetType();
            PropertyContainer.GetProperties().Where(p => p.OwnerType == thisType)
                .ForEach(p =>
                {
                    msg += DisplayHelper.Display(p) + ":" + GetProperty(p) + ",";
                });

            return msg.TrimEnd(',');
        }
    }

    /// <summary>
    /// 端口类型实体配置
    /// </summary>
    class DevicePortConfigValueConfig : EntityConfig<DevicePortConfigValue>
    {
        /// <summary>
        /// 增加实体验证规则
        /// </summary>
        /// <param name="rules">验证规格集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(DevicePortConfigValue.DevicePortProperty, new RequiredRule());
        }
    }

    /// <summary>
    /// 设备端口
    /// </summary>
    public enum DevicePort
    {
        /// <summary>
        /// USB
        /// </summary>
        [Label("USB")]
        USB,

        /// <summary>
        /// 串口
        /// </summary>
        [Label("串口")]
        Serial,
    }

    /// <summary>
    /// 串口名称
    /// </summary>
    public enum PortName
    {
        /// <summary>
        /// COM1
        /// </summary>
        [Label("COM1")]
        COM1 = 1,

        /// <summary>
        /// COM2
        /// </summary>
        [Label("COM2")]
        COM2 = 2,

        /// <summary>
        /// COM3
        /// </summary>
        [Label("COM3")]
        COM3 = 3,

        /// <summary>
        /// COM4
        /// </summary>
        [Label("COM4")]
        COM4 = 4,

        /// <summary>
        /// COM5
        /// </summary>
        [Label("COM5")]
        COM5 = 5,

        /// <summary>
        /// COM6
        /// </summary>
        [Label("COM6")]
        COM6 = 6,

        /// <summary>
        /// COM7
        /// </summary>
        [Label("COM7")]
        COM7 = 7,

        /// <summary>
        /// COM8
        /// </summary>
        [Label("COM8")]
        COM8 = 8,

        /// <summary>
        /// COM9
        /// </summary>
        [Label("COM9")]
        COM9 = 9
    }
}
