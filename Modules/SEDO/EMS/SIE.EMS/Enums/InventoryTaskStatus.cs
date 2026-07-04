using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 盘点任务状态
    /// </summary>
    public enum InventoryTaskStatus
	{
		/// <summary>
		/// 未开始
		/// </summary>
		[Label("未开始")]
		NotBegin = 10,
		/// <summary>
		/// 盘点中
		/// </summary>
		[Label("盘点中")]
		Doing = 20,
		/// <summary>
		/// 初盘完成
		/// </summary>
		[Label("初盘完成")]
		FirstDone = 30,
		/// <summary>
		/// 复盘中
		/// </summary>
		[Label("复盘中")]
		ScondDoing = 40,
		/// <summary>
		/// 已完成
		/// </summary>
		[Label("已完成")]
		Completed = 60,
		/// <summary>
		/// 关闭
		/// </summary>
		[Label("关闭")]
		Closed = 70,
	}
}