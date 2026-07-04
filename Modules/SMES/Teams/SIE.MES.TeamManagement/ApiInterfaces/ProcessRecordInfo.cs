using System;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 处理记录API信息类
    /// </summary>
    [Serializable]
    public class ProcessRecordInfo
    {
        /// <summary>
        /// 评分记录Id
        /// </summary>
        public double ScoreRecordId { get; set; }

        /*/// <summary>
        /// 处理人Id
        /// </summary>
        public double HandlerId { get; set; }*/

        /// <summary>
        /// 处理方式 [0为调整、1为拒绝、2为撤销]
        /// </summary>
        public int ProcessMode { get; set; }

        /// <summary>
        /// 评分项目Id
        /// </summary>
        public double RateItemId { get; set; }

        /// <summary>
        /// 项目分值
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 处理结果
        /// </summary>
        public string ProcessResult { get; set; }
    }
}
