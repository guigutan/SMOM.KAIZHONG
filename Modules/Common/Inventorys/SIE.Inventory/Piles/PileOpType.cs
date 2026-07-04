using SIE;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Piles
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum PileOpType
    {
        /// <summary>
        /// 创建
        /// </summary>
        [Label("创建")]
        Create = 1,

        /// <summary>
        /// 更新
        /// </summary>
        [Label("更新")]
        Upate = 2,

    }
}