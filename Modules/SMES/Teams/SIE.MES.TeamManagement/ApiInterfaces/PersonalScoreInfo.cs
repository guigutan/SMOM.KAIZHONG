using System;
using System.Collections.Generic;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 员工个人评分记录API信息类
    /// </summary>
    [Serializable]
    public class PersonalScoreInfo
    {
        /// <summary>
        /// 构造数
        /// </summary>
        public PersonalScoreInfo()
        {
            ScoreRecordInfoList = new List<ScoreRecordInfo>();
            AchieveLevelSetInfoList = new List<AchieveLevelSetInfo>();
        }

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 评分分值
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 评估开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 评估结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 评分等级 [优秀、良好、及格、不良四个评分等级]
        /// </summary>
        public string ScoreGrade { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 评分记录列表
        /// </summary>
        public List<ScoreRecordInfo> ScoreRecordInfoList { get; set; }

        /// <summary>
        /// 评分绩效等级配置列表
        /// </summary>
        public List<AchieveLevelSetInfo> AchieveLevelSetInfoList { get; set; }
    }
}
