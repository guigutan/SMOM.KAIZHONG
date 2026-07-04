using SIE.FMS;
using SIE.Web.FMS.FileManage.Commands;

namespace SIE.Web.FMS
{
    /// <summary>
    /// 文件管理 视图配置
    /// </summary>
    internal class FileManageViewConfig : WebViewConfig<SIE.FMS.FileManage>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(FileManageCommands.UploadCommand, FileManageCommands.DeleteCommand, FileManageCommands.DownLoadCommand, FileManageCommands.PreViewCommand,
                FileManageCommands.EditCommand, FileManageCommands.PublishCommand, FileManageCommands.ScarpCommand, FileManageCommands.FlowCommand, FileManageCommands.ReturnCommand, FileManageCommands.AuditCommand);
            using (View.OrderProperties())
            {
                View.Property(p => p.Name);
                View.Property(p => p.Code);
                View.Property(p => p.Version);
                View.Property(p => p.FileState);
                View.Property(p => p.Size);
                View.Property(p => p.CreateBy).HasLabel("上传人");
                View.Property(p => p.CreateDate).HasLabel("上传时间");
            }
        }
    }
}
