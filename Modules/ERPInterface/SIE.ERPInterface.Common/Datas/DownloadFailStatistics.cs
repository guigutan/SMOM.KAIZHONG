using SIE.ERPInterface.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 接口下载失败统计
    /// </summary>
    [Serializable]
    public class DownloadFailStatistics
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public JobType JobType { get; set; }

        /// <summary>
        /// 任务方向
        /// </summary>
        public JobDirection JobDirection { get; set; }

        /// <summary>
        /// 失败次数
        /// </summary>
        public int FailCount { get; set; }
    }
}
