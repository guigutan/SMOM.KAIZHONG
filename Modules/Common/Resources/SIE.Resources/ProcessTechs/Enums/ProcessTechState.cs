using SIE.ObjectModel;

namespace SIE.Resources.ProcessTechs.Enums
{
    /// <summary>
    /// 制程工艺
    /// </summary>
    public enum ProcessTechState
    {
        /// <summary>
        /// 已排产
        /// </summary>
        [Label("是")]
        ARRANGE = 0,

        /// <summary>
        /// 未排产
        /// </summary>
        [Label("否")]
        UNARRANGE = 1,
    }
}
