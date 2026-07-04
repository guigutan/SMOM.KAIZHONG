using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘 班次信息
    /// </summary>
    [Serializable]
    public class EdgeShift
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

    }
}
