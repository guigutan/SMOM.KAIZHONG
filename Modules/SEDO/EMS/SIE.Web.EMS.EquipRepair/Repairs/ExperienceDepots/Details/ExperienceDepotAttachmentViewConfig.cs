using SIE.Domain;
using SIE.EMS.EquipRepair.ExperienceDepots.Attachments;
using SIE.Web.Common.Attachments.Commands;

namespace SIE.Web.EMS.EquipRepair.Repairs.ExperienceDepots.Details
{
    /// <summary>
    /// 图片视图配置
    /// </summary>
    public class ExperienceDepotAttachmentViewConfig : WebViewConfig<ExperienceDepotAttachment>
    {/// <summary>
     /// 图片
     /// </summary>
        public const string PhotoViewGroup = "PhotoView";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(PhotoViewGroup);
            if (ViewGroup == PhotoViewGroup)
                PhotoView();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal(true);
            View.AddBehavior("SIE.Web.EMS.EquipRepair.Repairs.ExperienceDepots.Behaviors.ExpDepotPictureBehavior");
            View.ReplaceCommands(typeof(ViewImageCommand).FullName, "SIE.Web.EMS.EquipRepair.Repairs.ExperienceDepots.Details.ViewNewImageCommand");
            View.Property(p => p.FileName).HasLabel("图片名称");
            View.Property(p => p.FilePath).HasLabel("图片路径");
            View.Property(p => p.FileExtesion).HasLabel("图片扩展名");
            View.Property(p => p.FileSize).HasLabel("图片大小");
            View.AttachDetailChildrenProperty(typeof(ExperienceDepotAttachment), (c) =>
            {
                var item = c.Parent as ExperienceDepotAttachment;
                item = RF.GetById<ExperienceDepotAttachment>(item.Id);
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
                View.Property(p => p.Content).UseCustomEditor(p => p.XType = "expDepotPictureEditor").HasLabel("").ShowInDetail();
                View.Property(p => p.FileName).Show(ShowInWhere.Hide);
                View.Property(p => p.FilePath).Show(ShowInWhere.Hide);
                View.Property(p => p.FileExtesion).Show(ShowInWhere.Hide);
                View.Property(p => p.FileSize).Show(ShowInWhere.Hide);
            }
        }
    }
}
