using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ControlChart.SpcUtils
{
    /// <summary>
    /// 剔除的数据
    /// </summary>
    [Serializable]
    public class TakeRecord
    {
        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime? DataTime { get; set; }
        /// <summary>
        /// 不合格品数
        /// </summary>
        public double? FailQty { get; set; }
        /// <summary>
        /// 缺陷数
        /// </summary>
        public double? DefectQty { get; set; }
        /// <summary>
        /// 样本数
        /// </summary>
        public double? SampleQty { get; set; }
        /// <summary>
        /// 测试值
        /// </summary>
        public double? CheckValue { get; set; }
    }
}
