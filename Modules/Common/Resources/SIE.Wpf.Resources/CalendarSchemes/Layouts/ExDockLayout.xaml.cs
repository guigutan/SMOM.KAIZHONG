using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using SIE.Domain.Validation;
using SIE.MetaModel.View;
using SIE.Wpf.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;

namespace SIE.Wpf.Resources.CalendarSchemes.Layouts
{
    /// <summary>
    /// DockLayout.xaml 的交互逻辑
    /// </summary>
    public partial class ExDockLayout : UserControl, ILayoutControl
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExDockLayout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 布局配置
        /// </summary>
        /// <param name="components">UI组件</param>
        public void Arrange(UIComponents components)
        {
            mainContainer.Content = components.Main.Control;
            if (components.CommandsContainer != null)
                toolBarContainer.Content = components.CommandsContainer.Control;
            if (components.Condition != null)
                criteriaContainer.Content = components.Condition.Control;
            else
                leftPanel.Visibility = Visibility.Collapsed;

            var children = components.Children;
            if (children.Count > 0)
            {
                var childPanel = CreateLayoutPanel();
                mainGroup.Add(childPanel);
                var tab = new DXTabControl
                {
                    Margin = new Thickness(0, 0, 1, 1),
                    SnapsToDevicePixels = true
                };
                childPanel.Content = tab;
                foreach (var child in children)
                {
                    var tabItem = CreateTabItem(child);
                    tab.Items.Add(tabItem);
                }

                ChildLayout childLayout = new ChildLayout(components.Main.MainView);
                var extTabItem = new DXTabItem() { Content = childLayout };
                extTabItem.SetResourceBinding(DXTabItem.HeaderProperty, "例外日历");
                tab.Items.Add(extTabItem);
                LayoutSize size = new LayoutSize(-4, -6);
                components.LayoutMeta.LayoutSize = size;
                var proportion = components.LayoutMeta.LayoutSize ?? LayoutSize.Default;
                if (components.LayoutMeta.IsLayoutChildrenHorizonal)
                {
                    mainGroup.Orientation = Orientation.Horizontal;
                    mainPanel.ItemWidth = GetWidth(proportion.Parent);
                    childPanel.ItemWidth = GetWidth(proportion.Children);
                }
                else
                {
                    mainPanel.ItemHeight = GetWidth(proportion.Parent);
                    childPanel.ItemHeight = GetWidth(proportion.Children);
                }

                //任何一个子 View 可见，整个控件都可见
                tab.Visibility = components.Main.MainView.ChildrenViews.Any(v => v.IsVisible) ?
                    Visibility.Visible : Visibility.Collapsed;

                //在选择状态发生改变时，设置每个view的Active状态
                tab.SelectionChanged += (sender, e) =>
                {
                    //设置每个view的Active状态
                    var childView = WPFMeta.GetLogicalView(tab.SelectedContainer);
                    if (childView != null)
                    {
                        childView.IsActive = true;
                    }
                };
            }
        }

        /// <summary>
        /// 获取宽度
        /// </summary>
        /// <param name="value">double</param>
        /// <returns>GridLength</returns>
        GridLength GetWidth(double value)
        {
            if (double.IsNaN(value))
                return GridLength.Auto;
            if (value < 0)
                return new GridLength(-value, GridUnitType.Star);
            return new GridLength(value, GridUnitType.Pixel);
        }

        /// <summary>
        /// 创建TabItem标签
        /// </summary>
        /// <param name="child">区域</param>
        /// <returns>DXTabItem</returns>
        DXTabItem CreateTabItem(Region child)
        {
            DXTabItem tabItem = new DXTabItem();

            var label = child.Label ?? child.ControlResult.MainView.Meta.Label;

            if (label.IsNullOrEmpty())
                throw new ValidationException("{0}没有设置Label属性".L10nFormat(child.ControlResult.MainView.Meta.EntityType));

            var tabHeader = new Label();
            tabHeader.SetResourceBinding(Label.ContentProperty, label);
            tabItem.Header = tabHeader;
            child.ControlResult.Control.Margin = new Thickness(-10);
            tabItem.Content = child.ControlResult.Control;
            AutomationProperties.SetName(tabItem, label);

            WPFMeta.SetLogicalView(tabItem, child.ControlResult.MainView);

            //IsActive
            var view = child.ControlResult.MainView;
            tabItem.IsSelected = view.IsActive;
            view.IsActiveChanged += (o, e) => tabItem.IsSelected = view.IsActive;

            //IsVisible
            tabItem.Visibility = view.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            view.IsVisibleChanged += (o, e) =>
            {
                var tabControl = tabItem.GetLogicalParent<DXTabControl>();
                Debug.Assert(tabControl != null, "tabControl != null");

                if (view.IsVisible)
                {
                    tabItem.Visibility = Visibility.Visible;
                    tabControl.Visibility = Visibility.Visible;
                }
                else
                {
                    tabItem.Visibility = Visibility.Collapsed;

                    if (tabControl.Items.OfType<DXTabItem>().All(i => i.Visibility == Visibility.Collapsed))
                    {
                        tabControl.Visibility = Visibility.Collapsed;
                    }
                }
            };
            return tabItem;
        }

        /// <summary>
        /// 创建布局容器
        /// </summary>
        /// <returns>LayoutPanel</returns>
        LayoutPanel CreateLayoutPanel()
        {
            var panel = new LayoutPanel
            {
                AllowClose = false,
                AllowFloat = false,
                ShowPinButton = false,
                ShowBorder = false,
                ShowCaption = false,
                MinHeight = 100,
                MinWidth = 100
            };
            return panel;
        }
    }
}