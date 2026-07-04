using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 倒扣结果
    /// </summary>
    [Serializable]
    public class WoCostItemDeductResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        public int SuccessCount { get;   set; }
        
        /// <summary>
        /// 失败
        /// </summary>
        public int FailCount { get;   set; }
    }
}
