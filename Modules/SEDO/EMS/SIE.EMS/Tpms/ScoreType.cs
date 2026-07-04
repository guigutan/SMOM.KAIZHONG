using SIE.ObjectModel;

namespace SIE.EMS.Tpms
{
    /// <summary>
    /// 类型
    /// </summary>
    public enum ScoreType
    {
        /// <summary>
        /// 点检
        /// </summary>
        [Label("点检")]
        Check = 0,

        /// <summary>
        /// 保养
        /// </summary>
        [Label("保养")]
        Maintain = 1,

        /// <summary>
        /// 维修
        /// </summary>
        [Label("维修")]
        Repair = 2,
    }
}
