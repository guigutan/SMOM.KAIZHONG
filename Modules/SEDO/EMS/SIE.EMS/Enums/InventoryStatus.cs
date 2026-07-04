using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 盘点状态
    /// </summary>
    public enum InventoryStatus
    {
        /// <summary>
        /// 未盘点
        /// </summary>
        [Label("未盘点")]
        Not = 10,

        /// <summary>
        /// 部分盘点
        /// </summary>
        [Label("部分盘点")]
        Part = 15,
        /// <summary>
        /// 已盘点
        /// </summary>
        [Label("已盘点")]
        Done = 20,
    }
}