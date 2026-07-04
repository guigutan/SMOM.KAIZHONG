using SIE.ObjectModel;

namespace SIE.EMS.EquipRepairs.Enums
{
    /// <summary>
    /// 维修类型
    /// </summary>
    public enum EquipRepairType
    {
        /// <summary>
		/// 设备维修
		/// </summary>
		[Label("设备维修")]
        EquipRepair = 0,

        /// <summary>
        /// 备件维修
        /// </summary>
        [Label("备件维修")]
        SparePartRepair = 1,

    }
}
