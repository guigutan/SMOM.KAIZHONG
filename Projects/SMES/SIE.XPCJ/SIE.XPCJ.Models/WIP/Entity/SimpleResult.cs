using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP.Entity
{
    /// <summary>
    /// 简单结果返回
    /// </summary>
    [Serializable]
    public class SimpleResult
    {
        /// <summary>
        /// 字符结果1
        /// </summary>
        public string Msg1 { get; set; }
        /// <summary>
        /// 字符结果2
        /// </summary>
        public string Msg2 { get; set; }
    }
}
