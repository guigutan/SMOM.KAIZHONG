using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 业务类型
    /// </summary>
    public enum RequisitionType
	{
		/// <summary>
		/// 资产领用
		/// </summary>
		[Label("资产领用")]
		Consume = 10,
		/// <summary>
		/// 资产借用
		/// </summary>
		[Label("资产借用")]
		Borrow = 20,
	}
}