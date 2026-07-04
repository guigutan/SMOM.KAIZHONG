using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘班次信息
    /// </summary>
    public class EdgeShiftInfo
    {
        /// <summary>
        /// 下次请求时间（建议）
        /// </summary>
        public string NextRefreshTime { get; set; }

        /// <summary>
        /// 班次数据
        /// </summary>
        public List<EdgeShift> Datas { get; set; } = new List<EdgeShift>();
    }
}
