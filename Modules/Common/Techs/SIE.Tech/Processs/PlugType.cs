using SIE.ObjectModel;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 出入站类型
    /// </summary>
    public enum PlugType
    {
        /// <summary>
        /// 出站
        /// </summary>
        [Label("出站")]
        Out,

        /// <summary>
        /// 入站
        /// </summary>
        [Label("入站")]
        In,

        /// <summary>
        /// 移除入站
        /// </summary>
        [Label("移除入站")]
        Remove = 10,
    }
}