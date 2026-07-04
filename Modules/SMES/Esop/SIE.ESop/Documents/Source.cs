using SIE.ObjectModel;

namespace SIE.ESop.Documents
{
    /// <summary>
    /// 来源
    /// </summary>
    public enum Source
    {
        /// <summary>
        /// 手工来源
        /// </summary>
        [Label("手工")]
        Manual = 0,

        /// <summary>
        /// 外部程序
        /// </summary>
        [Label("程序")]
        ByProgram = 1,

        /// <summary>
        /// 接口
        /// </summary>
        [Label("接口")]
        ByInterface = 2
    }
}