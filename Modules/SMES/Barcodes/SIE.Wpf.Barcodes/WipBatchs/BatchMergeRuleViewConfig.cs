using SIE.Barcodes.WipBatchs;

namespace SIE.Wpf.Barcodes.WipBatchs
{
    /// <summary>
    /// 批次合并规则视图配置
    /// </summary>
    internal class BatchMergeRuleViewConfig : WPFViewConfig<BatchMergeRule>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WPFCommandNames.ListAdd);
            View.Property(p => p.MergeParam);
            View.Property(p => p.IsSelected);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}