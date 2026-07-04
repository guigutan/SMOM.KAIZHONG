using SIE.Kit.MES.CallMaterials;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料原因视图配置
    /// </summary>
    internal class UrgencyCallMeterialReasonViewConfig : WebViewConfig<UrgencyCallMeterialReason>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.BillNo).HasLabel("叫料单号");
            View.Property(p => p.ReasonName).HasLabel("叫料原因");
        }
    }
}
