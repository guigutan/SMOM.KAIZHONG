using SIE.ObjectModel;

namespace SIE.EMS.SpareParts.Enums
{
    /// <summary>
    /// 入库备件类型
    /// </summary>
    public enum StorePartType
	{
		/// <summary>
		/// 拆机件
		/// </summary>
		[Label("拆机件")]
		OldPart = 5,

		/// <summary>
		/// 原件
		/// </summary>
		[Label("原件")]
		NewPart = 10,
	}
}
