using SIE.ObjectModel;

namespace SIE.Core.Common
{
    /// <summary>
    /// 指令类型
    /// </summary>
    public enum InstructType
    {
        /// <summary>
        /// 入库
        /// </summary>
        [Label("入库")]
        In,

        /// <summary>
        /// 出库
        /// </summary>
        [Label("出库")]
        Out,

        /// <summary>
        /// 移库
        /// </summary>
        [Label("移库")]
        Move
    }
}
