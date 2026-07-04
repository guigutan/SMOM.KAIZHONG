using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 外部领用类型
    /// </summary>
    public enum ExternalType
	{
		/// <summary>
		/// 供应商
		/// </summary>
		[Label("供应商")]
		Supply = 10,
		/// <summary>
		/// 客户
		/// </summary>
		[Label("客户")]
		Customer = 20,
		/// <summary>
		/// 其他
		/// </summary>
		[Label("其他")]
		Other = 30,
	}
}