using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.Tech.Routings.DesignPropertys
{
    /// <summary>
    /// ContainerProperty.xaml 的交互逻辑
    /// </summary>
    public partial class ContainerProperty : UserControl
    {
        /// <summary>
        /// 画布属性配置
        /// </summary>
        public ContainerProperty()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化画布属性控件
        /// 工艺路线版本发布状态不能修改属性
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="state">工艺路线状态</param>
        public void InitContainerProperty(IContainer container, RoutingState? state)
        {
            if (container == null)
            {
                Visibility = Visibility.Collapsed;
                DataContext = null;
                return;
            }

            Visibility = Visibility.Visible;
            if (state == RoutingState.Release)
            {
                propertyGrid.IsEnabled = false;
            }
            else
            {
                propertyGrid.IsEnabled = true;
            }

            this.DataContext = container;
        }
    }
}
