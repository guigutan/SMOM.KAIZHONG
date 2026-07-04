using SIE.Domain;
using SIE.Equipments.Abnormal.SysSenders;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.WipResources;
using System.Collections.Generic;

namespace SIE.Web.Equipments.Abnormal
{
    /// <summary>
    /// 异常停线触发任务配置项视图配置
    /// </summary>
    class AbnormalAlertSysSenderConfigViewConfig : WebViewConfig<AbnormalAlertSysSenderConfig>
    {
        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.Equipments.Abnormal.Behaviors.AbnormalAlertSysConfigBehavior");
            View.Property(p => p.LineId).UseDataSource((e, c, r) =>
                {
                    var eq = e as AbnormalAlertSysSenderConfig;
                    if (eq == null)
                        return new EntityList<WipResource>();
                    return AppRuntime.Service.Resolve<WipResourceController>().GetWipResources(null, new List<SyncSourceType>() { SyncSourceType.Enterprise }, c, r);
                })
                .UsePagingLookUpEditor(p =>
                {
                    p.DicLinkField = new Dictionary<string, string>()
                    {
                        { nameof(AbnormalAlertSysSenderConfig.LineName),nameof(WipResource.Name)}
                    };
                }).HasLabel("默认产线").UseListSetting(p => p.HelpInfo = "预警配置如包括产线，则以预警配置的产线为推送条件。");
            View.Property(p => p.LineName).Readonly().HasLabel("默认产线名称");
            View.Property(p => p.EquipAccountId).UsePagingLookUpEditor(p =>
            {
                p.DicLinkField = new Dictionary<string, string>()
                    {
                        { nameof(AbnormalAlertSysSenderConfig.EquipName),nameof(EquipAccount.Name)}
                    };
            }).HasLabel("默认设备").UseListSetting(p => p.HelpInfo = "预警配置如包括设备，则以预警配置的设备为推送条件。");
            View.Property(p => p.EquipName).Readonly().HasLabel("默认设备名称");
            View.Property(p => p.IsAutoRestore).HasLabel("").UseCheckEditor(p =>
            {
                p.BoxLabel = "在预警解除后，停线自动恢复。";
            });
        }
    }
}
