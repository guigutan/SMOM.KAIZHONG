using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.Tech.Routings.DesignPropertys
{
    /// <summary>
    /// RuleProperty.xaml 的交互逻辑
    /// </summary>
    public partial class RuleProperty : UserControl
    {
        /// <summary>
        /// 线条规则属性控件
        /// </summary>
        public RuleProperty()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化线条属性
        /// 工艺路线版本为发布状态不能修改属性
        /// </summary>
        /// <param name="rule">规则接口</param>
        /// <param name="state">工艺路线版本状态</param>
        public void InitRuleProperty(IRule rule, RoutingState? state)
        {
            if (rule == null)
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

            DataContext = rule;
        }
    }
}
