using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.MES.LoadItems.Enum
{
    /// <summary>
    /// 工单消耗量单据类型枚举
    /// </summary>
    public enum WoCostItemType
    {
        /// <summary>
        /// 工单消耗
        /// </summary>
        [Label("工单消耗")]
        [Category("IsNew")]
        WoCost = 10,

        /// <summary>
        /// 工单消耗-超BOM
        /// </summary>
        [Label("工单消耗-超BOM")]
        [Category("IsNew")]
        OverBom = 20,

        /// <summary>
        /// 物料倒扣
        /// </summary>
        [Label("物料倒扣")]
        DeductItem = 30,
    }
}
