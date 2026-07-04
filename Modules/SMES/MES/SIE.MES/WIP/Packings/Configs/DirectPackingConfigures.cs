using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Packings.Enums;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SIE.MES.WIP.Packings.Configs
{

    #region 端口类型配置
    /// <summary>
    /// 端口类型配置
    /// </summary>
    [System.ComponentModel.DisplayName("设置端口类型")]
    [System.ComponentModel.Description("扫描枪或其它设备连接电脑的端口类型")]
    public class DirectDevicePortConfig : ModuleCategoryConfig<ResourceStation, DirectDevicePortConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override DirectDevicePortConfigValue DefaultValue { get; } = new DirectDevicePortConfigValue { DevicePort = DevicePort.USB };
    }

    /// <summary>
    /// 端口类型值配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("串口参数值")]
    public class DirectDevicePortConfigValue : ConfigValue
    {
        #region 设备端口 DevicePort
        /// <summary>
        /// 设备端口
        /// </summary>
        [Label("设备端口")]
        public static readonly Property<DevicePort> DevicePortProperty = P<DirectDevicePortConfigValue>.Register(e => e.DevicePort);

        /// <summary>
        /// 设备端口
        /// </summary>
        public DevicePort DevicePort
        {
            get { return this.GetProperty(DevicePortProperty); }
            set { this.SetProperty(DevicePortProperty, value); }
        }
        #endregion

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
    public class DirectDevicePortConfigValueConfig : EntityConfig<DirectDevicePortConfigValue>
    {
        /// <summary>
        /// 增加实体验证规则
        /// </summary>
        /// <param name="rules">验证规格集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(DirectDevicePortConfigValue.DevicePortProperty, new RequiredRule());
        }
    }
    #endregion

    #region 通信串口参数配置
    /// <summary>
    /// 通信串口参数配置
    /// </summary>
    [System.ComponentModel.DisplayName("通信串口参数")]
    [System.ComponentModel.Description("电脑与扫描枪或其它硬件串口通信参数,可配置多个")]
    public class DirectSerialPortsConfig : ModuleCategoryConfig<ResourceStation, DirectSerialPortsConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override DirectSerialPortsConfigValue DefaultValue { get; } = new DirectSerialPortsConfigValue();
    }

    /// <summary>
    /// 通信串口参数值配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("串口参数值")]
    public class DirectSerialPortsConfigValue : ConfigValue
    {
        #region 通信串口列表 SerialPortList
        /// <summary>
        /// 通信串口列表
        /// </summary>
        [Label("通信串口列表")]
        public static readonly ListProperty<EntityList<DirectSerialPort>> SerialPortListProperty = P<DirectSerialPortsConfigValue>.RegisterList(e => e.SerialPortList);

        /// <summary>
        /// 通信串口列表
        /// </summary>
        public EntityList<DirectSerialPort> SerialPortList
        {
            get { return this.GetLazyList(SerialPortListProperty); }
        }
        #endregion

        /// <summary>
        /// 显示所有属性的名称
        /// </summary>
        /// <returns>返回所有属性的名称</returns>
        public override string Display()
        {
            StringBuilder result = new StringBuilder();
            foreach (var s in SerialPortList)
            {
                result.Append("[" + s.Display() + "]");
            }
            return result.ToString();
        }
    }

    /// <summary>
    /// 串口实体
    /// </summary>
    [Serializable, ChildEntity]
    [Label("串口参数值")]
    public class DirectSerialPort : ConfigValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DirectSerialPort()
        {
            PortName = PortName.COM1;
            BaudRate = 9600;
        }

        #region 通信串口参数 DirectSerialPortsConfigValue
        /// <summary>
        /// 通信串口参数Id
        /// </summary>
        [Label("通信串口参数")]
        public static readonly IRefIdProperty DirectSerialPortsConfigValueIdProperty =
            P<DirectSerialPort>.RegisterRefId(e => e.DirectSerialPortsConfigValueId, ReferenceType.Parent);

        /// <summary>
        /// 通信串口参数Id
        /// </summary>
        public double DirectSerialPortsConfigValueId
        {
            get { return (double)this.GetRefId(DirectSerialPortsConfigValueIdProperty); }
            set { this.SetRefId(DirectSerialPortsConfigValueIdProperty, value); }
        }

        /// <summary>
        /// 通信串口参数
        /// </summary>
        public static readonly RefEntityProperty<DirectSerialPortsConfigValue> DirectSerialPortsConfigValueProperty =
            P<DirectSerialPort>.RegisterRef(e => e.DirectSerialPortsConfigValue, DirectSerialPortsConfigValueIdProperty);

        /// <summary>
        /// 通信串口参数
        /// </summary>
        public DirectSerialPortsConfigValue DirectSerialPortsConfigValue
        {
            get { return this.GetRefEntity(DirectSerialPortsConfigValueProperty); }
            set { this.SetRefEntity(DirectSerialPortsConfigValueProperty, value); }
        }
        #endregion

        #region 端口名称 PortName
        /// <summary>
        /// 端口名称
        /// </summary>
        [Label("端口名称")]
        public static readonly Property<PortName> PortNameProperty = P<DirectSerialPort>.Register(e => e.PortName);

        /// <summary>
        /// 端口名称
        /// </summary>
        public PortName PortName
        {
            get { return this.GetProperty(PortNameProperty); }
            set { this.SetProperty(PortNameProperty, value); }
        }
        #endregion

        #region 波特率 BaudRate
        /// <summary>
        /// 波特率
        /// </summary>
        [Label("波特率")]
        public static readonly Property<int> BaudRateProperty = P<DirectSerialPort>.Register(e => e.BaudRate);

        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate
        {
            get { return this.GetProperty(BaudRateProperty); }
            set { this.SetProperty(BaudRateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示所有属性的名称
        /// </summary>
        /// <returns>返回所有属性的名称</returns>
        public override string Display()
        {
            return "{0}:{1};{2}:{3}".FormatArgs(DisplayHelper.Display(PortNameProperty), PortName, DisplayHelper.Display(BaudRateProperty), BaudRate);
        }
    }
    #endregion

    #region 称重设备通信串口参数配置值
    /// <summary>
    /// 称重设备通信串口参数
    /// </summary>
    [System.ComponentModel.DisplayName("称重设备通信串口参数")]
    [System.ComponentModel.Description("电脑与称重设备串口通信参数,可配置多个")]
    public class DirectWeightSerialProtsConfig : ModuleCategoryConfig<ResourceStation, DirectWeightSerialPortsConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override DirectWeightSerialPortsConfigValue DefaultValue { get; } = new DirectWeightSerialPortsConfigValue();

    }

    /// <summary>
    /// 称重设备通信串口参数配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("串口参数值")]
    public class DirectWeightSerialPortsConfigValue : ConfigValue
    {
        #region 串口列表 DirectWeightSerialPortList
        /// <summary>
        /// 串口列表
        /// </summary>
        [Label("串口列表")]
        public static readonly ListProperty<EntityList<DirectWeightSerialPort>> DirectWeightSerialPortListProperty = P<DirectWeightSerialPortsConfigValue>.RegisterList(e => e.DirectWeightSerialPortList);

        /// <summary>
        /// 串口列表
        /// </summary>
        public EntityList<DirectWeightSerialPort> DirectWeightSerialPortList
        {
            get { return this.GetLazyList(DirectWeightSerialPortListProperty); }
        }
        #endregion


        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>串口</returns>
        public override string Display()
        {
            StringBuilder result = new StringBuilder();
            foreach (var s in DirectWeightSerialPortList)
            {
                result.Append("[" + s.Display() + "]");
            }
            return result.ToString();
        }

        /// <summary>
        /// 称重设备通信串口参数配置值
        /// </summary>
        [Serializable, ChildEntity]
        [Label("串口参数值")]
        public class DirectWeightSerialPort : ConfigValue
        {
            /// <summary>
            /// 构造方法
            /// </summary>
            public DirectWeightSerialPort()
            {
                PortName = PortName.COM1;
                BaudRate = 9600;
            }

            #region 正则表达式 Regular
            /// <summary>
            /// 正则表达式，用于解析串口数据
            /// </summary>
            [Label("正则表达式")]
            public static readonly Property<string> RegularProperty = P<DirectWeightSerialPort>.Register(e => e.Regular);

            /// <summary>
            /// 正则表达式，用于解析串口数据
            /// </summary>
            public string Regular
            {
                get { return this.GetProperty(RegularProperty); }
                set { this.SetProperty(RegularProperty, value); }
            }
            #endregion

            #region 串行端口配置值 SerialPortsConfigValue
            /// <summary>
            /// 串行端口配置值ID
            /// </summary>
            public static readonly IRefIdProperty SerialPortsConfigValueIdProperty =
            P<DirectWeightSerialPort>.RegisterRefId(e => e.SerialPortsConfigValueId, ReferenceType.Parent);

            /// <summary>
            /// 串行端口配置值ID
            /// </summary>
            public double SerialPortsConfigValueId
            {
                get { return (double)this.GetRefId(SerialPortsConfigValueIdProperty); }
                set { this.SetRefId(SerialPortsConfigValueIdProperty, value); }
            }

            /// <summary>
            /// 串行端口配置值
            /// </summary>
            public static readonly RefEntityProperty<DirectWeightSerialPortsConfigValue> SerialPortsConfigValueProperty =
                P<DirectWeightSerialPort>.RegisterRef(e => e.SerialPortsConfigValue, SerialPortsConfigValueIdProperty);

            /// <summary>
            /// 串行端口配置值
            /// </summary>
            public DirectWeightSerialPortsConfigValue SerialPortsConfigValue
            {
                get { return this.GetRefEntity(SerialPortsConfigValueProperty); }
                set { this.SetRefEntity(SerialPortsConfigValueProperty, value); }
            }

            #endregion

            #region 串口名称 PortName
            /// <summary>
            /// 串口名称
            /// </summary>
            [Label("串口名称")]
            public static readonly Property<PortName> PortNameProperty = P<DirectWeightSerialPort>.Register(e => e.PortName);

            /// <summary>
            /// 串口名称
            /// </summary>
            public PortName PortName
            {
                get { return this.GetProperty(PortNameProperty); }
                set { this.SetProperty(PortNameProperty, value); }
            }
            #endregion

            #region 波特率 BaudRate
            /// <summary>
            /// 波特率
            /// </summary>
            [Label("波特率")]
            public static readonly Property<int> BaudRateProperty = P<DirectWeightSerialPort>.Register(e => e.BaudRate);

            /// <summary>
            /// 波特率
            /// </summary>
            public int BaudRate
            {
                get { return this.GetProperty(BaudRateProperty); }
                set { this.SetProperty(BaudRateProperty, value); }
            }
            #endregion

            /// <summary>
            /// 显示值
            /// </summary>
            /// <returns>串口名称</returns>
            public override string Display()
            {
                return "{0}:{1};{2}:{3}".FormatArgs(DisplayHelper.Display(PortNameProperty), PortName, DisplayHelper.Display(BaudRateProperty), BaudRate);
            }
        }
    }
    #endregion

}
