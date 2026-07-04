using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
	/// <summary>
	/// 润滑状态
	/// </summary>
   public enum LubricationStatus
    {
		/// <summary>
		/// 待执行
		/// </summary>
		[Label("待执行")]
		Pending = 10,
		/// <summary>
		/// 执行中
		/// </summary>
		[Label("执行中")]
		Doing = 20,
		/// <summary>
		/// 已完成
		/// </summary>
		[Label("已完成")]
		Done = 30,
		/// <summary>
		/// 关闭
		/// </summary>
		[Label("关闭")]
		Closed = 40,
	}
}
