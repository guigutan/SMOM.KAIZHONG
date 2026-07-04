using SIE.ObjectModel;

namespace SIE.EMS.EquipRepairs.Enums
{
    /// <summary>
    /// 派工类型
    /// </summary>
    public enum EquipRepairWay
    {
        /// <summary>
		/// 内修
		/// </summary>
		[Label("内修")]
        InnerRepair = 0,

        /// <summary>
        /// 外修
        /// </summary>
        [Label("外修")]
        OuterRepair = 1,

    }
}
