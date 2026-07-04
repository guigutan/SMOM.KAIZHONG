using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.WIP.PackRecombine
{
    /// <summary>
    /// PackRecombineLayout.xaml 的交互逻辑
    /// </summary>
    public partial class PackRecombineLayout : UserControl, ILayoutControl
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public PackRecombineLayout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="components">UI组件</param>
        public void Arrange(UIComponents components)
        {
            if (components.CommandsContainer != null)
            {
                toolBar.Margin = new Thickness(5, 5, 5, 0);
                toolBar.Content = components.CommandsContainer.Control;
            }
            mainView.Content = components.Main.Control;
            mainView.Margin = new Thickness(-5, -10, -5, 0);
        }

        /// <summary>
        /// 初始化子视图
        /// </summary>
        /// <param name="tab">子页签</param>
        public void InitChildrenControl(DXTabControl tab)
        {
            childrenView.Content = tab;
        }

        /// <summary>
        /// 初始化子视图
        /// </summary>
        /// <param name="relationControl">包装关系控件</param>
        public void InitRelationControl(FrameworkElement relationControl)
        {
            packingRelation.Content = relationControl;
        }
    }
}