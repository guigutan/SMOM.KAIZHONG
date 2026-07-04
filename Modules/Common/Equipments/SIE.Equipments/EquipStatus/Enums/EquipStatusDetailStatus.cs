using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Equipments.EquipStatus.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum EquipStatusDetailStatus
    {
        /// <summary>
        /// 待机
        /// </summary>
        [Label("待机")]
        Standby = 1,

        /// <summary>
        /// 运行
        /// </summary>
        [Label("运行")]
        Running = 2,

        /// <summary>
        /// 离线
        /// </summary>
        [Label("离线")]
        OffLine = 0
    }
}
