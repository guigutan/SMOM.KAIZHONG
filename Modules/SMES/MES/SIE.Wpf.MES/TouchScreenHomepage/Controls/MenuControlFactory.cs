using System;

namespace SIE.Wpf.MES.TouchScreenHomepage
{
    /// <summary>
    /// AndonButtonControlFactory
    /// </summary>
    public static class MenuControlFactory
    {
        /// <summary>
        /// 控件类型
        /// </summary>
        static Type ControlType = typeof(MenuControl);

        /// <summary>
        /// 创建控件类型
        /// </summary>
        /// <returns>AndonButtonControl</returns>
        public static MenuControl CreateControl()
        {
            var ctl = Activator.CreateInstance(ControlType) as MenuControl;
            if (ctl == null)
            {
                throw new PlatformException("{0}没有继承自MenuControl".L10nFormat(ControlType.GetQualifiedName()));
            }
            return ctl;
        }
    }
}
