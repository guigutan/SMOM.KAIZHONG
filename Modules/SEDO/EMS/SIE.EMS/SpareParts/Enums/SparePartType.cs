using SIE.ObjectModel;

namespace SIE.EMS.SpareParts.Enums
{
    /// <summary>
    /// 备件类型
    /// </summary>
    public enum SparePartType
	{
		/// <summary>
		/// 专用备件
		/// </summary>
		[Label("专用备件")]
		Special = 5,
		/// <summary>
		/// 常规备件
		/// </summary>
		[Label("常规备件")]
		CommonPart = 10,
		/// <summary>
		/// 易耗备件
		/// </summary>
		[Label("易耗备件")]
		WearingPart = 20,
		/// <summary>
		/// 耗材类
		/// </summary>
		[Label("耗材类")]
		Consumables = 30,
		/// <summary>
		/// 工具类
		/// </summary>
		[Label("工具类")]
		Tool = 40,
	}
}