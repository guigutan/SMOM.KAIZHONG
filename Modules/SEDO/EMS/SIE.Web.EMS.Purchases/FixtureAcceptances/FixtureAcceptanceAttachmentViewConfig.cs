using SIE.Domain;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.Web.ClientMetaModel;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.EMS.Purchases.FixtureAcceptances.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.FixtureAcceptances
{
    /// <summary>
    /// 验收附件视图配置
    /// </summary>
    internal class FixtureAcceptanceAttachmentViewConfig : WebViewConfig<FixtureAcceptanceAttachment>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.ClearCommands(); 
            View.UseCommands("SIE.Web.EMS.Purchases.FixtureAcceptances.Commands.UploadFixtureAcceptCommand", typeof(DeleteAcceptanceAttachmentCommand).FullName,
                typeof(FtpDownloadCommand).FullName);
            View.Property(p => p.FileName).ShowInList(200);
            View.Property(p => p.FilePath);
            View.Property(p => p.FileExtesion);
            View.Property(p => p.FileSize);
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
            View.UseCommand(typeof(SaveAcceptanceAttachmentCommand).FullName);
            View.Property(p => p.SnViewModelId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as FixtureAcceptanceAttachment;
                
                var models = new EntityList<FixtureAcceptanceSn>();

                if (entity == null || !entity.OwnerId.HasValue || entity.OwnerId.Value == 0)
                {
                    return models;
                }
                return RT.Service.Resolve<FixtureAcceptancesController>().GetAcceptanceSnInfo(entity.OwnerId.Value, pagingInfo);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.Sn), nameof(e.Sn));
                m.DicLinkField = keyValues;
            });

            View.Property(p => p.FilePath)
                .UseConfigValueEditor(p =>
                {
                    p.XType = "emsFileUploadContextEditor";
                    p.AllowAsterisk = AllowAsterisks.close;
                }).HasLabel("")
                .Show(ShowInWhere.All);

            View.Property(p => p.FileName).Show(ShowInWhere.Hide);
            View.Property(p => p.FileExtesion).Show(ShowInWhere.Hide);
            View.Property(p => p.FileSize).Show(ShowInWhere.Hide);
        }

    }
}