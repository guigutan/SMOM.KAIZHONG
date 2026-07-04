using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.API.APIModels
{
    /// <summary>
    /// 设备运营成本分析
    /// </summary>
    [Serializable]
    public class EquipmentCostAnalysis
    {
        /// <summary>
        /// 成本项目
        /// </summary>
        public string CostName { get; set; }

        /// <summary>
        /// 成本额
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

    }
}
