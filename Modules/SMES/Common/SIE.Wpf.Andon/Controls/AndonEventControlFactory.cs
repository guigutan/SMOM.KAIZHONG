using SIE.Wpf.Andon.Controls;
using System;

namespace SIE.Wpf.MES.Controls
{
    /// <summary>
    /// AndonBarControlFactory
    /// </summary>
    public static class AndonEventControlFactory
    {
        /// <summary>
        /// 控件类型
        /// </summary>
        static Type ControlType = typeof(AndonEventControl);

        /// <summary>
        /// 创建控件类型
        /// </summary>
        /// <returns>DefectControl</returns>
        public static AndonEventControl CreateControl()
        {
            var ctl = Activator.CreateInstance(ControlType) as AndonEventControl;
            if (ctl == null)
            {
                throw new PlatformException("{0}没有继承自AndonEventControl".L10nFormat(ControlType.GetQualifiedName()));
            }
            return ctl;
        }
    }
}
