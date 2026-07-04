using SIE.MES.BatchWIP.Products;

namespace SIE.Wpf.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次产品工艺路线变更事件
    /// </summary>
    internal class BatchWipProductRoutingEventViewConfig : WPFViewConfig<BatchWipProductRoutingEvent>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchWipProductRouting));
            View.Property(p => p.ChangeUserName).HasLabel("修改人");
            View.Property(p => p.ChangeDate).HasLabel("修改时间");
            View.Property(p => p.Remark).Show( ShowInWhere.All);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }
    }
}