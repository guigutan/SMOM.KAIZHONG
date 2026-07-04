using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.Tech.Routings.DesignPropertys
{
    /// <summary>
    /// ActivityProperty.xaml 的交互逻辑
    /// </summary>
    public partial class ActivityProperty : UserControl
    {
        /// <summary>
        /// 工序属性
        /// </summary>
        public ActivityProperty()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化选中工序属性
        /// </summary>
        /// <param name="activity">当前选中</param>
        /// <param name="state">工艺流程状态</param>
        public void InitActivityProperty(IActivity activity, RoutingState? state)
        {
            if (activity == null || activity.Type != ActivityType.Interaction)
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

            this.DataContext = activity;
        }
    }
}