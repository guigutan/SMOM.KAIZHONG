using SIE.ObjectModel;

namespace SIE.MES.WorkReportPlans
{
    /// <summary>
    /// 模板类型
    /// </summary>
    public enum TemplateNames
    {
        /// <summary>
        /// 通用类
        /// </summary>
        [Label("通用类")]
        General,
        /// <summary>
        /// 装备行业钣金-机加类
        /// </summary>
        [Label("装备行业钣金-机加类")]
        SheetMetal_Machining,

        /// <summary>
        /// 装备行业组装-焊接类
        /// </summary>
        [Label("装备行业组装-焊接类")]
        Assembly_Welding,

        /// <summary>
        /// 装备行业装配-包装类
        /// </summary>
        [Label("装备行业装配-包装类")]
        Assembly_Packaging,

       



    }
}
