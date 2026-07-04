using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.ShiftSchedules.Models
{
    /// <summary>
    /// 星期
    /// </summary>
    [Flags]
    public enum Week
    {
        /// <summary>
        /// 星期一
        /// </summary>
        [Label("星期一")]
        Monday = 1,

        /// <summary>
        /// 星期二
        /// </summary>
        [Label("星期二")]
        Tuesday = 2,

        /// <summary>
        /// 星期三
        /// </summary>
        [Label("星期三")]
        Wednesday = 4,

        /// <summary>
        /// 星期四
        /// </summary>
        [Label("星期四")]
        Thursday = 8,

        /// <summary>
        /// 星期五
        /// </summary>
        [Label("星期五")]
        Friday = 16,

        /// <summary>
        /// 星期六
        /// </summary>
        [Label("星期六")]
        Saturday = 32,

        /// <summary>
        /// 星期日
        /// </summary>
        [Label("星期日")]
        Sunday = 64,
    }
}
