using SIE.Defects.InspectionItems;
using SIE.Domain;

namespace SIE.Wpf.Defects.InspectionItems
{
    /// <summary>
    /// 检验方式视图配置
    /// </summary>
    internal class InspectionModeViewConfig : WPFViewConfig<InspectionMode>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.UseDefaultCommands();
        }

        /// <summary>
        /// 视图列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}