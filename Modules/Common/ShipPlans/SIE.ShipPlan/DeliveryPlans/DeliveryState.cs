using SIE.ObjectModel;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 状态
    /// </summary>
    public enum DeliveryState
	{
		/// <summary>
		/// 创建
		/// </summary>
		[Label("创建")]
		Created = 10,

		/// <summary>
		/// 审核
		/// </summary>
		[Label("审核")]
		Aduited = 20,

		/// <summary>
		/// 执行中
		/// </summary>
		[Label("执行中")]
		Executing = 30,

		/// <summary>
		/// 已完成
		/// </summary>
		[Label("已完成")]
		Finished = 40,

		/// <summary>
		/// 取消
		/// </summary>
		[Label("取消")]
		Cancel = 50,
	}
}
