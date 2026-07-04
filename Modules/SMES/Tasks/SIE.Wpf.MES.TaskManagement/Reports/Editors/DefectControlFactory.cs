using System;

namespace SIE.Wpf.MES.TaskManagement.Reports.Editors
{
    /// <summary>
    /// DefectControlFactory
    /// </summary>
    public static class DefectControlFactory
    {
        /// <summary>
        /// 缺陷控件类型
        /// </summary>
        static Type DefectControlType = typeof(ReportDefectControl);

        /// <summary>
        /// 创建缺陷控件类型
        /// </summary>
        /// <returns>DefectControl</returns>
        public static ReportDefectControl CreateControl()
        {
            var ctl = Activator.CreateInstance(DefectControlType) as ReportDefectControl;
            if (ctl == null)
                throw new PlatformException("{0}没有继承自DefectControl".L10nFormat(DefectControlType.GetQualifiedName()));
            return ctl;
        }
    }
}
