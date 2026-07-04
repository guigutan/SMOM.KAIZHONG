using SIE.MES.Projects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Projects.ApiModels
{
    /// <summary>
    /// 工序标准参数明细数据
    /// </summary>
    [Serializable]
    public class ProStdParamDtlInfo
    {
        /// <summary>
        /// 项目参数Id
        /// </summary>
        public double ProjectParamId { get; set; }

        /// <summary>
        /// 参数值类型
        /// </summary>
        public ProcessStDtlValueType ProcessStDtlValueType { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 标准值
        /// </summary>
        public string SingleValue { get; set; }

        /// <summary>
        /// 上限值
        /// </summary>
        public decimal? RangeMaxValue { get; set; }

        /// <summary>
        /// 下限值
        /// </summary>
        public decimal? RangeMinValue { get; set; }

        /// <summary>
        /// 工序标准参数Id
        /// </summary>
        public double ProcessStandardParamId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId { get; set; }

    }
}
