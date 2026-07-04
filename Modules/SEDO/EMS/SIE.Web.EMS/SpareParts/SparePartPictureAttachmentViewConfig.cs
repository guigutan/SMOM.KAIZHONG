using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.EMS.SpareParts.Commands;

namespace SIE.Web.EMS.SpareParts
{
    /// <summary>
    /// 图片附件视图配置
    /// </summary>
    public class SparePartPictureAttachmentViewConfig : WebViewConfig<SparePartPictureAttachment>
    {
        /// <summary>
        /// 图片
        /// </summary>
        public const string PhotoViewGroup = "PhotoView";

        /// <summary>
        /// 表单视图
        /// </summary>
        public const string FormEditViewGroup = "FormEditView";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { PhotoViewGroup, FormEditViewGroup });

            if (ViewGroup == PhotoViewGroup)
                PhotoView();
            if (ViewGroup == FormEditViewGroup)
                FormEditView();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal(true);
            View.AddBehavior("SIE.Web.EMS.SpareParts.Behaviors.SparePartPictureBehavior");
            View.ReplaceCommands(typeof(UploadAttachmentCommand).FullName, typeof(UploadSparePartPicCommand).FullName);
            View.RemoveCommands(typeof(ViewImageCommand).FullName);
            View.Property(p => p.FileName).HasLabel("图片名称");
            View.Property(p => p.FilePath).HasLabel("图片路径");
            View.Property(p => p.FileExtesion).HasLabel("图片扩展名");
            View.Property(p => p.FileSize).HasLabel("图片大小");
            View.AttachDetailChildrenProperty(typeof(SparePartPictureAttachment), (c) =>
            {
                var item = c.Parent as SparePartPictureAttachment;
                item = RF.GetById<SparePartPictureAttachment>(item.Id);
                return item;
            }, PhotoViewGroup).HasLabel("备件图片");
        }
        /// <summary>
        /// 图片
        /// </summary>
        protected void PhotoView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p=>p.Content).UseCustomEditor(p => p.XType = "sparePartPictureEditor").HasLabel("").ShowInDetail();
                View.Property(p => p.FileName).Show(ShowInWhere.Hide);
                View.Property(p => p.FilePath).Show(ShowInWhere.Hide);
                View.Property(p => p.FileExtesion).Show(ShowInWhere.Hide);
                View.Property(p => p.FileSize).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 表单编辑视图
        /// </summary>
        protected void FormEditView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(typeof(UploadAttachmentCommand).FullName,typeof(UploadSparePartPicCommand).FullName);
            View.Property(p => p.FileName).HasLabel("图片名称");
            View.Property(p => p.FilePath).HasLabel("图片路径");
            View.Property(p => p.FileExtesion).HasLabel("图片扩展名");
            View.Property(p => p.FileSize).HasLabel("图片大小");

        }
    }
}
