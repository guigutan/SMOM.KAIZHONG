using SIE.Packages;
using System.Linq;
using System.Windows.Controls;

namespace SIE.Wpf.Packages.Packings.Layouts
{
    /// <summary>
    /// PackingRelationLayout.xaml 的交互逻辑
    /// </summary>
    public partial class PackingRelationLayout : UserControl, ILayoutControl
    {
        /// <summary>
        /// 包装关系布局
        /// </summary>
        public PackingRelationLayout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="components">UI组件</param>
        public void Arrange(UIComponents components)
        {
            criteriaContainer.Content = components.Children.FirstOrDefault(f => typeof(PackingRelation).IsAssignableFrom(f.ControlResult.MainView.EntityType))?.ControlResult?.Control;
        }
    }
}
