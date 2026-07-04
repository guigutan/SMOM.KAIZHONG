using SIE.ObjectModel;

namespace SIE.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 折旧方式
    /// </summary>
    public enum DepreciationWay
	{
		/// <summary>
		/// 平均年限法
		/// </summary>
		[Label("平均年限法")]
		AverageAge = 5,

		/// <summary>
		/// 年限总和法
		/// </summary>
		[Label("年限总和法")]
		SumOfYears =10,

		/// <summary>
		/// 双倍余额递减法
		/// </summary>
		[Label("双倍余额递减法")]
		DoubleDecliningBalance = 15,
	}
}