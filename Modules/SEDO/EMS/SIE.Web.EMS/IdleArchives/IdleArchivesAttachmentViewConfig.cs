using SIE.EMS.AssetTransfers;
using SIE.EMS.IdleArchives;
using SIE.Web.Common.Attachments.Commands;

namespace SIE.Web.EMS.IdleArchives
{
    /// <summary>
    /// 闲置封存附件视图配置
    /// </summary>
    internal class IdleArchivesAttachmentViewConfig : WebViewConfig<IdleArchiveAttachment>
    {

        /// <summary>
        /// 添加记录
        /// </summary>
        public const string SeeView = "SeeView";


        /// <summary>
        /// 附件视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SeeView);
            if (ViewGroup == SeeView)
            {
                this.UseReadOnlyCommand();
            }
        }

        protected override void ConfigListView()
        {
            View.ReplaceCommands(typeof(DeleteAttachmentCommand).FullName, "SIE.Web.Core.Common.Commands.ImmediateDeleteCommand");
        }

        /// <summary>
        /// readonlyCommand
        /// </summary>
        protected virtual void UseReadOnlyCommand()
        {
            View.ClearCommands();
            View.UseCommands(typeof(FtpDownloadCommand).FullName, typeof(ViewImageCommand).FullName);
        }
    }
}