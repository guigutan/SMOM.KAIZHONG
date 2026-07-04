using SIE.Domain;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands;
using SIE.Web.EMS.Purchases.EquipmentSetups.Commands;

namespace SIE.Web.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试附件视图配置
    /// </summary>
    internal class SetupAttachmentViewConfig : WebViewConfig<SetupAttachment>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands("SIE.Web.EMS.Purchases.EquipmentSetups.Commands.UploadSetupCommand", typeof(DeleteAcceptanceAttachmentCommand).FullName,
                typeof(FtpDownloadCommand).FullName);
            View.Property(p => p.FileName).ShowInList(200);
            View.Property(p => p.FilePath).ShowInList(150);
            View.Property(p => p.FileExtesion);
            View.Property(p => p.FileSize);
            View.Property(p => p.EquipmentSetupPlanId).HasLabel("工作节点");
            View.Property(p => p.EquipAccountId).HasLabel("设备编码");
            View.Property(p => p.EquipAccountName).ShowInList(150);
            View.Property(p => p.UploaderId);
            View.Property(p => p.UploadDate).ShowInList(150);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(2);
            View.UseCommand(typeof(SaveSetupAttachmentCommand).FullName);
            View.Property(p => p.EquipmentSetupPlanId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as SetupAttachment;
                if (entity == null)
                {
                    return new EntityList<EquipmentSetupPlan>();
                }
                return RT.Service.Resolve<EquipmentSetupController>().GetPlansBySetupId(entity.EquipmentSetupId, pagingInfo, keyword);
            }).HasLabel("工作计划");
            View.Property(p => p.EquipAccountId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as SetupAttachment;
                if (entity == null)
                {
                    return new EntityList<EquipAccount>();
                }
                return RT.Service.Resolve<EquipmentSetupController>().GetEquipmentsBySetupId(entity.EquipmentSetupId, pagingInfo);
            }).HasLabel("设备编码");
            View.Property(p => p.FilePath).UseConfigValueEditor(p =>
            {
                p.XType = "uploadequipsetupeditor";
                p.TriggerCls = "iconfont icon-ArrowUpBold1";
                p.AllowBlank = false;
            }).HasLabel("请选择文件").ShowInDetail(columnSpan: 2);
        }
    }
}