using SIE.ESop.Configs;

namespace SIE.Wpf.ESop.Configs
{
    /// <summary>
    /// 文档服务器配置视图配置
    /// </summary>
    internal class AttachmentConfigViewConfig : WPFViewConfig<AttachmentConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.MaxSize).UseSpinEditor(p => p.MinValue = 1);
            View.Property(p => p.MappingSheetRegular);
            View.Property(p => p.ItemSheet);
            View.Property(p => p.WorkOrderSheet);
            View.Property(p => p.UseCom);
        }
    }
}