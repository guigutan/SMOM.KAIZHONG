using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports.Enums
{
    /// <summary>
    /// 报工方式
    /// </summary>
    public enum SourceType
    {
        /// <summary>
        /// 其他
        /// </summary>
        [Label("其他")]
        Other = 0,
        /// <summary>
        /// 手工报工
        /// </summary>
        [Label("手工报工")]
        Report_Manual = 1,

        /// <summary>
        /// 扫码报工
        /// </summary>
        [Label("扫码报工")]
        Report_Scan = 2,

        /// <summary>
        /// IOT数采报工
        /// </summary>
        [Label("IOT数采")]
        Report_IOT = 3,

        /// <summary>
        /// 可疑品处理
        /// </summary>
        [Label("可疑品处理")]
        Report_SuspectProduct
    }
}
