using SIE.Resources.ProcessTechTypes;

namespace SIE.Wpf.Resources.ProcessTechTypes
{
    /// <summary>
    /// 制程工艺类型
    /// </summary>
    public class ProcessTechTypeViewConfig : WPFViewConfig<ProcessTechType>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete, WPFCommandNames.ListSave);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.AlgorithmMarking);
                View.Property(p => p.Sequence);
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.AlgorithmMarking);
            }
        }
    }
}
