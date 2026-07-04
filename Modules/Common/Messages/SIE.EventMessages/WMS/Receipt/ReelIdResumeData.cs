using System;

namespace SIE.EventMessages
{
    /// <summary>
    /// 标签履历数据
    /// </summary>
    public class ReelIdResumeData
    {
        /// <summary>
        /// 场景
        /// </summary>
        public string Scene { get; set; }

        /// <summary>
        /// 作业人
        /// </summary>
        public string Worker { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime WorkTime { get; set; }
    }
}
