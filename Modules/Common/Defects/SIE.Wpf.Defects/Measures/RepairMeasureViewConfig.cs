using SIE.Defects.Measures;
using SIE.Domain;
using SIE.Wpf.Defects.Measures.Commands;

namespace SIE.Wpf.Defects.Measures
{
    /// <summary>
    /// 维修措施视图配置
    /// </summary>
    internal class RepairMeasureViewConfig : WPFViewConfig<RepairMeasure>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.UseDefaultBehaviors();
            View.ReplaceCommands(WPFCommandNames.ListCopy, typeof(RepairMeasureCopyCommand));
            View.Property(p => p.Code).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 配置下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
        }
    }
}