using SIE.MES.TaskManagement.Reports;

namespace SIE.Web.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工批次标签视图配置
    /// </summary>
    internal class ReportWipBatchViewConfig : WebViewConfig<ReportWipBatch>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置视图
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(ReportDispatchTask), typeof(ReportRecordExamine));
            View.Property(p => p.BatchNo).Readonly().ShowInList(150).UseTextEditor(p => p.ColumnXType = "setReportWipBatchBatchColumnColorStyle");
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }
    }
}