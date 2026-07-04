using SIE.ObjectModel;

namespace SIE.EMS.SpecialEquipment.Enums
{
    /// <summary>
    /// 定检状态
    /// </summary>
    public enum RegularInspectionStatus
	{
		/// <summary>
		/// 合格
		/// </summary>
		[Label("合格")]
		OK = 10,
		/// <summary>
		/// 不合格
		/// </summary>
		[Label("不合格")]
		NG = 20
	}
}