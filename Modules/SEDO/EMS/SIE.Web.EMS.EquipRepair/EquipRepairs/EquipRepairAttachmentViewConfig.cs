using SIE.Domain;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.Lubrications;
using SIE.EMS.SpecialEquipment.RegularInspections;
using SIE.Web.ClientMetaModel;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.Core.Common.Commands;
using SIE.Web.EMS.EquipRepair.EquipRepairs.Commands;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 图片附件视图配置
    /// </summary>
    public class EquipRepairAttachmentViewConfig : WebViewConfig<EquipRepairAttachment>
    {
        /// <summary>
        /// 查看视图
        /// </summary>
        private readonly string PhotoViewGroup = "PhotoView";

        /// <summary>
        /// 报修视图
        /// </summary>
        private readonly string ApplyRepairViewGroup = "ApplyRepairView";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { PhotoViewGroup, ApplyRepairViewGroup });
            if (ViewGroup == PhotoViewGroup)
            {
                PhotoView();
            }
            if (ViewGroup == ApplyRepairViewGroup)
            {
                ApplyRepairView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal(true);
            View.AddBehavior("SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.RepairAttachmentBehavior");
            View.ReplaceCommands(typeof(UploadAttachmentCommand).FullName, typeof(UploadRepairAttachmentCommand).FullName);
            View.ReplaceCommands(typeof(DeleteAttachmentCommand).FullName, "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.DeleteRepairAttachmentCommand");
            View.UseCommand(typeof(ViewImageCommand).FullName);
            View.Property(p => p.RepairOperationType).Readonly().HasOrderNo(8);
            View.AttachDetailChildrenProperty(typeof(EquipRepairAttachment), (c) =>
            {
                var item = c.Parent as EquipRepairAttachment;
                return RF.GetById<EquipRepairAttachment>(item.Id);

            }, PhotoViewGroup).HasLabel("图片查看");
        }

        /// <summary>
        /// 查看视图
        /// </summary>
        protected void PhotoView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            { 
                View.Property(p => p.Content)
                   .UseCustomEditor(p => { p.XType = "repairPictureEditor"; p.AllowAsterisk = AllowAsterisks.close; }).HasLabel("")
                    .ShowInDetail();
                View.Property(p => p.FileName).Show(ShowInWhere.Hide);
                View.Property(p => p.FilePath).Show(ShowInWhere.Hide);
                View.Property(p => p.FileExtesion).Show(ShowInWhere.Hide);
                View.Property(p => p.FileSize).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 报修视图
        /// </summary>
        protected void ApplyRepairView()
        {
            View.AssignAuthorize(typeof(RegularInspection), typeof(Lubrication));
            View.UseDefaultCommands();
            View.ReplaceCommands(typeof(DeleteAttachmentCommand).FullName, typeof(ImmediateDeleteCommand).FullName);
            View.Property(p => p.RepairOperationType).Readonly().Show(ShowInWhere.All).HasOrderNo(8);
        }
    }
}
