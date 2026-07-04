using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 单据来源
    /// </summary>
    public enum BillSourceType
	{
		/// <summary>
		/// 手工创建
		/// </summary>
		[Label("手工创建")]
		Manual = 10,
		/// <summary>
		/// 自动创建
		/// </summary>
		[Label("自动创建")]
		Automatically = 20,
	}
}