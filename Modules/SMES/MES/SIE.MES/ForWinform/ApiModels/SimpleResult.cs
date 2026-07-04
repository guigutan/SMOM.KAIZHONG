using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ForWinform.ApiModels
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
