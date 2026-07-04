using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.MES.WorkOrders.Routings
{
    /// <summary>
    /// WorkOrderRoutingEdit.xaml 的交互逻辑
    /// </summary>
    public partial class WorkOrderRoutingEdit : UserControl
    {
        /// <summary>
        /// 工艺路线版本
        /// </summary>
        RoutingVersion routingVersion = null;

        /// <summary>
        /// 构造方法
        /// </summary>
        public WorkOrderRoutingEdit()
        {
            InitializeComponent();
            container.ModelChanged += Container_ModelChanged;
        }

        /// <summary>
        /// 初始化布局
        /// </summary>
        /// <param name="layout">布局</param>
        public void InitializeLayout(string layout)
        {
            container.LoadFromXmlString(layout);
        }

        /// <summary>
        /// 模型变更事件
        /// </summary>
        /// <param name="obj">工艺路线设计接口</param>
        private void Container_ModelChanged(IContainer obj)
        {
            if (obj != null)
            {
                obj.SelectedElementChanged += Model_SelectedElementChanged;
            }
        }

        /// <summary>
        /// 选择元素变更事件
        /// </summary>
        /// <param name="obj">元素接口</param>
        private void Model_SelectedElementChanged(IElement obj)
        {
            container.svContainer.Focus();
            activityProperty.InitActivityProperty(obj as IActivity, routingVersion?.State);
            ruleProperty.InitRuleProperty(obj as IRule, routingVersion?.State);
            containerProperty.InitContainerProperty(obj as IContainer, routingVersion?.State);
        }

        /// <summary>
        /// 水平居中命令
        /// </summary>
        public static RoutedUICommand HorizontalCenter = new RoutedUICommand();

        /// <summary>
        /// 垂直居中命令
        /// </summary>
        public static RoutedUICommand VerticalCenter = new RoutedUICommand();

        /// <summary>
        /// 横向分布命令
        /// </summary>
        public static RoutedUICommand HorizontalDistribution = new RoutedUICommand();

        /// <summary>
        /// 纵向分布命令
        /// </summary>
        public static RoutedUICommand VerticalDistribution = new RoutedUICommand();

        /// <summary>
        /// 控制纵向分布命令能否执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void VerticalDistribution_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (container == null)
            {
                return;
            }

            if (container.Model.SelectElements.Where(p => p is IActivity).Count() >= 3)
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 纵向分布命令
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void VerticalDistribution_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            container.Model.VerticalDistribution();
        }

        /// <summary>
        /// 控制横向分布命令能否执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void HorizontalDistribution_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (container == null)
            {
                return;
            }

            if (container.Model.SelectElements.Where(p => p is IActivity).Count() >= 3)
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 横向分布命令
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void HorizontalDistribution_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            container.Model.HorizontalDistribution();
        }

        /// <summary>
        /// 控制上下居中命令能否执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void VerticalCenter_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (container == null)
            {
                return;
            }

            if (container.Model.SelectElements.Where(p => p is IActivity).Count() > 1)
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 上下居中命令
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void VerticalCenter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            container.Model.VerticalCenter();
        }

        /// <summary>
        /// 控制左右居中命令能否执行
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void HorizontalCenter_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (container == null)
            {
                return;
            }

            if (container.Model.SelectElements.Where(p => p is IActivity).Count() > 1)
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 左右居中命令
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void HorizontalCenter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            container.Model.HorizontalCenter();
        }
    }
}