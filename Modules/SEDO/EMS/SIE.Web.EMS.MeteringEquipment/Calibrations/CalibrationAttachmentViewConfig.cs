using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.Web.Common.Attachments.Commands;

namespace SIE.Web.EMS.MeteringEquipment.Calibrations
{
	/// <summary>
	/// 计量设备定检附件视图配置
	/// </summary>
	[ManagedProperty.CompiledPropertyDeclarer]
	public class CalibrationAttachmentViewConfig : WebViewConfig<CalibrationAttachment>
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

        /// <summary>
        /// readonlyCommand
        /// </summary>
        protected virtual void UseReadOnlyCommand()
        {
            View.ClearCommands();
            View.UseCommands(typeof(FtpDownloadCommand).FullName, typeof(ViewImageCommand).FullName);
            using (View.OrderProperties())
            {
                View.UseGridSelectionModel(mode: "SINGLE");
                View.Property(p => p.FileName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FilePath).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FileExtesion).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FileSize).Show(ShowInWhere.All).Readonly();
            }
        }

	}
}