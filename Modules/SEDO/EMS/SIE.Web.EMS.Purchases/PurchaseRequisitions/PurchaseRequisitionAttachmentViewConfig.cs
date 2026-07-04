using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands;

namespace SIE.Web.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 附件界面
    /// </summary>
    internal class PurchaseRequisitionAttachmentViewConfig : WebViewConfig<PurchaseRequisitionAttachment>
    {
        /// <summary>
        /// 附件视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.ClearCommands();
            View.UseCommands(typeof(UploadPurReqAttachmentCommand).FullName, typeof(DeletePurReqAttachmentCommand).FullName,
                typeof(FtpDownloadCommand).FullName, typeof(ViewImageCommand).FullName);
            using (View.OrderProperties())
            {
                View.UseGridSelectionModel(mode: "SINGLE");
                View.Property(p => p.FileName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FilePath).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FileExtesion).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FileSize).Show(ShowInWhere.All).Readonly();
            }
        }
    }
}
