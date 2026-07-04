using SIE.ObjectModel;

namespace SIE.Defects
{
    /// <summary>
    /// 质量类型
    /// </summary>
    public enum QualityType
    {
        /// <summary>
        /// 原材料
        /// </summary>
        [Label("材料")]
        Item = 0,

        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品")]
        Product = 1,

        /// <summary>
        /// 通用
        /// </summary>
        [Label("通用")]
        Common = 4,

        /// <summary>
        /// 尺寸
        /// </summary>
        [Label("尺寸")]
        Size = 5,

        /// <summary>
        /// 外观
        /// </summary>
        [Label("外观")]
        Exterior = 6,

        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        Type = 7,

        /// <summary>
        /// 性能
        /// </summary>
        [Label("性能")]
        Performance = 8
    }
}