using System;

namespace SIE.Wpf.Defects.Editors
{
    /// <summary>
    /// 缺陷责任控件工厂
    /// </summary>
    public static class ResponsibilityControlFactory
    {
        /// <summary>
        /// 缺陷责任控件类型
        /// </summary>
        static Type ResponsibilityControlType = typeof(ResponsibilityControl);

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <returns>缺陷责任控件</returns>
        public static ResponsibilityControl CreateControl()
        {
            var ctl = Activator.CreateInstance(ResponsibilityControlType) as ResponsibilityControl;
            if (ctl == null)
                throw new PlatformException("{0}没有继承自ResponsibilityControl".L10nFormat(ResponsibilityControlType.GetQualifiedName()));
            return ctl;
        }
    }
}