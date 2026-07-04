using SIE.EMS.AssetRequisitions.Configs;

namespace SIE.Web.EMS.AssetRequisitions
{
    /// <summary>
    ///  视图配置
    /// </summary>
    public class ApprovalFlowConfigValueViewConfig : WebViewConfig<ApprovalFlowConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            bool falseValue = false;
            View.Property(p => p.IsEnableApproval).Cascade(p => p.IsEnableApprovalFlow, null);
            View.Property(p => p.IsEnableApprovalFlow).Readonly(p => p.IsEnableApproval == falseValue);
        }
    }
}
