using SIE.ObjectModel;

namespace SIE.EMS.EquipRepairs.Enums
{
    /// <summary>
    /// 紧急程度
    /// </summary>
    public enum UrgentDegree
    {
        /// <summary>
		/// 紧急
		/// </summary>
		[Label("紧急")]
        Urgent = 0,

        /// <summary>
        /// 高
        /// </summary>
        [Label("高")]
        High = 1,

        /// <summary>
        /// 一般
        /// </summary>
        [Label("一般")]
        Common = 2,
    }
}
