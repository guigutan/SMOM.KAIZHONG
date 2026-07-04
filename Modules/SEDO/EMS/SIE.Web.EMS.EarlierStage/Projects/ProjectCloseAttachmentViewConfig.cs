using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.EMS.EarlierStage.Projects.Commands;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目结项附件界面
    /// </summary>
    internal class ProjectCloseAttachmentViewConfig : WebViewConfig<ProjectCloseAttachment>
    {
        /// <summary>
        /// 附件视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.ClearCommands();
            View.UseCommands(typeof(UploadProCloseAttachmentCommand).FullName, typeof(DeleteProCloseAttachmentCommand).FullName,
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
