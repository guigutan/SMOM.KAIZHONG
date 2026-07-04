using SIE.ObjectModel;

namespace SIE.Packages.Packings.Enums
{
    /// <summary>
    /// 包装规则兼容型验证方式（验证规格）
    /// </summary>
    public enum PackingRuleValidMode
    {
        /// <summary>
        /// 数量一致
        /// </summary>
        [Label("数量一致")]
        Current = 0,

        /// <summary>
        /// 相同包装规格
        /// </summary>
        [Label("相同包装规格")]
        Child = 1,

        /// <summary>
        /// 不验证
        /// </summary>
        [Label("不验证")]
        None = 3,
    }

    /// <summary>
    /// 自动打包方式
    /// </summary>
    public enum AutoDoPackingMode
    {
        /// <summary>
        /// 自动打包
        /// </summary>
        [Label("自动打包")]
        AutoPacking = 1,

        /// <summary>
        /// 自动级联打包
        /// </summary>
        [Label("自动级联打包")]
        AutoCasePacking = 2
    }

    /// <summary>
    /// 自动打包方式（直接）
    /// </summary>
    public enum DirectAutoDoPackingMode
    {
        /// <summary>
        /// 自动打包
        /// </summary>
        [Label("自动打包")]
        AutoPacking = 1,
    }
}