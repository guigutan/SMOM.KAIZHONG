using SIE.EMS.FixedAssets.Accounts;

namespace SIE.Web.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 设备履历视图配置
    /// </summary>
    public class FixedAssetResumeViewConfig : WebViewConfig<FixedAssetResume>
    {
        /// <summary>
        /// 列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.AddBehavior("SIE.Web.EMS.FixedAssets.Accounts.Scripts.FixedAssetResumeBehavior");
            View.UseCommands("SIE.Web.EMS.FixedAssets.Accounts.Commands.OpenResumeBillViewCommand", "SIE.Web.EMS.FixedAssets.Accounts.Commands.EquipResumeSearchCommand");
            View.Property(p => p.UseState).HasLabel("管理状态");
            View.Property(p => p.ResumeType);
            View.Property(p => p.No);
            View.Property(p => p.Remark);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate);
        }
    }
}
