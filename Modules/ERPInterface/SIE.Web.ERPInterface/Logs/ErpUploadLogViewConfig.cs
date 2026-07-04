using SIE.ERPInterface.Common.Logs;
using SIE.Web.ERPInterface.Logs.Commands;

namespace SIE.Web.ERPInterface.Logs
{
    /// <summary>
    /// 任务下载记录视图配置
    /// </summary>
    internal class ErpUploadLogViewConfig : WebViewConfig<ErpUploadLog>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.DisableEditing();
            View.UseCommands(typeof(ReUploadErpUploadLogCommand).FullName, typeof(LookUpContextCommand).FullName, typeof(CloseUploadErpUploadLogCommand).FullName, typeof(RestoreUploadErpUploadLogCommand).FullName);
            View.Property(p => p.OrderNo).ShowInList(width: 120);
            View.Property(p => p.LineNo);
            View.Property(p => p.TransactionType);
            View.Property(p => p.InterfaceName);
            View.Property(p => p.ResponseMessage).ShowInList(width: 200);
            View.Property(p => p.IsSuccess).Readonly();
            //View.Property(p => p.RequestStr).ShowInList(width: 200);
            //View.Property(p => p.ResponseStr);
            View.Property(p => p.ReloadCount);
            View.Property(p => p.State);
            View.Property(p => p.UploadTransactionState).UseDisplayEditor(f => f.ColumnXType = "stateColorStyle").Show();
            View.Property(p => p.UpdateDate).HasLabel("上传时间");

            View.Property(p => p.SapKeyMsg);
            View.Property(p => p.TransactionId);
            View.Property(p => p.UploadTransactionId).HasLabel("事务上传ID");


        }

        /// <summary>
        /// 明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.DisableEditing();
            View.HasDetailColumnsCount(4);
            View.AddBehavior("SIE.Web.ERPInterface.Scripts.ErpUploadLogBehavior");
            using (View.OrderProperties())
            {

                View.Property(p => p.InterfaceName).ShowInDetail(columnSpan: 2);
                View.Property(p => p.OrderNo).ShowInDetail(columnSpan: 2);
                View.Property(p => p.State).ShowInDetail(columnSpan: 2);
                View.Property(p => p.LineNo).ShowInDetail(columnSpan: 2).HasLabel("行号");
                View.Property(p => p.RequestStr).UseMemoEditor().ShowInDetail(columnSpan: 4, height: "300px");
                View.Property(p => p.ResponseStr).UseMemoEditor().ShowInDetail(columnSpan: 2, height: "300px");

            }
        }
    }
}