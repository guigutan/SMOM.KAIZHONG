using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using SIE.Domain.Validation;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Wpf.Helpers;
using SIE.Wpf.Resources.WipResources.Controls;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;

namespace SIE.Wpf.Resources.WipResources
{
    /// <summary>
    /// ResourceDockLayout.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceDockLayout : UserControl, ILayoutControl
    {
        /// <summary>
        /// 资源日历
        /// </summary>
        ChildLayout _childLayout;

        /// <summary>
        /// 生产资源列表逻辑视图
        /// </summary>
        ListLogicalView mainView;

        /// <summary>
        /// 生产资源布局构造函数
        /// </summary>
        public ResourceDockLayout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 重置布局
        /// </summary>
        /// <param name="components">容器</param>
        public void Arrange(UIComponents components)
        {
            mainContainer.Content = components.Main.Control;
            SetMainViewEvent(components);
            if (components.CommandsContainer != null)
                toolBarContainer.Content = components.CommandsContainer.Control;
            if (components.Condition != null)
                criteriaContainer.Content = components.Condition.Control;
            var children = components.Children;
            var childPanel = CreateLayoutPanel();
            mainGroup.Add(childPanel);
            var tab = new DXTabControl();
            tab.Margin = new Thickness(0, 0, 1, 1);
            tab.SnapsToDevicePixels = true;
            childPanel.Content = tab;
            foreach (var child in children)
            {
                var tabItem = CreateTabItem(child);
                tab.Items.Add(tabItem);
            }

            tab.Items.Insert(0, CreateCalendarTabItem());
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

            ////任何一个子 View 可见，整个控件都可见
            tab.Visibility = components.Main.MainView.ChildrenViews.Any(v => v.IsVisible)
                ? Visibility.Visible
                : Visibility.Collapsed;
            ////在选择状态发生改变时，设置每个view的Active状态
            tab.SelectionChanged += (sender, e) =>
            {
                ////设置每个view的Active状态
                var childView = WPFMeta.GetLogicalView(tab.SelectedContainer);
                if (childView != null)
                    childView.IsActive = true;
            };
        }

        /// <summary>
        /// 设置主表事件
        /// </summary>
        /// <param name="components">容器</param>
        private void SetMainViewEvent(UIComponents components)
        {
            mainView = components.Main.MainView as ListLogicalView;
            if (mainView != null)
            {
                mainView.CurrentChanged += (s, e) =>
                {
                    var resource = mainView.Current as WipResource;
                    if (_childLayout != null && _childLayout.calendar != null)
                    {
                        _childLayout.WipResource = resource;
                        _childLayout.RefreshSchemeWeek();
                    }
                };
            }
        }

        /// <summary>
        /// 创建布局容器
        /// </summary>
        /// <returns>布局容器</returns>
        LayoutPanel CreateLayoutPanel()
        {
            var panel = new LayoutPanel();
            panel.AllowClose = false;
            panel.AllowFloat = false;
            panel.ShowPinButton = false;
            panel.ShowBorder = false;
            panel.ShowCaption = false;
            panel.MinHeight = 100;
            panel.MinWidth = 100;
            return panel;
        }

        /// <summary>
        /// 创建子标签页
        /// </summary>
        /// <param name="child">子块</param>
        /// <returns>子标签页</returns>
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
            var view = child.ControlResult.MainView;
            tabItem.IsSelected = view.IsActive;
            view.IsActiveChanged += (o, e) => tabItem.IsSelected = view.IsActive;
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
        /// 创建日历方案标签页
        /// </summary>
        /// <returns>标签页</returns>
        DXTabItem CreateCalendarTabItem()
        {
            _childLayout = new ChildLayout();
            DXTabItem item = new DXTabItem() { Content = _childLayout };
            item.SetResourceBinding(DXTabItem.HeaderProperty, "资源日历".L10N());
            return item;
        }

        /// <summary>
        /// 设置布局宽度
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>布局宽度</returns>
        GridLength GetWidth(double value)
        {
            if (double.IsNaN(value))
                return GridLength.Auto;
            if (value < 0)
                return new GridLength(-value, GridUnitType.Star);
            return new GridLength(value, GridUnitType.Pixel);
        }
    }
}
