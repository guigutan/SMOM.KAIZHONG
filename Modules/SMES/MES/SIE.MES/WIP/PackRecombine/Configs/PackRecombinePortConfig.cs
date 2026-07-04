using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.WIP.Configs;
using SIE.ObjectModel;
using System;
using System.ComponentModel;
using System.Text;

namespace SIE.MES.WIP.PackRecombine.Configs
{
    /// <summary>
    /// 包装拆合通信串口参数配置
    /// </summary>
    [DisplayName("包装拆合通信串口参数配置")]
    [Description("电脑与扫描枪或其它硬件串口通信参数")]
    public class PackRecombinePortConfig : ModuleConfig<PackRecombinePortConfigValue>
    {
        /// <summary>
        /// 通信串口参数默认值
        /// </summary>
        public override PackRecombinePortConfigValue DefaultValue { get; } = new PackRecombinePortConfigValue();
    }

    /// <summary>
    /// 串口参数配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("串口参数值")]
    public class PackRecombinePortConfigValue : ConfigValue
    {
        #region 通信串口列表 SerialPortList
        /// <summary>
        /// 通信串口列表
        /// </summary>
        [Label("通信串口列表")]
        public static readonly ListProperty<EntityList<PackRecombineSerialPort>> SerialPortListProperty = P<PackRecombinePortConfigValue>.RegisterList(e => e.SerialPortList);

        /// <summary>
        /// 通信串口列表
        /// </summary>
        public EntityList<PackRecombineSerialPort> SerialPortList
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
                result.Append( "[" + s.Display() + "]");
            return result.ToString();
        }
    }

    /// <summary>
    /// 串口参数
    /// </summary>
    [ChildEntity, Serializable]
    public class PackRecombineSerialPort : ConfigValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PackRecombineSerialPort()
        {
            PortName = PortName.COM1;
            BaudRate = 9600;
        }

        #region 串口参数 PackRecombinePort
        /// <summary>
        /// 串口参数Id
        /// </summary>
        [Label("串口参数")]
        public static readonly IRefIdProperty PackRecombinePortIdProperty =
            P<PackRecombineSerialPort>.RegisterRefId(e => e.PackRecombinePortId, ReferenceType.Parent);

        /// <summary>
        /// 串口参数Id
        /// </summary>
        public double PackRecombinePortId
        {
            get { return (double)this.GetRefId(PackRecombinePortIdProperty); }
            set { this.SetRefId(PackRecombinePortIdProperty, value); }
        }

        /// <summary>
        /// 串口参数
        /// </summary>
        public static readonly RefEntityProperty<PackRecombinePortConfigValue> PackRecombinePortProperty =
            P<PackRecombineSerialPort>.RegisterRef(e => e.PackRecombinePort, PackRecombinePortIdProperty);

        /// <summary>
        /// 串口参数
        /// </summary>
        public PackRecombinePortConfigValue PackRecombinePort
        {
            get { return this.GetRefEntity(PackRecombinePortProperty); }
            set { this.SetRefEntity(PackRecombinePortProperty, value); }
        }
        #endregion 

        #region 端口名称 PortName
        /// <summary>
        /// 串口名称
        /// </summary>
        [Label("端口名称")]
        public static readonly Property<PortName> PortNameProperty = P<PackRecombineSerialPort>.Register(e => e.PortName);

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
        public static readonly Property<int> BaudRateProperty = P<PackRecombineSerialPort>.Register(e => e.BaudRate);

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
            return "端口名称:{0};波特率:{1}".L10nFormat(PortName, BaudRate);
        }
    }
}