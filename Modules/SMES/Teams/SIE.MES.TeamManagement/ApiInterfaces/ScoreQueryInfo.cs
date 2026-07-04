using System;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 评分查询条件API信息类
    /// </summary>
    [Serializable]
    public class ScoreQueryInfo
    {
        /// <summary>
        /// 员工Id
        /// </summary>
        public double? EmployeeId { get; set; }

        /// <summary>
        /// 班组Id
        /// </summary>
        public double? WorkGroupId { get; set; }

        /// <summary>
        /// 项目分类Id
        /// </summary>
        public double? CategoryId { get; set; }

        /// <summary>
        /// 正负绩效
        /// 0为正绩效、1为负绩效
        /// </summary>
        public int? Performance { get; set; }

        /// <summary>
        /// 申诉状态
        /// 0为待申诉、1为申诉中、2为已处理
        /// </summary>
        public int? PetitionState { get; set; }

        /// <summary>
        /// 日期: 评分记录的发生时间OccurDate
        /// 0为最近三天、1为最近一周、2为最近一个月
        /// </summary>
        public int? QueryDate { get; set; }

        /// <summary>
        /// 页号
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }
    }
}
