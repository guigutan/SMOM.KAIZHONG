using System;

namespace SIE.EMS.Checks.ApiModels
{
    /// <summary>
    /// 点检计划
    /// </summary>
    [Serializable]
    public class CheckPlanInfo
    {
        /// <summary>
        /// 点检计划id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 点检单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 项目数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 维度，0--日，1--白班，2--晚班
        /// </summary>
        public string CheckCycleType { get; set; }

        /// <summary>
        /// 计划执行时间
        /// </summary>
        public string CheckBeginDate { get; set; }

        /// <summary>
        ///计划执行结束时间
        /// </summary>
        public string CheckEndDate { get; set; }

        /// <summary>
        /// 确认部门Id
        /// </summary>
        public double? ConfirmDeptId { get; set; }

        /// <summary>
        /// 确认结果
        /// </summary>
        public int? ConfirmResult { get; set; } 

        /// <summary>
        /// 确认备注
        /// </summary>
        public string ConfirmNote { get; set; }
    }
}