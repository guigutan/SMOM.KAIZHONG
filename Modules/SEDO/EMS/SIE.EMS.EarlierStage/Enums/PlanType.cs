using SIE.ObjectModel;

namespace SIE.EMS.EarlierStage.Enums
{
    /// <summary>
    /// 计划类型
    /// </summary>
    public enum PlanType
    {
        /// <summary>
        /// 计划内
        /// </summary>
        [Label("计划内")]
        In = 10,
        /// <summary>
        /// 计划外
        /// </summary>
        [Label("计划外")]
        Out = 20,
    }
}