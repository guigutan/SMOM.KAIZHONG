using System;

namespace SIE.EventMessages.WorkFlows.QMS.UnqualifiedAudit
{
    /// <summary>
    /// 处理方式的参数Model
    /// </summary>
    [Serializable]
    public class ProcessModeExtModel
    {
        /// <summary>
        /// 是否触发PDCA
        /// </summary>
        public bool? IsPDCA { get; set; }
        /// <summary>
        /// 是否停线
        /// </summary>
        public bool? IsStopLine { get; set; }
        /// <summary>
        /// 是否锁批
        /// </summary>

        public bool? IsLockBatch { get; set; }

        /// <summary>
        /// 锁批维度
        /// </summary>
        public int? LockBatchMode { get; set; }

        /// <summary>
        /// 锁批设置
        /// </summary>
        public decimal? LockBatchSetting { get; set; }

        /// <summary>
        /// 锁定工步
        /// </summary>
        public double? LockedProcess { get; set; }
        /// <summary>
        /// 工步状态
        /// </summary>

        public int? ProcessState { get; set; }
    }
}
