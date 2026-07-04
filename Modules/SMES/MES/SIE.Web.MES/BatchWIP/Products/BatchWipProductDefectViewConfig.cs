using SIE.MES.BatchWIP.Products;

namespace SIE.Web.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品缺陷记录视图配置
    /// </summary>
    internal class BatchWipProductDefectViewConfig : WebViewConfig<BatchWipProductDefect>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchWipProductVersion), typeof(BatchWipProductRouting));
            View.ClearCommands();
            View.Property(p => p.BatchNo).Readonly();
            View.Property(p => p.SubBatchNo).Readonly().Show(ShowInWhere.Hide);
            View.Property(p => p.ContainerNo).Readonly();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.StationName).Readonly();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.Location).Readonly();
            View.Property(p => p.FixedByName).Readonly();
            View.Property(p => p.FixedDate).Readonly();
            View.Property(p => p.Remark).Readonly();
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
