using SIE.ObjectModel;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
	/// 换料后处理方式
	/// </summary>
    public enum ChangeItemHandleMethod
    {
        /// <summary>
		/// 置换后作废
		/// </summary>
		[Label("置换后作废")]
        Scrap = 10,

        /// <summary>
		/// 置换后正常下料
		/// </summary>
		[Label("置换后正常下料")]
        Recycle = 20,

        /// <summary>
        /// 置换后不良下料
        /// </summary>
        [Label("置换后不良下料")]
        NGRecycle = 30,
    }
}
