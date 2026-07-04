using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 采购类型
    /// </summary>
    public enum PurchaseType
	{
		/// <summary>
		/// 项目采购
		/// </summary>
		[Label("项目采购")]
		ByProject = 10,
		/// <summary>
		/// 非项目采购
		/// </summary>
		[Label("非项目采购")]
		NoneProject = 20,
	}
}