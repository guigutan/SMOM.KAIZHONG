using SIE.MES.WIP.Packings.Configs;

namespace SIE.Wpf.MES.WIP.Configs
{
    /// <summary>
    /// 通信串口参数视图配置
    /// </summary>
    class WeightSerialPortsConfigValueViewConfig : WPFViewConfig<WeightSerialPortsConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            View.AddBehavior(typeof(SerialPortsViewBehavior));
            View.UseDetail(dialogHeight: 500, dialogWidth: 1000);
            using (View.OrderProperties())
            {
                View.ChildrenProperty(p => p.SerialPortList).Show(ChildShowInWhere.Detail).Readonly();
            }
        }
    }

    /// <summary>
    /// 称重设备通信串口视图配置
    /// </summary>
    class WeightSerialPortViewConfig : WPFViewConfig<WeightSerialPort>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            using (View.OrderProperties())
            {
                View.Property(p => p.PortName).Show(ShowInWhere.All);
                View.Property(p => p.BaudRate).Show(ShowInWhere.All);
                View.Property(p => p.Regular).Show(ShowInWhere.All);
            }
        }
    }
}