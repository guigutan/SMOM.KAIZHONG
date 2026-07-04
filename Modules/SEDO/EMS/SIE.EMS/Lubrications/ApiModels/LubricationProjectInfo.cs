using System;

namespace SIE.EMS.Lubrications.ApiModels
{
    /// <summary>
    /// 润滑项目信息
    /// </summary>
    [Serializable]
    public class LubricationProjectInfo
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 部位
        /// </summary>
        public string Part { get; set; }

        /// <summary>
        /// 润滑方式
        /// </summary>
        public string LubricatingType { get; set; }

        /// <summary>
        /// 项目耗材
        /// </summary>
        public string Consumable { get; set; }

        /// <summary>
        /// 操作方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 加油量上限
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 加油量下限
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 实际加油量
        /// </summary>
        public decimal? ActualValue { get; set; }

        /// <summary>
        /// 延期天数
        /// </summary>
        public int? DelayDays { get; set; }

        /// <summary>
        /// 润滑项目ID
        /// </summary>
        public double ProjectId { get; set; }
    }
}
