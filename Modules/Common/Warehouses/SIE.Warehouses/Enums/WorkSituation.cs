using SIE.ObjectModel;

namespace SIE.Warehouses
{
    /// <summary>
	/// 在岗情况
	/// </summary>
	public enum WorkSituation
    {
        /// <summary>
        /// 在岗
        /// </summary>
        [Label("在岗")]
        OnDuty,

        /// <summary>
        /// 离岗
        /// </summary>
        [Label("离岗")]
        OffDuty,
    }
}
