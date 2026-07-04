using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.Data
{
    /// <summary>
    /// 点检设备值查询参数实体
    /// </summary>
    [Serializable]
    public class CheckPlanEapData
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 点检项目ID
        /// </summary>
        public List<double> ProjectDetailIds { get; set; }
    }
}
