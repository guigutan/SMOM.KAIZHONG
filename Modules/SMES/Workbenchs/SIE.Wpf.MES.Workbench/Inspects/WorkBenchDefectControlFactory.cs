using System;

namespace SIE.Wpf.MES.Workbench.Inspects
{
    /// <summary>
    /// 工作台缺陷录入控件工厂
    /// </summary>
    public static class WorkBenchDefectControlFactory
    {
        /// <summary>
        /// 工作台缺陷控件类型
        /// </summary>
        static Type DefectControlType = typeof(WorkBenchDefectControl);

        /// <summary>
        /// 创建工作台缺陷控件类型
        /// </summary>
        /// <returns>WorkBenchDefectControl</returns>
        public static WorkBenchDefectControl CreateControl()
        {
            var ctl = Activator.CreateInstance(DefectControlType) as WorkBenchDefectControl;
            if (ctl == null)
                throw new PlatformException("{0}没有继承自WorkBenchDefectControl".L10nFormat(DefectControlType.GetQualifiedName()));
            return ctl;
        }
    }
}