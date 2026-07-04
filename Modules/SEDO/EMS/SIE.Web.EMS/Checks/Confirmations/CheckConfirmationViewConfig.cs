using SIE.EMS.Checks.Confirmations;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Checks.Records;
using SIE.Web.Common.Attachments.Commands;

namespace SIE.Web.EMS.Checks.Confirmations
{
    /// <summary>
    /// “点检确认”视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class CheckConfirmationViewConfig : WebViewConfig<CheckConfirmation>
    {
        /// <summary>
        /// 
        /// </summary>
        public const string CheckRecordListView = "CheckRecordListView";

        /// <summary>
        /// 点检确认视图
        /// </summary>
        public const string CheckConfirmView = "CheckConfirmView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CheckConfirmation), typeof(CheckRecord), typeof(CheckPlanViewModel));
            View.DeclareExtendViewGroup(CheckRecordListView, CheckConfirmView);
            if (ViewGroup == CheckRecordListView)
            {
                ConfigListView();
            }
            if (ViewGroup == CheckConfirmView)
            {
                CheckCfmView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(FtpDownloadCommand).FullName, typeof(ViewImageCommand).FullName);//移除附件的上传和删除按钮
            View.UseGridSelectionModel(isCheckboxmodel:false, mode:"SINGLE");
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectName).HasLabel("项目").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Score).UseEnumEditor(p => p.AllowBlank = false).Show(ShowInWhere.All).Readonly(ViewGroup == CheckRecordListView);
                View.Property(p => p.FileName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FilePath).HasLabel("文件路径(上传)").UseConfigValueEditor(p => { p.XType = "uploadPictureEditorForCheckPlanConfirmation"; p.AllowBlank = true; p.Editable = false; }).ShowInList(150).Show(ShowInWhere.Hide).Readonly(ViewGroup == CheckRecordListView);
                View.Property(p => p.FileExtesion).HasLabel("图片扩展名").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FileSize).HasLabel("图片大小").Show(ShowInWhere.All).Readonly(); 

                View.Property(p => p.Content).Show(ShowInWhere.Hide);

                View.Property(p => p.ConfirmResult).Show(ShowInWhere.Hide);
                View.Property(p => p.ConfirmNote).Show(ShowInWhere.Hide);

                View.Property(p => p.Confirmor).Show(ViewGroup == CheckRecordListView ? ShowInWhere.All : ShowInWhere.Hide).Readonly();
                View.Property(p => p.ConfirmDept).Show(ViewGroup == CheckRecordListView ? ShowInWhere.All : ShowInWhere.Hide).Readonly();
                View.Property(p => p.ConfirmDate).Show(ViewGroup == CheckRecordListView ? ShowInWhere.All : ShowInWhere.Hide).Readonly();

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        private void CheckCfmView()
        {
            View.ClearCommands();
            View.UseCommands("SIE.Web.EMS.Checks.Confirmations.Commands.RevokeUploadPicCommand");//移除附件的上传和删除按钮
            View.UseGridSelectionModel(isCheckboxmodel: false, mode: "SINGLE");
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectName).HasLabel("项目").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Score).UseEnumEditor(p => p.AllowBlank = false).Show(ShowInWhere.All).Readonly(p => ViewGroup == CheckRecordListView || p.ConfirmResult != null);
                View.Property(p => p.FileName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FilePath).HasLabel("文件路径(上传)").UseTextButtonFieldEditor(p => { p.XType = "uploadPictureEditorForCheckPlanConfirmation"; p.AllowBlank = true; p.Editable = false; }).ShowInList(150).Show(ShowInWhere.All).Readonly(p => ViewGroup == CheckRecordListView || p.ConfirmResult != null);
                View.Property(p => p.FileExtesion).HasLabel("图片扩展名").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FileSize).HasLabel("图片大小").Show(ShowInWhere.All).Readonly();

                View.Property(p => p.Content).Show(ShowInWhere.Hide);

                View.Property(p => p.ConfirmResult).Show(ShowInWhere.Hide);
                View.Property(p => p.ConfirmNote).Show(ShowInWhere.Hide);

                View.Property(p => p.Confirmor).Show(ViewGroup == CheckRecordListView ? ShowInWhere.All : ShowInWhere.Hide).Readonly();
                View.Property(p => p.ConfirmDept).Show(ViewGroup == CheckRecordListView ? ShowInWhere.All : ShowInWhere.Hide).Readonly();
                View.Property(p => p.ConfirmDate).Show(ViewGroup == CheckRecordListView ? ShowInWhere.All : ShowInWhere.Hide).Readonly();

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
