using DevExpress.Xpf.LayoutControl;
using SIE.MES.WIP.Packings.Configs;
using SIE.Wpf.MES.WIP.Configs;
using System.Windows;
using static SIE.MES.WIP.Packings.Configs.DirectWeightSerialPortsConfigValue;

namespace SIE.Wpf.MES.WIP.Packings.Configs
{

    /// <summary>
    /// 端口类型值 视图配置
    /// </summary>
    public class DirectDevicePortConfigValueViewConfig : WPFViewConfig<DirectDevicePortConfigValue>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.DevicePort).ShowInDetail();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.DevicePort);
        }
    }

    #region 通信串口相关视图配置
    /// <summary>
    /// 通信串口参数视图配置
    /// </summary>
    public class DirectSerialPortsConfigValueViewConfig : WPFViewConfig<DirectSerialPortsConfigValue>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.AddBehavior(typeof(DirectSerialPortsViewBehavior));
            View.UseDetail(dialogHeight: 500, dialogWidth: 800);
            View.ChildrenProperty(p => p.SerialPortList).Show(ChildShowInWhere.Detail);
        }
    }

    /// <summary>
    /// 串口实体 视图配置
    /// </summary>
    public class SerialPortViewConfig : WPFViewConfig<DirectSerialPort>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            View.Property(p => p.PortName).Show(ShowInWhere.All);
            View.Property(p => p.BaudRate).Show(ShowInWhere.All);
        }
    }

    /// <summary>
    /// 串口配置视图行为
    /// </summary>
    public class DirectSerialPortsViewBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加视图行为
        /// </summary>
        protected override void OnAttach()
        {
            var control = View.Control as LayoutControl;
            if (control != null)
                control.Margin = new Thickness(-30); //隐藏掉主表空白区域
        }
    }
    #endregion

    #region 通信串口参数视图配置
    /// <summary>
    /// 通信串口参数视图配置
    /// </summary>
    public class DirectWeightSerialPortsConfigValueViewConfig : WPFViewConfig<DirectWeightSerialPortsConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            View.AddBehavior(typeof(DirectSerialPortsViewBehavior));
            View.UseDetail(dialogHeight: 500, dialogWidth: 1000);
            using (View.OrderProperties())
            {
                View.ChildrenProperty(p => p.DirectWeightSerialPortList).Show(ChildShowInWhere.Detail).Readonly();
            }
        }
    }
    /// <summary>
    /// 称重设备通信串口视图配置
    /// </summary>
    public class DirectWeightSerialPortViewConfig : WPFViewConfig<DirectWeightSerialPort>
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
    #endregion

}
