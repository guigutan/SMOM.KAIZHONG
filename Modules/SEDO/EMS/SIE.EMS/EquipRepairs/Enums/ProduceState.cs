using SIE.ObjectModel;

namespace SIE.EMS.EquipRepairs.Enums
{
    /// <summary>
    /// 生产状态
    /// </summary>
    public enum ProduceState
    {
        /// <summary>
		/// 停机
		/// </summary>
		[Label("停机")]
        StopWork = 0,

        /// <summary>
        /// 生产
        /// </summary>
        [Label("生产")]
        Produce = 1,

    }
}
