using SIE.ObjectModel;

namespace SIE.Resources.Skills
{
    /// <summary>
    /// 认证状态
    /// </summary>
    public enum AuthStatus
    {
        /// <summary>
        /// 有效
        /// </summary>
        [Label("有效")]
        Valid = 0,

        /// <summary>
        /// 失效
        /// </summary>
        [Label("失效")]
        Invalid = 1

    }
}
