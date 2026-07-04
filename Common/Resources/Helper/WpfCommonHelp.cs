using Resources.Controls;
using Resources.IconPacks;
using System.Windows;

namespace Resources.Helper
{
    /// <summary>
    /// 模板类
    /// </summary>
    public static class WpfCommonHelp
    {
        /// <summary>
        /// 获取状态栏内容模板
        /// </summary>
        /// <returns>新实例</returns>
        public static DataTemplate GetStatusBarContent()
        {
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(CopyrightStatusBar));
            factory.SetValue(PackIcon.MarginProperty, new Thickness());
            return new DataTemplate { VisualTree = factory };
        }

        /// <summary>
        /// 获取切换库存组织图标模板
        /// </summary>
        /// <returns>返回 null 时，默认用框架的 PackIcon Kind="Globe"</returns>
        public static DataTemplate GetChangedInvOrg()
        {
            return null;
        }
    }
}
