using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.EMS.EarlierStage.Projects.Commands;

namespace SIE.Web.EMS.EarlierStage.Projects
{
    /// <summary>
	/// 项目管理附件资料视图配置
	/// </summary>    
    internal class ProjectAttachmentViewConfig : WebViewConfig<ProjectAttachment>
    {
        ///<summary>
        /// 配置列表视图 
        /// </summary>
        protected override void ConfigListView()
        {
            View.ReplaceCommands(typeof(UploadAttachmentCommand).FullName, typeof(UploadProjectAttachmentCommand).FullName);
            View.ReplaceCommands(typeof(DeleteAttachmentCommand).FullName, typeof(DeleteProjectAttachmentCommand).FullName);
            View.Property(p => p.FilePath).ShowInList(width: 20 * 25).Readonly();
            View.Property(p => p.FileName).ShowInList(width: 20 * 12).Readonly();
        }
    }
}
