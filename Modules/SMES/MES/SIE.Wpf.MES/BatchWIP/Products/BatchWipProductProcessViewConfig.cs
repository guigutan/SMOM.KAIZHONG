using SIE.MES.BatchWIP.Products;

namespace SIE.WPF.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次采集记录视图配置
    /// </summary>
    internal class BatchWipProductProcessViewConfig : WPFViewConfig<BatchWipProductProcess>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchWipProductVersion));
            View.UseLayoutSize(-4, -6);
            View.ClearCommands();
            View.Property(p => p.ResourceName);
            View.Property(p => p.ProcessName);
            View.Property(p => p.InputQty);
            View.Property(p => p.OutputQty);
            View.Property(p => p.InputDate);
            View.Property(p => p.OutputDate);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.DetailList).HasLabel("采集明细").Visible(false);

            View.AttachChildrenProperty(typeof(BatchWipProductProcessDetail), (o) =>
            {
                var args = o as ChildPagingDataArgs;
                var entity = args.Parent as BatchWipProductProcess;
                return RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductProcessInDetail(entity.Id, args.PagingInfo);
            });
        }
    }
}
