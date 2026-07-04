using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Equipments.EquipAccountCarrieds
{
    /// <summary>
    /// 载位类型
    /// </summary>
    public enum CarriedType
    {
        /// <summary>
        /// 上板位
        /// </summary>
        [Label("上板位")]
        UpPanel = 0,

        /// <summary>
        /// 下板位
        /// </summary>
        [Label("下板位")]
        DownPanel = 1
    }
}
