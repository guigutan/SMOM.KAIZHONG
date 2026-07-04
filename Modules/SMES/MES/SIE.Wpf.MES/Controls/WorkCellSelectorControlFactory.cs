using System;

namespace SIE.Wpf.MES.Controls
{
    /// <summary>
    /// 创建控件的工厂
    /// </summary>
    public static class WorkCellSelectorControlFactory
    {
        /// <summary>
        /// 控件类型
        /// </summary>
        static Type WorkCellSelectorControlType = typeof(WorkCellSelectorControl);
        /// <summary>
        /// 控件类型
        /// </summary>
        static Type KZWorkCellSelectorControlType = typeof(KZWorkCellSelectorControl);
        /// <summary>
        /// 创建控件
        /// </summary>
        /// <returns>DefectControl</returns>
        public static WorkCellSelectorControl CreateControl()
        {
            var ctl = Activator.CreateInstance(WorkCellSelectorControlType) as WorkCellSelectorControl;
            if (ctl == null)
            {
                throw new PlatformException("{0}没有继承自WorkCellSelectorControl".L10nFormat(WorkCellSelectorControlType.GetQualifiedName()));
            }
            return ctl;
        }
        /// <summary>
        /// 创建控件
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformException"></exception>
        public static KZWorkCellSelectorControl CreateKZControl()
        {
            var ctl = Activator.CreateInstance(KZWorkCellSelectorControlType) as KZWorkCellSelectorControl;
            if (ctl == null)
            {
                throw new PlatformException("{0}没有继承自KZWorkCellSelectorControl".L10nFormat(KZWorkCellSelectorControlType.GetQualifiedName()));
            }
            return ctl;
        }


       
        /// <summary>
        ///  创建控件
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformException"></exception>
        public static WipResourceListSelectorControl CreateWipResourceListControl()
        {
            var ctl = Activator.CreateInstance(typeof(WipResourceListSelectorControl)) as WipResourceListSelectorControl;
            if (ctl == null) { throw new PlatformException("{0}没有继承自WipResourceListSelectorControl".L10nFormat(typeof(WipResourceListSelectorControl).GetQualifiedName())); }
            return ctl;
        }

    }
}
