using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 维修措施
    /// </summary>
    public class EdgeRepairMeasure
    {
        /// <summary>
        /// 维修措施Id
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
