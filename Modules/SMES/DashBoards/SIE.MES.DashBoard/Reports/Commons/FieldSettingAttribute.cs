using System;

namespace SIE.MES.DashBoard.Reports.Commons
{
    /// <summary>
    /// 属性在Pivot Grid中显示配置
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class FieldSettingAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="area"></param>
        /// <param name="areaIndex"></param>
        public FieldSettingAttribute(string caption, FieldArea area, int areaIndex)
        {
            this.Caption = caption;
            this.Area = area;
            this.AreaIndex = areaIndex;
            this.SummaryType = SummaryType.Average;
        }

        /// <summary>
        /// 属性标题
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 显示的区域
        /// </summary>
        public FieldArea Area { get; set; }

        /// <summary>
        /// 排列顺序
        /// </summary>
        public int AreaIndex { get; set; }

        /// <summary>
        /// 合计计算类型（只对行属性有效）
        /// </summary>
        public SummaryType SummaryType { get; set; }
    }

    /// <summary>
    /// 合计计算类型
    /// </summary>
    public enum SummaryType
    {
        Count = 0,

        Sum = 1,

        Min = 2,

        Max = 3,

        Average = 4,

        StdDev = 5,

        StdDevp = 6,

        Var = 7,

        Varp = 8,

        Custom = 9
    }

    public enum FieldArea
    {
        /// <summary>
        /// 行区域
        /// </summary>
        RowArea = 0,

        /// <summary>
        /// 列区域
        /// </summary>
        ColumnArea = 1,

        /// <summary>
        /// 过滤区域
        /// </summary>
        FilterArea = 2,

        /// <summary>
        /// 数据区域
        /// </summary>
        DataArea = 3
    }
}
