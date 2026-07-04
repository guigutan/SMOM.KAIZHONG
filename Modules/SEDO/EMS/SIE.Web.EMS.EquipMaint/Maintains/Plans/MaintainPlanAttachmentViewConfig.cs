using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans
{
    /// <summary>
    /// 保养计划附件
    /// </summary>
    public class MaintainPlanAttachmentViewConfig : WebViewConfig<MaintainPlanAttachment>
    {
        /// <summary>
        /// 保养确认所用的备件更换视图
        /// </summary>
        public const string MaintainConfirmationListView = "MaintainConfirmationListView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaintainPlanViewModel));
            View.DeclareExtendViewGroup(MaintainConfirmationListView);
            if (ViewGroup == MaintainConfirmationListView)
            {
                ConfigListView();
                //View.ClearCommands();
                View.RemoveCommands("SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.UploadMaintainPlanExecuteAttachmentCommand", typeof(DeleteAttachmentCommand).FullName);//移除附件的上传和删除按钮
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ReplaceCommands(typeof(UploadAttachmentCommand).FullName,
                typeof(UploadMaintainPlanExecuteAttachmentCommand).FullName);
            View.ReplaceCommands(typeof(DeleteAttachmentCommand).FullName,
                "SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.DeleteMaintainPlanExecuteAttachmentCommand");

            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
