using SIE.MES.BatchWIP.Products;

namespace SIE.WPF.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品缺陷记录视图配置
    /// </summary>
    internal class BatchWipProductDefectViewConfig : WPFViewConfig<BatchWipProductDefect>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchWipProductVersion), typeof(BatchWipProductRouting));
            View.ClearCommands();
            View.Property(p => p.BatchNo);
            View.Property(p => p.SubBatchNo);
            View.Property(p => p.ContainerNo);
            View.Property(p => p.Qty);
            View.Property(p => p.StationName);
            View.Property(p => p.ProcessName);
            View.Property(p => p.ResourceName);
            View.Property(p => p.Location);
            View.Property(p => p.FixedByName);
            View.Property(p => p.FixedDate);
            View.Property(p => p.Remark);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.DetailList).HasLabel("缺陷代码");
            View.ChildrenProperty(p => p.ResponsibilityList).HasLabel("缺陷责任");
            View.ChildrenProperty(p => p.MeasureList).HasLabel("维修措施");
        }
    }
}
