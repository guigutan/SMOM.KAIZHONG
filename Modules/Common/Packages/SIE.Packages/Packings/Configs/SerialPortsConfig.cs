using SIE.Common.Configs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.Warehouses;
using System;
using System.Text;

namespace SIE.Packages.Configs
{
    /// <summary>
    /// 通信串口参数配置
    /// </summary>
    [System.ComponentModel.DisplayName("打包功能通信串口参数")]
    [System.ComponentModel.Description("电脑与扫描枪或其它硬件串口通信参数,可配置多个")]
    public class SerialPortsConfig : ModuleCategoryConfig<Warehouse, SerialPortsConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override SerialPortsConfigValue DefaultValue { get; } = new SerialPortsConfigValue();
    }

    /// <summary>
    /// 通信串口参数值配置
    /// </summary>
    [RootEntity, Serializable]
    public class SerialPortsConfigValue : ConfigValue
    {
        /// <summary>
        /// 串口列表
        /// </summary>
        public static readonly ListProperty<EntityList<SerialPort>> SerialPortListProperty = P<SerialPortsConfigValue>.RegisterList(e => e.SerialPortList);

        /// <summary>
        /// 串口列表
        /// </summary>
        public EntityList<SerialPort> SerialPortList
        {
            get { return this.GetLazyList(SerialPortListProperty); }
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>串口</returns>
        public override string Display()
        {
            var result = new StringBuilder();
            foreach (var st in SerialPortList)
            {
                result.Append("[");
                result.Append(st.Display());
                result.Append("]");
            }
            return result.ToString();
        }
    }

    /// <summary>
    /// 串口
    /// </summary>
    [Serializable, ChildEntity]
    public class SerialPort : ConfigValue
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public SerialPort()
        {
            PortName = PortName.COM1;
            BaudRate = 9600;
        }

        /// <summary>
        /// 串口配置值ID
        /// </summary>
        public static readonly IRefIdProperty SerialPortsConfigValueIdProperty =
        P<SerialPort>.RegisterRefId(e => e.SerialPortsConfigValueId, ReferenceType.Parent);

        /// <summary>
        /// 串口配置值ID
        /// </summary>
        public double SerialPortsConfigValueId
        {
            get { return (double)this.GetRefId(SerialPortsConfigValueIdProperty); }
            set { this.SetRefId(SerialPortsConfigValueIdProperty, value); }
        }

        /// <summary>
        /// 串口配置值
        /// </summary>
        public static readonly RefEntityProperty<SerialPortsConfigValue> SerialPortsConfigValueProperty =
            P<SerialPort>.RegisterRef(e => e.SerialPortsConfigValue, SerialPortsConfigValueIdProperty);

        /// <summary>
        /// 串口配置值
        /// </summary>
        public SerialPortsConfigValue SerialPortsConfigValue
        {
            get { return this.GetRefEntity(SerialPortsConfigValueProperty); }
            set { this.SetRefEntity(SerialPortsConfigValueProperty, value); }
        }

        /// <summary>
        /// 串口名称
        /// </summary>
        public static readonly Property<PortName> PortNameProperty = P<SerialPort>.Register(e => e.PortName);

        /// <summary>
        /// 串口名称
        /// </summary>
        public PortName PortName
        {
            get { return this.GetProperty(PortNameProperty); }
            set { this.SetProperty(PortNameProperty, value); }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        public static readonly Property<int> BaudRateProperty = P<SerialPort>.Register(e => e.BaudRate);

        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate
        {
            get { return this.GetProperty(BaudRateProperty); }
            set { this.SetProperty(BaudRateProperty, value); }
        }

        /// <summary>
        /// 显示值
        /// </summary>
        /// <returns> 波特率</returns>
        public override string Display()
        {
            return "{0}:{1};{2}:{3}".FormatArgs(DisplayHelper.Display(PortNameProperty), PortName, DisplayHelper.Display(BaudRateProperty), BaudRate);
        }
    }
}
