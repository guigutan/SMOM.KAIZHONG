using SIE.MES.WIP.Products;

namespace SIE.Wpf.MES.WIP.Products
{
    /// <summary>
    /// 生产采集记录视图配置
    /// </summary>
    internal class WipProductProcessViewConfig : WPFViewConfig<WipProductProcess>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductVersion));
            View.ClearCommands();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.OperateTime).Readonly().ShowInList(gridWidth: 150);
            View.Property(p => p.Result).Readonly();
            View.Property(p => p.StationName).Readonly();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.EmployeeName).Readonly();
            View.Property(p => p.Barcode).Readonly();
        }
    }
}