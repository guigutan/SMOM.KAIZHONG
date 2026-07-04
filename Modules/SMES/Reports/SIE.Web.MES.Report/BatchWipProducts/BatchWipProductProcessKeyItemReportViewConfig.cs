using SIE.MES.Report.BatchWipProducts;

namespace SIE.Web.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 产品生产关键件视图配置
    /// </summary>
    internal class BatchWipProductProcessKeyItemReportViewConfig : WebViewConfig<BatchWipProductProcessKeyItemReport>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchWipProductVersionReport));
            View.ClearCommands();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.StationName).Readonly();
            View.Property(p => p.SourceCode).Readonly();
            View.Property(p => p.SourceType).Readonly().UseEnumEditor();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.SingleQty).Readonly();
            View.Property(p => p.ItemCode).Readonly();
            View.Property(p => p.ItemName).Readonly();
            View.Property(p => p.ItemDesc).Readonly();
            View.Property(p => p.ItemNnitName).Readonly();
            View.Property(p => p.CreateDate).Readonly().HasLabel("操作时间").ShowInList(width: 150);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.ValueList).Show(ChildShowInWhere.Hide);
        }
    }
}
