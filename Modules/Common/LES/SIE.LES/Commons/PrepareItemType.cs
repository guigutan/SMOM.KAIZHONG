using SIE.ObjectModel;

namespace SIE.LES
{
    /// <summary>
    /// 备料方式
    /// </summary>
    public enum PrepareItemType
    {
        /// <summary>
		/// 拉式
		/// </summary>
		[Label("拉式")]
        Pull = 0,

        /// <summary>
		/// 推式
		/// </summary>
		[Label("推式")]
        Push = 1,

        /// <summary>
        /// 超BOM
        /// </summary>
        [Label("超BOM")]
        OverBom,
    }
}
