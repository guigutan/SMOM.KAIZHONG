using SIE.Domain;
using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.EMS.Purchases.SparePartAcceptances.ViewModels;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands;
using SIE.Web.EMS.Purchases.SparePartAcceptances.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 验收附件视图配置
    /// </summary>
    internal class SparePartAcceptanceAttachmentViewConfig : WebViewConfig<SparePartAcceptanceAttachment>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.ClearCommands();
            View.UseCommands("SIE.Web.EMS.Purchases.SparePartAcceptances.Commands.UploadSpareAcceptCommand", typeof(DeleteAcceptanceAttachmentCommand).FullName,
                typeof(FtpDownloadCommand).FullName);
            View.Property(p => p.FileName).ShowInList(200);
            View.Property(p => p.FilePath);
            View.Property(p => p.FileExtesion);
            View.Property(p => p.FileSize);
            View.Property(p => p.LotNo);
            View.Property(p => p.Sn);
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
            View.UseCommand(typeof(SaveAcceptAttachmentCommand).FullName);
            View.Property(p => p.LotViewModelId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as SparePartAcceptanceAttachment;

                if (entity == null || !entity.OwnerId.HasValue)
                {
                    return new EntityList<SparePartAcceptanceLotViewModel>();
                }

                var list = RT.Service.Resolve<SparePartAcceptanceController>().GetSparePartAcceptanceLotViewModels(entity.OwnerId.Value);

                return list;
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.LotNo), nameof(e.LotViewModel.LotNo));
                m.DicLinkField = keyValues;
            });

            View.Property(p => p.SnViewModelId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as SparePartAcceptanceAttachment;

                if (entity == null || !entity.OwnerId.HasValue)
                {
                    return new EntityList<SparePartAcceptanceSnViewModel>();
                }

                var list = RT.Service.Resolve<SparePartAcceptanceController>().GetSparePartAcceptanceSnViewModels(entity.OwnerId.Value);

                return list;
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.Sn), nameof(e.SnViewModel.Sn));
                m.DicLinkField = keyValues;
            });
            
            View.Property(p => p.FilePath).UseImageComponentEditor(p =>
                 {
                     p.XType = "emsFileUploadContextEditor";
                 })
                 .HasLabel("")
                 .Readonly()
                 .ShowInDetail(columnSpan: 1);
            View.Property(p => p.FileName).Show(ShowInWhere.Hide);
            View.Property(p => p.FileExtesion).Show(ShowInWhere.Hide);
            View.Property(p => p.FileSize).Show(ShowInWhere.Hide);
        }
    }
}