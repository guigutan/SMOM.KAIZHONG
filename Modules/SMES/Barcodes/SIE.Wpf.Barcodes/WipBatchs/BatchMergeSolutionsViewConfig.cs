using SIE.Barcodes.WipBatchs;
using SIE.Wpf.Barcodes.WipBatchs.Commands;

namespace SIE.Wpf.Barcodes.WipBatchs
{
    /// <summary>
	/// 批次合并方案视图配置
	/// </summary>
	internal class BatchMergeSolutionsViewConfig : WPFViewConfig<BatchMergeSolutions>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete, WPFCommandNames.ListSave, typeof(SetDefaultCommand), typeof(CancelDefaultCommand));
            View.Property(p => p.Name).Show();
            View.Property(p => p.Description).Show();
            View.Property(p => p.IsDefault).Show().Readonly();
            View.ChildrenProperty(p => p.RuleList).Show(ChildShowInWhere.List).UseViewGroup(BatchMergeRuleViewConfig.ListView);
        }
    }
}