using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.MES.WIP.PackRecombine
{
    /// <summary>
    /// 扫描方式
    /// </summary>
    public enum ScanMode
    {
        /// <summary>
        /// 移除
        /// </summary>
        [Category("PACKLOG")]
        [Label("移除")]
        Move,

        /// <summary>
        /// 加入
        /// </summary>
        [Category("PACKLOG")]
        [Label("加入")]
        Join,

        /// <summary>
        /// 查询
        /// </summary>
        [Label("查询")]
        Search,
    }
}