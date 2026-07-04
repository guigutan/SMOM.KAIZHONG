using System;

namespace SIE.EventMessages.AbnormalInfo
{
	/// <summary>
	/// 异常任务发布回调
	/// </summary>
	[Serializable]
    public class TaskCallEvent
    {
        /// <summary>
        /// 任务源key
        /// </summary> 
        public string PubKey { get; set; }
        /// <summary>
        /// 任务单号
        /// </summary>

        public string TaskNo { get; set; }

    }

    /// <summary>
    /// 异常任务处理
    /// </summary>
    [Serializable]
    public class TaskhandleEvent
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
        /// 任务处理
        /// </summary>

        public string HandelContent { get; set; }

    }

}
