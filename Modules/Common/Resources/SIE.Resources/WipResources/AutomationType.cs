using SIE.ObjectModel;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 自动化类型
    /// </summary>
    public enum AutomationType
    {
        /// <summary>
        /// 全自动
        /// </summary>
        [Label("全自动")]
        FullAutomatic = 0,

        /// <summary>
        /// 半自动
        /// </summary>
        [Label("半自动")]
        SemiAutomatic = 10,

        /// <summary>
        /// 手动
        /// </summary>
        [Label("手动")]
        Manual = 20,
    }
}
