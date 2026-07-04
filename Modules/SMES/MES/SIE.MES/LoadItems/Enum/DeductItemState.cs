using SIE.ObjectModel;

namespace SIE.MES.LoadItems
{
	/// <summary>
	/// 扣料状态
	/// </summary>
	public enum DeductItemState
	{
        /// <summary>
        /// 未扣料
        /// </summary>
        [Label("未扣料")]
        Create = 0,

        /// <summary>
        /// 成功
        /// </summary>
        [Label("成功")]
		Success = 10,

		/// <summary>
		/// 失败
		/// </summary>
		[Label("失败")]
		Fail = 20,

        /// <summary>
        /// 强制关闭
        /// </summary>
        [Label("强制关闭")]
        Close = 30,
    }
}