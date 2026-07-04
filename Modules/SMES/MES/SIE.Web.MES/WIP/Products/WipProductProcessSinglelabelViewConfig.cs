using SIE.MES.WIP.Products;

namespace SIE.Web.MES.WIP.Products
{
    /// <summary>
    /// 上料采集单体条码视图配置
    /// </summary>
    internal class WipProductProcessSinglelabelViewConfig : WebViewConfig<WipProductProcessSinglelabel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductVersion));
            View.ClearCommands();
            View.Property(p => p.SingleLabel).ShowInList(200).Readonly();
            View.Property(p => p.IsExternal).Readonly();
            View.Property(p => p.CreateDate).HasLabel("操作时间").Readonly().ShowInList(150);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            
        }
    }
}
