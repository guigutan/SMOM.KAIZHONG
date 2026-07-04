using DevExpress.Xpf.LayoutControl;
using SIE.MES.WIP.Configs;
using System.Windows;

namespace SIE.Wpf.MES.WIP.Configs
{
    /// <summary>
    /// 通信串口参数视图配置
    /// </summary>
    class SerialPortsConfigValueViewConfig : WPFViewConfig<SerialPortsConfigValue>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.AddBehavior(typeof(SerialPortsViewBehavior));
            View.UseDetail(dialogHeight: 500, dialogWidth: 800);
            View.ChildrenProperty(p => p.SerialPortList).Show(ChildShowInWhere.Detail);
        }
    }

    /// <summary>
    /// 串口实体 视图配置
    /// </summary>
    class SerialPortViewConfig : WPFViewConfig<SerialPort>
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
    public class SerialPortsViewBehavior : ViewBehavior
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
}