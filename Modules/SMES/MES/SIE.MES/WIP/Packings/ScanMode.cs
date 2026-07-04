using SIE.ObjectModel;

namespace SIE.MES.WIP.Packings
{
    /// <summary>
    /// 扫描方式
    /// </summary>
    public enum ScanMode
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Label("正常")]
        Normal,

        /// <summary>
        /// 加入
        /// </summary>
        [Label("加入")]
        Join,
    }
}