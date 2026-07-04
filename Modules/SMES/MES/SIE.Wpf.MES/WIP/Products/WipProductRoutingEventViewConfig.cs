using SIE.MES.WIP.Products;

namespace SIE.Wpf.MES.WIP.Products
{
    /// <summary>
    /// 产品维修记录视图配置
    /// </summary>
    internal class WipProductRoutingEventViewConfig : WPFViewConfig<WipProductRoutingEvent>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductRouting));
            View.Property(p => p.ChangeUserName);
            View.Property(p => p.ChangeDate).HasLabel("修改时间");
            View.Property(p => p.Remark);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }
    }
}