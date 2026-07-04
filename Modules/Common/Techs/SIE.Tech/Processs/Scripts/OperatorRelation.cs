using SIE.ObjectModel;

namespace SIE.Tech.Processs.Scripts
{
    /// <summary>
    /// 关系
    /// </summary>
    public enum OperatorRelation
    {
        /// <summary>
        /// 并且
        /// </summary>
        [Label("并且")]
        And = 5,

        /// <summary>
        /// 或者
        /// </summary>
        [Label("或者")]
        Or = 10,
    }
}