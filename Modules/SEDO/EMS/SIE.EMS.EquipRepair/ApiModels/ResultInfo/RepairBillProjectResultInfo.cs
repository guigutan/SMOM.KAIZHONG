using System;

namespace SIE.EMS.EquipRepair.ApiModels.ResultInfo
{
    /// <summary>
    /// 维修规程返回实体
    /// </summary>
    [Serializable]
    public class RepairBillProjectResultInfo
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public double ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 部位
        /// </summary>
        public string Part { get; set; }

        /// <summary>
        /// 项目耗材
        /// </summary>
        public string Consumable { get; set; }

        /// <summary>
        /// 操作方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 标准
        /// </summary>
        public string Standard { get; set; }

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
        /// 用时(分钟)
        /// </summary>
        public string UseTime { get; set; }
    }
}
