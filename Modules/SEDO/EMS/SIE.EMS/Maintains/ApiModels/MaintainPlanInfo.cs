using System;

namespace SIE.EMS.Maintains.ApiModels
{
    /// <summary>
    /// 保养计划
    /// </summary>
    [Serializable]
    public class MaintainPlanInfo
    {
        /// <summary>
        /// 保养计划id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 保养单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 计划开始执行时间
        /// </summary>
        public string PlanBeginDate { get; set; }

        /// <summary>
        /// 计划开始执行时间值
        /// </summary>
        public DateTime PlanBeginDateValue { get; set; }

        /// <summary>
        /// 计划结束执行时间
        /// </summary>
        public string PlanEndDate { get; set; }

        /// <summary>
        /// 计划结束执行时间值
        /// </summary>
        public DateTime PlanEndDateValue { get; set; }

        /// <summary>
        /// 项目数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 实际开始执行时间
        /// </summary>
        public string ActBeginDate { get; set; }

        /// <summary>
        /// 实际结束执行时间
        /// </summary>
        public string ActEndDate { get; set; }

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
