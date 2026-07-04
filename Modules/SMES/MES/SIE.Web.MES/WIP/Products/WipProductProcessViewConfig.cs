using SIE.MES.WIP.Products;

namespace SIE.Web.MES.WIP.Products
{
    /// <summary>
    /// 生产采集记录视图配置
    /// </summary>
    internal class WipProductProcessViewConfig : WebViewConfig<WipProductProcess>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductVersion));
            View.ClearCommands();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.OperateTime).Readonly().ShowInList(150);
            View.Property(p => p.ShiftId).Readonly().ShowInList(100);
            View.Property(p => p.Result).Readonly();
            View.Property(p => p.StationName).Readonly();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.EmployeeName).Readonly();
            View.Property(p => p.Barcode).Readonly().ShowInList(150);
            View.Property(p => p.InInning).Readonly().ShowInList(150);
            
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}