using System;

namespace SIE.WorkBenchChartBase.Commons
{
    /// <summary>
    /// 图表对应图片标签
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ChartDefinitionAttribute : Attribute
    {
        /// <summary>
        /// 有参构造函数
        /// </summary>
        ///<param name="category">控件分类</param>
        ///<param name="title">标题</param>
        public ChartDefinitionAttribute(string category, string title)
        {
            Title = title;
            Category = category;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ChartDefinitionAttribute() { }

        /// <summary>
        /// 控件标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 目标参数类型
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 目标参数类型
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 所属模块
        /// </summary>
        public ModuleFlag Module { get; set; }

        /// <summary>
        /// 分组页签名称
        /// </summary>
        public string GroupName { get; set; }
    }

    /// <summary>
    /// 模块标识
    /// </summary>
    [Flags]
    public enum ModuleFlag
    {
        /// <summary>
        /// all
        /// </summary>
        ALL = 0,
        /// <summary>
        /// aps
        /// </summary>
        APS = 1,
        /// <summary>
        /// mes
        /// </summary>
        MES = 2,
        /// <summary>
        /// qms
        /// </summary>
        QMS = 4,
        /// <summary>
        /// wms
        /// </summary>
        WMS = 8,
    }

    /// <summary>
    /// 分类标识
    /// </summary>
    [Flags]
    public enum CategoryFlag
    {
        /// <summary>
        /// 全部
        /// </summary>
        全部 = 0,
        /// <summary>
        /// 关键指标
        /// </summary>
        关键指标 = 1,
        /// <summary>
        /// 关键管理事项
        /// </summary>
        关键管理事项 = 2,
    }
}

