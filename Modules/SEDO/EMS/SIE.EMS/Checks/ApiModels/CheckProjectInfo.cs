using System;

namespace SIE.EMS.Checks.ApiModels
{
    /// <summary>
    /// 点检项目信息
    /// </summary>
    [Serializable]
    public class CheckProjectInfo
    {
        /// <summary>
        /// 点检项目ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 点检计划ID
        /// </summary>
        public double CheckPlanId { get; set; }

        /// <summary>
        /// 点检项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 点检结果
        /// </summary>
        public int? CheckResult { get; set; }


        /// <summary>
        /// 图片
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 实际值
        /// </summary>
        public int? Value { get; set; }

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
    }
}
