using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.Equipments.EquipAccounts.Commands;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.ExtensionViewConfig
{
    /// <summary>
	/// 设备台账附件资料视图配置
	/// </summary>    
    internal class MeteringEquipAccountAttachmentViewConfig : WebViewConfig<MeteringEquipAccountAttachment>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommand(typeof(SetEquipLogoCommand).FullName);
            View.ReplaceCommands(typeof(DeleteAttachmentCommand).FullName,typeof(DeleteEquipAccountAttachmentCommand).FullName);
            View.ReplaceCommands(typeof(UploadAttachmentCommand).FullName, typeof(UploadEqpAttachmentCommand).FullName);

            View.Property(p => p.FilePath).ShowInList(width: 20 * 25).Readonly();
            View.Property(p => p.FileName).ShowInList(width: 20 * 12).Readonly();
            View.Property(p => p.IsEquipLogo).ShowInList(width: 20 * 4).HasLabel("设备Logo").HasOrderNo(5).Readonly();
        }
    }
}
