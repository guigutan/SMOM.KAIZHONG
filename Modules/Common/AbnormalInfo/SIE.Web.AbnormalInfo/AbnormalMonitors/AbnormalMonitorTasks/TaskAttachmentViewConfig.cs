using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.Web.Common.Attachments.Commands;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 附件视图配置
    /// </summary>
    internal class TaskAttachmentViewConfig : WebViewConfig<TaskAttachment>
    {

        /// <summary>
        /// 只读视图
        /// </summary>
        const string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.DeclareExtendViewGroup(new string[] { ReadonlyView });
            if (ViewGroup == ReadonlyView)
            {
                ConfigReadonlyView();
            }
        }


        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.AssignAuthorize(typeof(AbnormalMonitorTask));
            View.UseCommands(typeof(UploadAttachmentCommand).FullName, typeof(DeleteAttachmentCommand).FullName, typeof(FtpDownloadCommand).FullName, typeof(ViewImageCommand).FullName, typeof(ViewPdfCommand).FullName);
        }

        /// <summary>
        /// 只读视图
        /// </summary>
        public void ConfigReadonlyView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(FtpDownloadCommand).FullName, typeof(ViewImageCommand).FullName, typeof(ViewPdfCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.FileName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FilePath).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FileExtesion).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FileSize).Show(ShowInWhere.All).Readonly();
            }
        }

    }
}