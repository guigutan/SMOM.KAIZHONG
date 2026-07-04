using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Maintains.ApiModels
{
    /// <summary>
    /// 保养信息
    /// </summary>
    [Serializable]
    public class MaintainPlanProjectInfo
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
        /// 项目耗材
        /// </summary>
        public string Consumable { get; set; }

        /// <summary>
        /// 操作方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc { get; set; }

        /// <summary>
        /// 实际值
        /// </summary>
        public decimal? ActualValue { get; set; }

        /// <summary>
        /// 检验结果(0:NG,1:OK)
        /// </summary>
        public int? Result { get; set; }

        /// <summary>
        /// 点检项目ID
        /// </summary>
        public double ProjectId { get; set; }
    }
}