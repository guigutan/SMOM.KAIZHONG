using SIE.ObjectModel;

namespace SIE.MES.WIP.Reworks
{
    /// <summary>
	/// 置换后处理方式
	/// </summary>
    public enum ReplaceItemHandleMethod
    {
        /// <summary>
		/// 置换后作废
		/// </summary>
		[Label("置换后作废")]
        Scrap = 10,

        /// <summary>
		/// 置换后不良下料
		/// </summary>
		[Label("置换后不良下料")]
        Recycle = 20,
    }
}
