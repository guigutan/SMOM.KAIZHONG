using SIE.MES.TeamManagement.ScoreRecords;
using System;
using System.Collections.Generic;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 评分记录API信息类
    /// </summary>
    [Serializable]
    public class ScoreRecordInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScoreRecordInfo()
        {
            ////AttachmentList = new List<byte[]>();
            AttachmentList = new List<AttachmentInfo>();
            PetitionList = new List<PetitionRecordInfo>();
        }

        /// <summary>
        /// 评分记录Id
        /// </summary>
        public double ScoreRecordId { get; set; }

        /// <summary>
        /// 评分项目Id
        /// </summary>
        public double RateItemId { get; set; }

        /// <summary>
        /// 评分项目名称
        /// </summary>
        public string RatedItemName { get; set; }

        /// <summary>
        /// 最低分值
        /// </summary>
        public decimal? MinScore { get; set; }

        /// <summary>
        /// 最高分值
        /// </summary>
        public decimal? MaxScore { get; set; }

        /// <summary>
        /// 评分状态
        /// </summary>
        public string ScoreState { get; set; }

        /// <summary>
        /// 评分状态值
        /// </summary>
        public ScoreState ScoreStateValue { get; set; }

        /// <summary>
        /// 项目分值
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime OccurDate { get; set; }

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 原评分项目Id
        /// </summary>
        public double OldRatedItemId { get; set; }

        /// <summary>
        /// 原评分项目名称
        /// </summary>
        public string OldRatedItemName { get; set; }

        /// <summary>
        /// 原项目分值
        /// </summary>
        public decimal OldScore { get; set; }

        /// <summary>
        /// 申诉处理方式 [调整评判  拒绝申诉  撤销评分]
        /// </summary>
        public string PetitionProcessMode { get; set; }

        /// <summary>
        /// 申诉处理方式值 [调整评判  拒绝申诉  撤销评分]
        /// </summary>
        public StateProcessMode? PetitionProcessModeValue { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 附件列表
        /// </summary>
        ////public List<byte[]> AttachmentList { get; set; }
        public List<AttachmentInfo> AttachmentList { get; set; }

        /// <summary>
        /// 申诉列表
        /// </summary>
        public List<PetitionRecordInfo> PetitionList { get; set; }
    }

    /// <summary>
    /// 评分记录API信息
    /// </summary>
    [Serializable]
    public class ScoreRecordInfos
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScoreRecordInfos()
        {
            ScoreRecordInfoList = new List<ScoreRecordInfo>();
        }

        /// <summary>
        /// 评分记录List集合
        /// </summary>
        public List<ScoreRecordInfo> ScoreRecordInfoList { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
    }
}