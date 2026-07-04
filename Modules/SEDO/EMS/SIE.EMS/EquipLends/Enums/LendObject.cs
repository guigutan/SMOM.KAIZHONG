using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipLends.Enums
{
    /// <summary>
    /// 借机对象
    /// </summary>
    public enum LendObject
    {
        /// <summary>
        /// 内部
        /// </summary>
        [Label("内部")]
        Internal = 0,

        /// <summary>
        /// 外部
        /// </summary>
        [Label("外部")]
        External = 1,
    }
}
