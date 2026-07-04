using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.ApiModels
{
    /// <summary>
    /// 点检确认单生成所需要的实体参数
    /// </summary>
    [Serializable]
    public class CheckConfirmationGenerateInfo
    {
        /// <summary>
        /// 点检计划ID
        /// </summary>
        public double CheckPlanId { get; set; }

        /// <summary>
        /// 确认部门ID
        /// </summary>
        public double? ConfirmDeptId { get; set; }
    }
}
