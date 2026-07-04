using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.MetaModel.View;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.EMS.Checks.Plans.Commands;

namespace SIE.Web.EMS.Checks.Plans
{
    public class CheckPlanAttachmentViewConfig : WebViewConfig<CheckPlanAttachment>
    {
        public const string CheckConfirmationListView = "CheckConfirmationListView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CheckPlanViewModel));
            View.DeclareExtendViewGroup(CheckConfirmationListView);
            if (ViewGroup == CheckConfirmationListView)
            {
                ConfigListView();
                //View.ClearCommands();
                View.RemoveCommands("SIE.Web.EMS.Checks.Plans.Commands.UploadCheckPlanExecuteAttachmentCommand", typeof(DeleteAttachmentCommand).FullName);//移除附件的上传和删除按钮
            }

        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ReplaceCommands(typeof(UploadAttachmentCommand).FullName, typeof(UploadCheckPlanExecuteAttachmentCommand).FullName);
            View.ReplaceCommands(typeof(DeleteAttachmentCommand).FullName, "SIE.Web.EMS.Checks.Plans.Commands.DeleteCheckPlanExecuteAttachmentCommand");

            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
