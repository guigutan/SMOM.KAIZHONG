using SIE.MES.Report.BatchWipProducts;

namespace SIE.Web.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 批次采集记录视图配置
    /// </summary>
    internal class BatchWipProductReportProcessViewConfig : WebViewConfig<BatchWipProductReportProcess>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.RemoveBehavior("SIE.Web.MES.BatchWip.BatchWipProductProcessBehavior");
            View.AddBehavior("SIE.Web.MES.Report.BatchWipProducts.BatchWipProductProcessBehavior");
            View.AssignAuthorize(typeof(BatchWipProductVersionReport));
            View.UseLayoutSize(-4, -6);
            View.ClearCommands();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.InputQty).Readonly();
            View.Property(p => p.OutputQty).Readonly();
            View.Property(p => p.InputDate).Readonly();
            View.Property(p => p.OutputDate).Readonly();
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.DetailList).HasLabel("采集明细").Visible(false);
        }
    }
}
