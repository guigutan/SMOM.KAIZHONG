using SIE.ERPInterface.Common.Logs;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Logs
{
    /// <summary>
    /// 任务下载时间视图配置
    /// </summary>
    internal class DownloadJobTimeViewConfig : WebViewConfig<DownloadJobTime>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.JobType).Readonly();
            View.Property(p => p.JobCate).Readonly();
            View.Property(p => p.LastDownloadDate).Readonly();
            View.ChildrenProperty(p => p.DetailList);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.LastDownloadDate);
            View.Property(p => p.JobType);
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.JobType);
        }
    }
}