

using SIE.XPCJ.Models.Attributes;

namespace SIE.XPCJ.Models.Enums
{
    /// <summary>
    /// 库类型
    /// </summary>
    public enum LibraryType
    {
        /// <summary>
        /// 实体
        /// </summary>
        [Label("实体")]
        Entity = 0,

        /// <summary>
        /// 虚拟
        /// </summary>
        [Label("虚拟")]
        Fictitious = 1,
    }
}