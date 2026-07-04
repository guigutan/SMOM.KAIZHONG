using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.Plans
{
    /// <summary>
    /// 设备点检(保养)类型
    /// </summary>
    public enum EquipCheckType
    {
        /// <summary>
        /// 设备
        /// </summary>
        [Label("设备")]
        Equip = 0,
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        Line = 1,
        /// <summary>
        /// 批量添加
        /// </summary>
        [Label("批量添加")]
        BatchAdd = 2,
    }
}
