using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.MaterialReceptions.Enums
{
    /// <summary>
    /// 按单接收默认接收数量
    /// </summary>
    public enum ConfigValues
    {
        /// <summary>
        /// 全部接收
        /// </summary>
        [Label("全部接收")]
        AllAccept = 0,

        /// <summary>
        /// 接收数量为0
        /// </summary>
        [Label("接收数量为0")]
        NotAccept = 1,
    }
}
