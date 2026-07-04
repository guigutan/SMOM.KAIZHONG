using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 工程确认信息实体
    /// </summary>
    [Serializable]
    public class EngineerConfirmInfo
    {
        /// <summary>
        /// 维修单ID
        /// </summary>
        public double RepairBillId { get; set; }

        /// <summary>
        /// 委外维修报告
        /// </summary>
        public string OutsourcedMaintenanceReport { get; set; }

        /// <summary>
        /// 故障原因编码
        /// </summary>
        public string FaultReasonCode { get; set; }

        /// <summary>
        /// 故障等级(0:致命，1:严重，2:轻微)
        /// </summary>
        public int? FaultLevel { get; set; }

        /// <summary>
        /// 维修费用
        /// </summary>
        public decimal? RepairCosts { get; set; }

        /// <summary>
        /// 故障类别ID
        /// </summary>
        public double? FaultCategoryId { get; set; }

        /// <summary>
        /// 故障部位
        /// </summary>
        public string FaultPart { get; set; }

        /// <summary>
        /// 维修类别
        /// 0:常见故障维修
        /// 1:一般故障维修
        /// 2:突发故障维修
        /// 3:不正当使用故障维修
        /// 4:计划性维修
        /// 5:预防性维修
        /// </summary>
        public int? RepairCategory { get; set; }

        /// <summary>
        /// 停机维修
        /// </summary>
        public bool? RepairDowntime { get; set; }

        /// <summary>
        /// 维修等级
        /// 0:大修
        /// 1:中修
        /// 2:小修
        /// </summary>
        public int? RepairLevel { get; set; }

        /// <summary>
        /// 维修方法
        /// </summary>
        public string RepairMethod { get; set; }

        /// <summary>
        /// 预防建议
        /// </summary>
        public string PreventionAdvice { get; set; }

        /// <summary>
        /// 故障描述ID
        /// </summary>
        public double? FaultDescriptionId { get; set; }

        /// <summary>
        /// 故障描述备注
        /// </summary>
        public string FaultDescriptionRemark { get; set; }

        /// <summary>
        /// 故障代码
        /// </summary>
        public string DeviceAbnormalCode { get; set; }

        /// <summary>
        /// TPM评分项
        /// </summary>
        public List<TpmScoreInfo> TpmScoreInfos { get; set; } = new List<TpmScoreInfo>();
    }
}
