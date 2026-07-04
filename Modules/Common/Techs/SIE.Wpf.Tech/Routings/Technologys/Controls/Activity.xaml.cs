using SIE.Tech.Routings.Technologys;
using SIE.Utils;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace SIE.Wpf.Tech.Routings.Technologys.Controls
{
    /// <summary>
    /// Activity.xaml 的交互逻辑
    /// </summary>
    public partial class Activity : UserControl
    {
        /// <summary>
        /// 容器
        /// </summary>
        Container Container { get; set; }

        /// <summary>
        /// 活动节点
        /// </summary>
        public IActivity Model { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="model">活动节点</param>
        public Activity(Container container, IActivity model)
        {
            InitializeComponent();
            Model = model;
            Activity owner = this;
            model.Control = owner;
            Container = container;
            this.DataContext = model;
            SetContextMenu();
        }

        /// <summary>
        /// 设置右键菜单
        /// </summary>
        private void SetContextMenu()
        {
            if (Container.IsReadOnly) return;
            var contextMenu = new ContextMenu();
            var menuItem = new MenuItem
            {
                Header = "删除",
                DataContext = this
            };
            menuItem.Click += DeleteControls;
            contextMenu.Items.Add(menuItem);
            this.ContextMenu = contextMenu;
        }

        /// <summary>
        /// 删除活动节点
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        void DeleteControls(object sender, RoutedEventArgs e)
        {
            if (Model.Type != ActivityType.Interaction)
            {
                CRT.MessageService.ShowMessage("类型：{0} 不允许删除".L10nFormat(EnumViewModel.EnumToLabel(Model.Type).L10N()), "提示".L10N());
                return;
            }

            foreach (var rule in Model.BeginRules)
            {
                Container.Children.Remove(rule.Control as UIElement);
            }

            foreach (var rule in Model.EndRules)
            {
                Container.Children.Remove(rule.Control as UIElement);
            }

            foreach (var rule in Model.Rules)
            {
                var delRule = Container.Model.Rules.FirstOrDefault(p => p.Id == rule.Id);
                if (delRule != null)
                    Container.Model.Rules.Remove(delRule);
            }

            Container.Children.Remove(this);
            Container.Model.RemoveChild(Model);
        }

        /// <summary>
        /// 鼠标移动到活动节点时触发
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Container.TrackingMouseMove && e.LeftButton == MouseButtonState.Pressed && Container.TempRule == null)
            {
                this.CaptureMouse();
                FrameworkElement element = sender as FrameworkElement;
                element.Cursor = Cursors.Hand;
                var point = e.GetPosition(Container.cnsDesignerContainer);
                var oldPoint = Model.GetPoint();
                Container.Model.MoveSelectItems(point.X - oldPoint.X, point.Y - oldPoint.Y);
                Container.Scrollable();
            }
        }

        /// <summary>
        /// 鼠标左击回弹事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
        }

        /// <summary>
        /// 鼠标点击规则节点时触发，
        /// 创建规则
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void CenterEllipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (Container.TempRule == null && sender is Ellipse ellipse && ellipse.DataContext != null && ellipse.DataContext is IRule)
            {
                Container.TempRule = new Rule(Container, ellipse.DataContext as IRule);
                Container.TempRule.Model.SetBeginActivity(Model);
                Container.Children.Add(Container.TempRule);
                Container.TempRule.Model.StartPoint = new SIE.Tech.Routings.Technologys.Point(Model.Left, Model.Top);
                Container.TempRule.Model.EndPoint = new SIE.Tech.Routings.Technologys.Point(Model.Left, Model.Top);
                Container.Model.SelectedElement(Container.TempRule.Model);
                Container.TempRule.CaptureMouse();
            }
        }

        /// <summary>
        /// 鼠标点击规则节点时触发
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void CenterEllipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Container.TempRule == null)
                e.Handled = true;
            Container.TempRule = null;
        }

        /// <summary>
        /// 鼠标左键点击活动节点时触发
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Activity activity && activity.Model != null)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                    Container.Model.SelectedElement(activity.Model, true, false);
                else
                    Container.Model.SelectedElement(activity.Model);
                e.Handled = true;
            }
        }
    }
}
