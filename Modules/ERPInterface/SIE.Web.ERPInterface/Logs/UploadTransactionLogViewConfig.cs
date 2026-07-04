using SIE.ERPInterface.Common.Logs;

namespace SIE.Web.ERPInterface.Logs
{
    /// <summary>
    /// 事务上传记录视图配置
    /// </summary>
    internal class UploadTransactionLogViewConfig : WebViewConfig<UploadTransactionLog>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.ErpBatchId).Readonly().ShowInList(width: 120);
            View.Property(p => p.ResponseCode).Readonly().ShowInList(width: 250);
            View.Property(p => p.ResponseMessage).Readonly().ShowInList(width: 500);
            View.Property(p => p.RequestStr).UseHtmlEditor().Readonly();
            View.Property(p => p.ResponseStr).UseHtmlEditor().Readonly();
            View.Property(p => p.RequestDate).Readonly();
            View.Property(p => p.ResponseDate).Readonly();
            View.ChildrenProperty(p => p.UploadTransactionLogDtlList).HasLabel("事务上传记录明细");

            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ErpBatchId);
            View.Property(p => p.ResponseCode);
            View.Property(p => p.RequestDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Today).Readonly();
            View.Property(p => p.ResponseDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Today).Readonly();
        }
    }
}