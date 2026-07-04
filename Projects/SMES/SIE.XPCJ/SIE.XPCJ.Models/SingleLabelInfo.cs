using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models
{
    /// <summary>
    /// 单体条码信息
    /// </summary>
    [Serializable]
    public class SingleLabelInfo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double? ItemId { get; set; }

        /// <summary>
        /// 单体条码
        /// </summary>
        public string SingleLabel { get; set; }

        /// <summary>
        /// 是否系统外条码
        /// </summary>
        public bool IsExternal { get; set; }
    }
}
