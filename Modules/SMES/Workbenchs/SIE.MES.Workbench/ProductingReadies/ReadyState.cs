using SIE.ObjectModel;

namespace SIE.MES.Workbench.ProductingReadies
{
    /// <summary>
    /// 准备状态
    /// </summary>
    public enum ReadyState
    {
        /// <summary>
        /// OK
        /// </summary>
        [Label("OK")]
        OK,

        /// <summary>
        /// NG
        /// </summary>
        [Label("NG")]
        NG
    }
}