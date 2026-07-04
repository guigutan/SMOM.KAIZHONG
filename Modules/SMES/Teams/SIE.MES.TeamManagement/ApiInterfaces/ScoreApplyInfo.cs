using System;
using System.Collections.Generic;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 评分填写API信息类
    /// </summary>
    [Serializable]
    public class ScoreApplyInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScoreApplyInfo()
        {
            EmployeeIdList = new List<double>();
            AttachmentList = new List<AttachmentInfo>();
        }

        /// <summary>
        /// 员工Id列表
        /// </summary>
        public List<double> EmployeeIdList { get; set; }

        /// <summary>
        /// 评分项目Id
        /// </summary>
        public double RatedItemId { get; set; }

        /// <summary>
        /// 评分分值
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime OccurDate { get; set; }

        /*/// <summary>
        /// 发起人Id
        /// </summary>
        public double InitiatorId { get; set; }*/

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 附件列表
        /// </summary>
        public List<AttachmentInfo> AttachmentList { get; set; }
    }
}
