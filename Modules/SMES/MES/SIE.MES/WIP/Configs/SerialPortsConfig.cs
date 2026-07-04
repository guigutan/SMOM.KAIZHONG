using SIE.Common.Configs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Text;

namespace SIE.MES.WIP.Configs
{
    /// <summary>
    /// 通信串口参数配置
    /// </summary>
    [System.ComponentModel.DisplayName("通信串口参数")]
    [System.ComponentModel.Description("电脑与扫描枪或其它硬件串口通信参数,可配置多个")]
    public class SerialPortsConfig : ModuleCategoryConfig<ResourceStation, SerialPortsConfigValue>
    {
        /// <summary>
        /// 通信串口参数默认值
        /// </summary>
        public override SerialPortsConfigValue DefaultValue { get; } = new SerialPortsConfigValue();
    }

    /// <summary>
    /// 通信串口参数值配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("串口参数值")]
    public class SerialPortsConfigValue : ConfigValue
    {
        /// <summary>
        /// 通信串口列表
        /// </summary>
        [Label("通信串口列表")]
        public static readonly ListProperty<EntityList<SerialPort>> SerialPortListProperty = P<SerialPortsConfigValue>.RegisterList(e => e.SerialPortList);

        /// <summary>
        /// 通信串口列表
        /// </summary>
        public EntityList<SerialPort> SerialPortList
        {
            get { return this.GetLazyList(SerialPortListProperty); }
        }

        /// <summary>
        /// 显示所有属性的名称
        /// </summary>
        /// <returns>返回所有属性的名称</returns>
        public override string Display()
        {
            StringBuilder result = new StringBuilder();
            foreach (var s in SerialPortList)
                result.Append("[" + s.Display() + "]");
            return result.ToString();
        }
    }

    /// <summary>
    /// 串口实体
    /// </summary>
    [Serializable, ChildEntity]
    [Label("串口参数值")]
    public class SerialPort : ConfigValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SerialPort()
        {
            PortName = PortName.COM1;
            BaudRate = 9600;
        }

        #region 通信串口参数
        /// <summary>
        /// 通信串口参数ID
        /// </summary>
        [Label("通信串口参数")]
        public static readonly IRefIdProperty SerialPortsConfigValueIdProperty =
        P<SerialPort>.RegisterRefId(e => e.SerialPortsConfigValueId, ReferenceType.Parent);

        /// <summary>
        /// 通信串口参数id
        /// </summary>
        public double SerialPortsConfigValueId
        {
            get { return (double)this.GetRefId(SerialPortsConfigValueIdProperty); }
            set { this.SetRefId(SerialPortsConfigValueIdProperty, value); }
        }

        /// <summary>
        /// 通信串口参数
        /// </summary>
        public static readonly RefEntityProperty<SerialPortsConfigValue> SerialPortsConfigValueProperty =
            P<SerialPort>.RegisterRef(e => e.SerialPortsConfigValue, SerialPortsConfigValueIdProperty);

        /// <summary>
        /// 通信串口参数
        /// </summary>
        public SerialPortsConfigValue SerialPortsConfigValue
        {
            get { return this.GetRefEntity(SerialPortsConfigValueProperty); }
            set { this.SetRefEntity(SerialPortsConfigValueProperty, value); }
        }
        #endregion

        #region 端口名称
        /// <summary>
        /// 串口名称
        /// </summary>
        [Label("端口名称")]
        public static readonly Property<PortName> PortNameProperty = P<SerialPort>.Register(e => e.PortName);

        /// <summary>
        /// 串口名称
        /// </summary>
        public PortName PortName
        {
            get { return this.GetProperty(PortNameProperty); }
            set { this.SetProperty(PortNameProperty, value); }
        }
        #endregion

        #region 波特率
        /// <summary>
        /// 波特率
        /// </summary>
        [Label("波特率")]
        public static readonly Property<int> BaudRateProperty = P<SerialPort>.Register(e => e.BaudRate);

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
}
