using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models
{
    /// <summary>
    /// 绑定条码
    /// </summary>
    [Serializable]
    public class BindingSn
    {
        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
