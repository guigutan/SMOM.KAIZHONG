using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 生成异常任务事件
    /// </summary>
    [Serializable]
    public class GenerateTaskEvent
    {
        /// <summary>
        /// 预警定义Id
        /// </summary>
        public double AbnormalDefineId { get; set; }

    }
}
