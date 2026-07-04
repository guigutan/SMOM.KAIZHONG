using SIE.Common;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.AbnormalInfo
{
    /// <summary>
    /// 异常任务发布
    /// </summary>
    [Serializable]
    public class PubTaskEvent
    {
        /// <summary>
        /// 任务源key
        /// </summary> 
        public string PubKey { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string TaskDescription { get; set; }
    }

}
