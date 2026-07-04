using SIE.ObjectModel;

namespace SIE.LES.MaterialPreparations.Enums
{
    /// <summary>
    /// 备料类型
    /// </summary>
    public enum PrepareType
    {
        /// <summary>
        /// 生产领料
        /// </summary>
        [Label("生产领料")]
        Pmw = 0,

        /// <summary>
        /// 生产超领
        /// </summary>
        [Label("生产超领")]
        Emc = 1,

        /// <summary>
        /// 车间领料
        /// </summary>
        [Label("车间领料")]
        Sfmr = 2,
    }
}
