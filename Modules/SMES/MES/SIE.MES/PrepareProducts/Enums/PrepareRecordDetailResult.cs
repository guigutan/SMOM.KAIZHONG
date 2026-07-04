using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PrepareProducts.Enums
{
    /// <summary>
    /// 产前准备记录子表结果
    /// </summary>
    public enum PrepareRecordDetailResult
    {
        /// <summary>
        /// 通过
        /// </summary>
        [Label("通过")]
        Pass = 0,

        /// <summary>
        /// 不通过
        /// </summary>
        [Label("不通过")]
        Fail = 1,
    }
}
