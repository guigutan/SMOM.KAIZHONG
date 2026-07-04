using System;

namespace SIE.Wpf.MES.Controls
{
    /// <summary>
    /// 维修措施控件工厂
    /// </summary>
    public static class MeasureControlFactory
    {
        /// <summary>
        /// MeasureControlType
        /// </summary>
        static Type MeasureControlType = typeof(MeasureControl);

        /// <summary>
        /// CreateControl
        /// </summary>
        /// <returns>MeasureControl</returns>
        public static MeasureControl CreateControl()
        {
            var ctl = Activator.CreateInstance(MeasureControlType) as MeasureControl;
            if (ctl == null)
            {
                throw new PlatformException("{0}没有继承自MeasureControl".L10nFormat(MeasureControlType.GetQualifiedName()));
            }

            return ctl;
        }
    }
}
