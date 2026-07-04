using SIE.Domain;
using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.EMS.Purchases.EquipmentAcceptances.ViewModels;
using SIE.Web.ClientMetaModel;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.EquipmentAcceptances
{
    /// <summary>
    /// 设备开箱验收附件视图配置
    /// </summary>
    internal class EquipmentAcceptanceAttachmentViewConfig : WebViewConfig<EquipmentAcceptanceAttachment>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.ClearCommands();
            View.UseCommand("SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.UploadAcceptanceAttachmentCommand");
            View.UseCommand(typeof(DeleteAcceptanceAttachmentCommand).FullName);
            View.UseCommand(typeof(FtpDownloadCommand).FullName);
            View.Property(p => p.EquipmentCode).ShowInList(width: 200);
            View.Property(p => p.FileName).ShowInList(width: 200);
            View.Property(p => p.FilePath);
            View.Property(p => p.FileExtesion);
            View.Property(p => p.FileSize);
            View.Property(p => p.CreateByName).HasLabel("上传人");
            View.Property(p => p.CreateDate).HasLabel("上传时间");
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(1);

            View.ClearCommands();
            View.UseCommand("SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.SaveAcceptanceAttachmentCommand");
            View.Property(p => p.EquipmentAcceptanceDetailViewModel)
                .UseDataSource((source, pagingInfo, keyword) =>
                {
                    var equipmentAcceptanceAttachment = source as EquipmentAcceptanceAttachment;
                    if (equipmentAcceptanceAttachment == null
                        || !equipmentAcceptanceAttachment.OwnerId.HasValue)
                    {
                        return new EntityList<EquipmentAcceptanceDetailViewModel>();
                    }

                    return RT.Service.Resolve<EquipmentAcceptanceController>()
                        .GetEquipmentAcceptanceDetailViewModels(equipmentAcceptanceAttachment.OwnerId.Value, pagingInfo, keyword);
                })
                .UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EquipmentCode), nameof(e.EquipmentAcceptanceDetailViewModel.EquipmentCode));
                    m.DicLinkField = keyValues;
                });

            View.Property(p => p.FilePath)
                .UseConfigValueEditor(p =>
                {
                    p.XType = "emsFileUploadEditor";
                    p.AllowAsterisk = AllowAsterisks.close;
                })
                .Show(ShowInWhere.All).HasLabel("");

            View.Property(p => p.FileName).Show(ShowInWhere.Hide);
            View.Property(p => p.FileExtesion).Show(ShowInWhere.Hide);
            View.Property(p => p.FileSize).Show(ShowInWhere.Hide);

        }
    }
}