using System;

namespace SIE.EMS.Maintains.ApiModels
{
    /// <summary>
    /// 保养项目
    /// </summary>
    [Serializable]
    public class MaintainProjectInfo
    {
        /// <summary>
        /// 保养项目ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 保养计划ID
        /// </summary>
        public double MaintainPlanId { get; set; }

        /// <summary>
        /// 保养项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 实际值
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 保养结果
        /// </summary>
        public int? MaintainResult { get; set; }

        /// <summary>
        /// 保养周期
        /// </summary>
        public string Cycle { get; set; }

        /// <summary>
        /// 距离上次保养/天
        /// </summary>
        public int? LastTime { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
