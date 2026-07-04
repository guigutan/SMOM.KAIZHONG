using SIE.Fixtures.Repairs;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.Fixtures.Repairs.Commands;

namespace SIE.Web.Fixtures.Repairs
{
    /// <summary>
    /// 保养任务查询体-界面
    /// </summary>
    public class FixtureRepairAttachmentViewConfig : WebViewConfig<FixtureRepairAttachment>
    {
        /// <summary>
        /// 自定义添加异常/维修详情视图
        /// </summary>
        public const string AddFixtureRepairDetail = "AddFixtureRepairDetail";

        /// <summary>
        /// 自定义异常/维修详情视图
        /// </summary>
        public const string FixtureRepairDetail = "FixtureRepairDetail";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AddFixtureRepairDetail, FixtureRepairDetail);
            if (ViewGroup == AddFixtureRepairDetail)
                AddFixtureRepairDetailView();
            if (ViewGroup == FixtureRepairDetail)
                FixtureRepairDetailView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(FtpDownloadCommand).FullName, typeof(ViewImageCommand).FullName, typeof(ViewPdfCommand).FullName);
            View.Property(p => p.FileName);
            View.Property(p => p.FilePath);
            View.Property(p => p.FileExtesion);
            View.Property(p => p.FileSize);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate);
        }

        /// <summary>
        /// 工治具报修-添加页面-【图片】视图配置
        /// </summary>
        protected virtual void AddFixtureRepairDetailView()
        {
            View.AssignAuthorize(typeof(FixtureRepair));
            View.ReplaceCommands(typeof(UploadAttachmentCommand).FullName, typeof(UploadPictureCommand).FullName);
            View.Property(p => p.FileName);
            View.Property(p => p.FilePath);
            View.Property(p => p.FileExtesion);
            View.Property(p => p.FileSize);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate);
        }

        /// <summary>
        /// 工治具报修-维修页面-【图片】视图配置
        /// </summary>
        protected virtual void FixtureRepairDetailView()
        {
            View.AssignAuthorize(typeof(FixtureRepair));
            View.RemoveCommands(typeof(UploadAttachmentCommand).FullName, typeof(DeleteAttachmentCommand).FullName);
            View.Property(p => p.FileName);
            View.Property(p => p.FilePath);
            View.Property(p => p.FileExtesion);
            View.Property(p => p.FileSize);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate);
        }
    }
}
