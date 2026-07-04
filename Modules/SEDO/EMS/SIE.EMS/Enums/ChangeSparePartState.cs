using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 备件更换状态
    /// </summary>
    public enum ChangeSparePartState
    {
        /// <summary>
        /// 新建
        /// </summary>
        [Label("新建")]
        New = 0,

        /// <summary>
        /// 完成
        /// </summary>
        [Label("完成")]
        Finished = 1,

    }
}
