using SIE.Wpf.Andon.Controls;
using System;

namespace SIE.Wpf.MES.Controls
{
    /// <summary>
    /// AndonButtonControlFactory
    /// </summary>
    public static class AndonButtonControlFactory
    {
        /// <summary>
        /// 控件类型
        /// </summary>
        static Type ControlType = typeof(AndonButtonControl);

        /// <summary>
        /// 创建控件类型
        /// </summary>
        /// <returns>AndonButtonControl</returns>
        public static AndonButtonControl CreateControl()
        {
            var ctl = Activator.CreateInstance(ControlType) as AndonButtonControl;
            if (ctl == null)
            {
                throw new PlatformException("{0}没有继承自AndonButtonControl".L10nFormat(ControlType.GetQualifiedName()));
            }
            return ctl;
        }
    }
}
