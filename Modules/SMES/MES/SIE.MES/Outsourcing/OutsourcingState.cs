using SIE.ObjectModel;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    /// 状态
    /// </summary>
    public enum OutsourcingState
	{
        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
		Close = 0,

		/// <summary>
		/// 未开始
		/// </summary>
		[Label("未开始")]
		NotStarted = 10,
		/// <summary>
		/// 已完成
		/// </summary>
		[Label("已完成")]
		Completed = 20,
		/// <summary>
		/// 委外中
		/// </summary>
		[Label("委外中")]
		Outsourcing = 30,
	}
}