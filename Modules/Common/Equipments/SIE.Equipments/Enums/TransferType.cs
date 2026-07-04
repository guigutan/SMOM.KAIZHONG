using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 业务类型
    /// </summary>
    public enum TransferType
	{
		/// <summary>
		/// 工厂内转移
		/// </summary>
		[Label("工厂内转移")]
		InsideFactory = 10,
		/// <summary>
		/// 跨工厂调拨
		/// </summary>
		[Label("跨工厂调拨")]
		AcrossFactory = 20,
	}
}