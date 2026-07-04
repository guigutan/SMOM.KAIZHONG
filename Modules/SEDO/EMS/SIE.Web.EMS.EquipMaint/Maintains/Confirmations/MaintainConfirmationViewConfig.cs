using SIE.EMS.Maintains.Confirmations;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.EMS.Maintains.Records;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.EMS.EquipMaint.Maintains.Confirmations.Commands;

namespace SIE.Web.EMS.EquipMaint.Maintains.Confirmations
{
    /// <summary>
    /// “保养确认”视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class MaintainConfirmationViewConfig : WebViewConfig<MaintainConfirmation>
    {
        /// <summary>
        /// 
        /// </summary>
        public const string MaintainRecordListView = "MaintainRecordListView";

        /// <summary>
        /// 保养确认视图
        /// </summary>
        public const string MaintainConfirmView = "MaintainConfirmView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaintainConfirmation), typeof(MaintainRecord), typeof(MaintainPlanViewModel));
            View.DeclareExtendViewGroup(MaintainRecordListView, MaintainConfirmView);
            if (ViewGroup == MaintainRecordListView)
            {
                ConfigListView();
            }
            else if (ViewGroup == MaintainConfirmView)
            {
                MaintainCfmView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(EmsDownloadCommand).FullName, typeof(ViewImageCommand).FullName);

            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectName).HasLabel("项目").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Score).UseEnumEditor(p => p.AllowBlank = false).Show(ShowInWhere.All).Readonly(ViewGroup == MaintainRecordListView);
                View.Property(p => p.FileName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FilePath).HasLabel("文件路径(上传)").UseConfigValueEditor(p =>
                {
                    p.XType = "uploadPictureEditorForMaintainPlanConfirmation";
                    p.AllowBlank = true;
                }).ShowInList(150)
                    .Show(ShowInWhere.Hide).Readonly(ViewGroup == MaintainRecordListView);
                View.Property(p => p.FileExtesion).HasLabel("图片扩展名").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FileSize).HasLabel("图片大小").Show(ShowInWhere.All).Readonly();

                View.Property(p => p.Content).Show(ShowInWhere.Hide);

                View.Property(p => p.ConfirmResult).Show(ViewGroup == MaintainRecordListView ? ShowInWhere.All : ShowInWhere.Hide);
                View.Property(p => p.ConfirmNote).Show(ViewGroup == MaintainRecordListView ? ShowInWhere.All : ShowInWhere.Hide);

                View.Property(p => p.Confirmor).Show(ViewGroup == MaintainRecordListView ? ShowInWhere.All : ShowInWhere.Hide).Readonly();
                View.Property(p => p.ConfirmDept).Show(ViewGroup == MaintainRecordListView ? ShowInWhere.All : ShowInWhere.Hide).Readonly();
                View.Property(p => p.ConfirmDate).Show(ViewGroup == MaintainRecordListView ? ShowInWhere.All : ShowInWhere.Hide).Readonly();

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 保养确认视图
        /// </summary>
        private void MaintainCfmView()
        {
            View.ClearCommands();
            View.UseCommands("SIE.Web.EMS.EquipMaint.Maintains.Confirmations.Commands.RevokeUploadPicCommand");//移除附件的上传和删除按钮
            View.UseGridSelectionModel(isCheckboxmodel: false, mode: "SINGLE");
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectName).HasLabel("项目").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Score).UseEnumEditor(p => p.AllowBlank = false).Show(ShowInWhere.All).Readonly(ViewGroup == MaintainRecordListView);
                View.Property(p => p.FileName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FilePath).HasLabel("文件路径(上传)").UseConfigValueEditor(p =>
                {
                    p.XType = "uploadPictureEditorForMaintainPlanConfirmation";
                    p.AllowBlank = true;
                }).ShowInList(150)
                    .Show(ViewGroup == MaintainRecordListView ? ShowInWhere.Hide : ShowInWhere.All).Readonly(ViewGroup == MaintainRecordListView);
                View.Property(p => p.FileExtesion).HasLabel("图片扩展名").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FileSize).HasLabel("图片大小").Show(ShowInWhere.All).Readonly();

                View.Property(p => p.Content).Show(ShowInWhere.Hide);

                View.Property(p => p.ConfirmResult).Show(ViewGroup == MaintainRecordListView ? ShowInWhere.All : ShowInWhere.Hide);
                View.Property(p => p.ConfirmNote).Show(ViewGroup == MaintainRecordListView ? ShowInWhere.All : ShowInWhere.Hide);

                View.Property(p => p.Confirmor).Show(ViewGroup == MaintainRecordListView ? ShowInWhere.All : ShowInWhere.Hide).Readonly();
                View.Property(p => p.ConfirmDept).Show(ViewGroup == MaintainRecordListView ? ShowInWhere.All : ShowInWhere.Hide).Readonly();
                View.Property(p => p.ConfirmDate).Show(ViewGroup == MaintainRecordListView ? ShowInWhere.All : ShowInWhere.Hide).Readonly();

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
