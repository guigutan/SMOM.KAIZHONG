using SIE.ERPInterface.Common.Logs;

namespace SIE.Web.ERPInterface.Logs
{
    /// <summary>
    /// 任务下载时间明细视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class DownloadJobTimeDetailViewConfig : WebViewConfig<DownloadJobTimeDetail>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.ERPInterface.Logs.Commands.RequestContextCommand");
            View.UseCommands("SIE.Web.ERPInterface.Logs.Commands.ResponseContextCommand");
            View.AddBehavior("SIE.Web.ERPInterface.Scripts.DownloadJobTimeDetailQueryBehavior");
            View.FormEdit();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.ErpBatchId).Readonly().HasLabel("ID");
            View.Property(p => p.RequestStr).Readonly();
            View.Property(p => p.ResponseStr).Readonly();           
            View.Property(p => p.RequestDate).Readonly();
            View.Property(p => p.ResponseDate).Readonly();
            View.Property(p => p.ResponseCode).Readonly();
            View.Property(p => p.ResponseMessage).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}