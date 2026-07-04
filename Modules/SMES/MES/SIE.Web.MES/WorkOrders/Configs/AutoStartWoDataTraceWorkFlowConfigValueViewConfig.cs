using SIE.MES.WorkOrders.Configs;

namespace SIE.Web.MES.WorkOrders.Configs
{
    /// <summary>
    /// 工单创建是否自动发起追溯流程 配置值视图配置
    /// </summary>
    public class AutoStartWoDataTraceWorkFlowConfigValueViewConfig : WebViewConfig<AutoStartWoDataTraceWorkFlowConfigValue>
    {
        protected override void ConfigView()
        {
            View.Property(p => p.IsAutoStart).Show(ShowInWhere.All);
        }
    }
}
