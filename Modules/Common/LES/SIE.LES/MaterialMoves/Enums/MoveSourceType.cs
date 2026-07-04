using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialMoves.Enums
{
    /// <summary>
    /// 挪料来源类型
    /// </summary>
    public enum MoveSourceType
    {
        /// <summary>
        /// 手动
        /// </summary>
        [Label("手动")]
        Hand = 0,

        /// <summary>
        /// 自动
        /// </summary>
        [Label("自动")]
        Auto = 1,
    }
}
