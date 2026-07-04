using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 副资产对象
    /// </summary>
    public enum ViceAssetObject
	{
		/// <summary>
		/// 备件
		/// </summary>
		[Label("备件")]
		Spare = 10,
		/// <summary>
		/// 工治具
		/// </summary>
		[Label("工治具")]
		Fixture = 20,
		/// <summary>
		/// 模具
		/// </summary>
		[Label("模具")]
		Molde = 30,
	}
}