using DevExpress.Xpf.LayoutControl;
using SIE.Wpf.Controls;
using System.Linq;
using System.Windows;

namespace SIE.Wpf.MES.WIP
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
    }
}