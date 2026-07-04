using SIE.MES.WIP.PackRecombine.Configs;

namespace SIE.Wpf.MES.WIP.Configs
{
    /// <summary>
    /// 包装拆分串口配置视图
    /// </summary>
    internal class PackRecombinePortConfigValueViewConfig : WPFViewConfig<PackRecombinePortConfigValue>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.ChildrenProperty(p => p.SerialPortList).Show(ChildShowInWhere.Detail).Readonly();
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.ChildrenProperty(p => p.SerialPortList).Show(ChildShowInWhere.Detail).Readonly();
        }
    }

    /// <summary>
    /// 串口实体 视图配置
    /// </summary>
    internal class PackRecombineSerialPortViewConfig : WPFViewConfig<PackRecombineSerialPort>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.PortName).Show(ShowInWhere.All);
            View.Property(p => p.BaudRate).Show(ShowInWhere.All);
        }
    }
}