using SIE.ERPInterface.Common.Logs;

namespace SIE.Web.ERPInterface.Logs
{
    /// <summary>
    /// 任务下载记录视图配置
    /// </summary>
    internal class DownloadJobLogViewConfig : WebViewConfig<DownloadJobLog>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.BeginDate).Readonly();
            View.Property(p => p.EndDate).Readonly();
            View.Property(p => p.DataCount).Readonly();
            View.Property(p => p.SuccessCount).Readonly();
            View.Property(p => p.FailCount).Readonly();
            View.Property(p => p.Remark).Readonly().ShowInList(width: 200);
            View.Property(p => p.InfId).Readonly();
            View.Property(p => p.ErpKey).Readonly();
            View.Property(p => p.OperationType).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.JobMode).Readonly();
            View.Property(p => p.JobType).Readonly();
            View.Property(p => p.JobDirection).Readonly();
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.InfId);
            View.Property(p => p.ErpKey);
            View.Property(p => p.OperationType);
            View.Property(p => p.State);
            View.Property(p => p.JobMode);
            View.Property(p => p.JobType);
            View.Property(p => p.JobDirection);
            View.Property(p => p.BeginDate);
            View.Property(p => p.EndDate);
        }
    }
}