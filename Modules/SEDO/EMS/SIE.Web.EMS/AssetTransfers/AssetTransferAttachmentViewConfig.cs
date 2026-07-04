using SIE.EMS.AssetTransfers;
using SIE.Web.Common.Attachments.Commands;

namespace SIE.Web.EMS.AssetTransfers
{
    /// <summary>
    /// 资产调拨附件视图配置
    /// </summary>
    internal class AssetTransferAttachmentViewConfig : WebViewConfig<AssetTransferAttachment>
    {

        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup("UploadView");
            if (ViewGroup == "UploadView")
            {
                UploadView();
            }
        
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.RemoveCommands(typeof(ViewImageCommand).FullName);
            View.ReplaceCommands(typeof(DeleteAttachmentCommand).FullName, "SIE.Web.Core.Common.Commands.ImmediateDeleteCommand");
        }
       
        /// <summary>
        /// 发货上传视图
        /// </summary>
        private void UploadView()
        {
            View.RemoveCommands(typeof(ViewImageCommand).FullName,typeof(DeleteAttachmentCommand).FullName,
                typeof(DownloadCommand).FullName,typeof(FtpDownloadCommand).FullName);
        }
    }
}