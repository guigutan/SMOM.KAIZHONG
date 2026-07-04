using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.WIP.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Text;

namespace SIE.MES.WIP.Packings.Configs
{
    /// <summary>
    /// 称重设备通信串口参数
    /// </summary>
    [System.ComponentModel.DisplayName("称重设备通信串口参数")]
    [System.ComponentModel.Description("电脑与称重设备串口通信参数,可配置多个")]
    public class WeightSerialProtsConfig : ModuleCategoryConfig<ResourceStation, WeightSerialPortsConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override WeightSerialPortsConfigValue DefaultValue { get; } = new WeightSerialPortsConfigValue();
    }

    /// <summary>
    /// 称重设备通信串口参数配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("串口参数值")]
    public class WeightSerialPortsConfigValue : ConfigValue
    {
        #region 串口列表 SerialPortList
        /// <summary>
        /// 串口列表
        /// </summary>
        [Label("串口列表")]
        public static readonly ListProperty<EntityList<WeightSerialPort>> SerialPortListProperty = P<WeightSerialPortsConfigValue>.RegisterList(e => e.SerialPortList);

        /// <summary>
        /// 串口列表
        /// </summary>
        public EntityList<WeightSerialPort> SerialPortList
        {
            get { return this.GetLazyList(SerialPortListProperty); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>串口</returns>
        public override string Display()
        {
            StringBuilder result = new StringBuilder();
            foreach (var s in SerialPortList)
                result .Append("[" + s.Display() + "]");
            return result.ToString();
        }
    }

    /// <summary>
    /// 称重设备通信串口参数配置值
    /// </summary>
    [Serializable, ChildEntity]
    [Label("串口参数值")]
    public class WeightSerialPort : ConfigValue
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public WeightSerialPort()
        {
            PortName = PortName.COM1;
            BaudRate = 9600;
        }

        #region 正则表达式 Regular
        /// <summary>
        /// 正则表达式，用于解析串口数据
        /// </summary>
        [Label("正则表达式")]
        public static readonly Property<string> RegularProperty = P<WeightSerialPort>.Register(e => e.Regular);

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
        P<WeightSerialPort>.RegisterRefId(e => e.SerialPortsConfigValueId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<WeightSerialPortsConfigValue> SerialPortsConfigValueProperty =
            P<WeightSerialPort>.RegisterRef(e => e.SerialPortsConfigValue, SerialPortsConfigValueIdProperty);

        /// <summary>
        /// 串行端口配置值
        /// </summary>
        public WeightSerialPortsConfigValue SerialPortsConfigValue
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
        public static readonly Property<PortName> PortNameProperty = P<WeightSerialPort>.Register(e => e.PortName);

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
        public static readonly Property<int> BaudRateProperty = P<WeightSerialPort>.Register(e => e.BaudRate);

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