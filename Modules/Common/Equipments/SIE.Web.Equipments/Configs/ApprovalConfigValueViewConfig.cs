using SIE.Equipments.Configs;

namespace SIE.Web.Equipments.Configs
{
    /// <summary>
    /// 审批流程配置值-界面
    /// </summary>
    internal class ApprovalConfigValueViewConfig : WebViewConfig<ApprovalConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.EnableAudit).Cascade(p => p.EnableApproval, null);
            View.Property(p => p.EnableApproval).Show(ShowInWhere.Hide).Readonly(p => !p.EnableAudit);
        }
    }
}
