using SIE.Barcodes.Configs;

namespace SIE.Wpf.Barcodes.Configs
{
    /// <summary>
    /// 归属工单验证配置值视图配置
    /// </summary>
    public class ValidationConfigValueViewConfig : WPFViewConfig<BelongWoValidationConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.ValidationModel).UseEnumEditor();
        }
    }
}
