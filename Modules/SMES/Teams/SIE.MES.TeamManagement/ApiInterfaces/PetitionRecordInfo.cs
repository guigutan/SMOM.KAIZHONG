using SIE.MES.TeamManagement.ScoreRecords;
using System;
using System.Collections.Generic;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 申诉记录API信息基类
    /// </summary>
    [Serializable]
    public class PetitionInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PetitionInfo()
        {
            ////AttachmentList = new List<byte[]>();
            AttachmentList = new List<AttachmentInfo>();
        }

        /// <summary>
        /// 申诉人Id
        /// </summary>
        public double PetitionerId { get; set; }

        /// <summary>
        /// 申诉人姓名
        /// </summary>
        public string PetitionerName { get; set; }

        /// <summary>
        /// 申诉说明
        /// </summary>
        public string PetitionerRemark { get; set; }

        /// <summary>
        /// 附件列表
        /// 每个附件类型是byte[]
        /// </summary>
        ////public List<byte[]> AttachmentList { get; set; }
        public List<AttachmentInfo> AttachmentList { get; set; }

        /// <summary>
        /// 评分记录Id
        /// </summary>
        public double ScoreRecordId { get; set; }
    }

    /// <summary>
    /// 申诉记录API信息类
    /// </summary>
    [Serializable]
    public class PetitionRecordInfo : PetitionInfo
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 申诉时间
        /// </summary>
        public DateTime PetitionerDate { get; set; }

        /// <summary>
        /// 处理人Id
        /// </summary>
        public double? HandlerId { get; set; }

        /// <summary>
        /// 处理人姓名
        /// </summary>
        public string HandlerName { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? ProcessDate { get; set; }

        /// <summary>
        /// 处理方式 [调整评判  拒绝申诉  撤销评分]
        /// </summary>
        public string ProcessMode { get; set; }

        /// <summary>
        /// 处理方式 [调整评判  拒绝申诉  撤销评分]
        /// </summary>
        public StateProcessMode? ProcessModeValue { get; set; }

        /// <summary>
        /// 处理结果
        /// </summary>
        public string ProcessResult { get; set; }

        /// <summary>
        /// 评分项目Id
        /// </summary>
        public double RateItemId { get; set; }

        /// <summary>
        /// 评分项目名称
        /// </summary>
        public string RatedItemName { get; set; }

        /// <summary>
        /// 项目分值
        /// </summary>
        public decimal Score { get; set; }
    }
}