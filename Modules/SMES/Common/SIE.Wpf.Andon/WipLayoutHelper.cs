using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using SIE.Wpf.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SIE.Wpf.Andon
{
    /// <summary>
    /// 采集布局帮助类
    /// </summary>
    public static class WipLayoutHelper
    {
        /// <summary>
        /// 设置容器边距
        /// </summary>
        /// <param name="control">控件</param>
        public static void SetPanelMargin(LayoutControl control)
        {
            if (control == null)
            {
                return;
            }
            var groups = control.Children.OfType<FrameworkElement>().Where(p => p.GetType() == typeof(LayoutGroup)).ToList();
            for (int i = 0; i < groups.Count; i++)
            {
                if (i == 0)
                {
                    continue; //第一个组为提示信息框，不要设置边距
                }
                var formPanel = groups[i].GetLogicalChild<FormPanel>();
                if (formPanel != null)
                {
                    formPanel.Margin = new Thickness(0, -5, 0, -5);
                }
            }
        }


        /// <summary>
        /// 重置界面子控件元素样式
        /// </summary>
        /// <param name="parent"></param>
        public static void ResizeChildrenStyle(DependencyObject parent)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is SimpleButton)
                {
                    var item = child as SimpleButton;
                    item.FontSize = 14;
                    item.Height = 36;
                    item.Width = 100;
                    if (item.Content.ToString() == "确定".L10N() || item.Content.ToString() == "取消".L10N())
                    {

                        item.FontSize = 16;
                        item.Height = 40;
                        item.Width = 100;
                    }
                }
                if (child is Label)
                {
                    var item = child as Label;
                    item.FontSize = 14;
                }
                else if (child is TextEdit)
                {
                    var item = child as TextEdit;
                    item.FontSize = 14;
                    item.Height = 36;
                }
                ResizeChildrenStyle(child); // 递归查找子元素。
            }
        }
    }
}