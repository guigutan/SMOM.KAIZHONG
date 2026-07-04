using DevExpress.Xpf.Editors;
using SIE.Domain;
using SIE.Tech.Processs;
using SIE.Tech.Routings.Technologys;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;

namespace SIE.Wpf.Tech.Routings.Technologys.Controls
{
    /// <summary>
    /// Rule.xaml 的交互逻辑
    /// </summary>
    public partial class Rule : UserControl
    {
        /// <summary>
        /// 容器
        /// </summary>
        Container Container { get; set; }

        /// <summary>
        /// 规则
        /// </summary>
        public IRule Model { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="model">规则</param>
        public Rule(Container container, IRule model)
        {
            InitializeComponent();
            Container = container;
            Model = model;
            Model.Control = this;
            this.DataContext = model;
            this.MouseDoubleClick += ConfigExpression;
            SetContextMenu();
        }

        /// <summary>
        /// 设置右键菜单
        /// </summary>
        private void SetContextMenu()
        {
            if (Container.IsReadOnly) return;
            var contextMenu = new ContextMenu();
            var menuItem = new MenuItem();
            menuItem.Header = "删除";
            menuItem.DataContext = this;
            menuItem.Click += DeleteControls;

            if (Model.ParamResultType == ResultTypeForDesign.Custom)
            {
                var expressionItem = new MenuItem();
                expressionItem.Header = "脚本配置";
                expressionItem.DataContext = this;
                expressionItem.Click += ConfigExpression;
                contextMenu.Items.Add(expressionItem);
            }

            contextMenu.Items.Add(menuItem);
            this.ContextMenu = contextMenu;
        }

        /// <summary>
        /// 配置脚本
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        void ConfigExpression(object sender, RoutedEventArgs e)
        {
            if (Model.ParamResultType == ResultTypeForDesign.Custom)
            {
                var panel = new ScrollViewer()
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                };

                var editor = new TextEdit
                {
                    AcceptsReturn = true,
                    TextWrapping = TextWrapping.Wrap,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalContentAlignment = VerticalAlignment.Top,
                    MinHeight = 280,
                    Margin = new Thickness(2),
                    BorderThickness = new Thickness(1),
                };

                editor.SetBinding(TextEditBase.TextProperty, new Binding(nameof(RuleModel.Expression)) { Source = Model, Mode = BindingMode.TwoWay });
                panel.Content = editor;

                string key = Guid.NewGuid().ToString("N");
                CRT.Workbench.ShowDialog(key, panel, v =>
                {
                    v.Title = "脚本 - 配置".L10N();
                    v.Width = 650;
                    v.Height = 400;
                    v.DefaultButton = -2;
                    var originalText = editor.Text;

                    v.Closed += (o, arg) =>
                    {
                        if (v.Result != 0)
                        {
                            editor.Text = originalText;
                        }
                    };
                });

                editor = null;
            }
        }

        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        void DeleteControls(object sender, RoutedEventArgs e)
        {
            Container.Children.Remove(this);
            Container.Model.RemoveChild(Model);
        }

        /// <summary>
        /// 设置结束活动节点
        /// </summary>
        public void SetEndActivity()
        {
            foreach (var activity in Container.Model.Activitys.Where(p => p.Type != ActivityType.Initial))
            {
                if (activity.PointIsInside(Model.EndPoint))
                {
                    bool isExist = true;
                    foreach (var rule in activity.EndRules.Where(p => p.EndActivity == activity))
                    {
                        if (rule.BeginActivity == Model.BeginActivity)
                            isExist = false;
                    }

                    if (isExist)
                    {
                        Model.SetEndActivity(activity);
                    }
                }
            }
        }

        /// <summary>
        /// 鼠标左键点击规则节点时触发
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void BezierSegment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
                Container.Model.SelectedElement(Model, true, false);
            else
                Container.Model.SelectedElement(Model);
            e.Handled = true;
        }

        /// <summary>
        /// 规则1鼠标左键点击事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Ellipse1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Container.MoveEllipse1 = sender as Ellipse;
            Container.MoveEllipse1.Tag = Model;
            Container.MoveEllipse1.CaptureMouse();
            e.Handled = true;
        }

        /// <summary>
        /// 规则2鼠标左键点击事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Ellipse2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Container.MoveEllipse2 = sender as Ellipse;
            Container.MoveEllipse2.Tag = Model;
            Container.MoveEllipse2.CaptureMouse();
            e.Handled = true;
        }
    }
}
