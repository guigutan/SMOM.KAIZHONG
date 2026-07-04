using SIE.ERPInterface.Common.Logs;

namespace SIE.Web.ERPInterface.Logs
{
    /// <summary>
    /// 事务上传记录明细视图配置
    /// </summary>
    internal class UploadTransactionLogDtlViewConfig : WebViewConfig<UploadTransactionLogDtl>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.UploadTransactionId).HasLabel("事务ID").Readonly().ShowInList(width: 100);
            View.Property(p => p.ProcessState).Readonly();
            View.Property(p => p.ValidateMessage).Readonly().ShowInList(width: 400);
            View.Property(p => p.ProcessMessage).Readonly().ShowInList(width: 200);

            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}