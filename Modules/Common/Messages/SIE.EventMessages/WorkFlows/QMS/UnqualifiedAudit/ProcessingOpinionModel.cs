using SIE.Common.WorkFlow.Models;
using SIE.Core.Enums;
using System;

namespace SIE.EventMessages.WorkFlows.QMS.UnqualifiedAudit
{
    /// <summary>
    /// 填写处理意见
    /// </summary>
    [Serializable]
    public class ProcessingOpinionModel: TriggerModel
    {

        /// <summary>
        /// 处理方式
        /// </summary>
        public ProcessMode? ProcessMode { get; set; }

        /// <summary>
        /// 处理参数
        /// </summary>
        public ProcessModeExtModel ProcessModeExt { get; set; }

        /// <summary>
        /// 处理意见
        /// </summary>
        public string Suggestion { get; set; }

        /// <summary>
        /// 是否申请红牌
        /// </summary>
        public bool IsRedCard { get; set; }

        /// <summary>
        /// 申请红牌处理人
        /// </summary>
        public double? RedCardStarterId { get; set; }

        /// <summary>
        /// 发起改善
        /// </summary>
        public bool IsImprove { get; set; }

        /// <summary>
        /// 改善类型
        /// </summary>
        public ImproveType? ImproveType { get; set; }

        /// <summary>
        /// 改善处理人
        /// </summary>
        public double? ImproveHandlerId { get; set; }

        /// <summary>
        /// 选择的检验项目
        /// </summary>
        public string JoinDetailIds { get; set; }

        /// <summary>
        /// 选择的检验项目的显示
        /// </summary>
        public string JoinDetailDisplay { get; set; }
    }
}
