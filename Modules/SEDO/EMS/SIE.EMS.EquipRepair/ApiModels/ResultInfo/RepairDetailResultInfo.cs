using SIE.EMS.EquipRepair.ApiModels.ResultInfo;
using System;
using System.Collections.Generic;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 设备维修单信息查询结果，单据及子单详细 信息实体
    /// </summary>
    [Serializable]
    public class RepairDetailResultInfo : RepairResultInfo
    {
        /// <summary>
        /// 派工类型(0:内修，1:外修)
        /// </summary>
        public int? RepairWay { get; set; }

        /// <summary>
        /// 委外维修报告
        /// </summary>
        public string OutsourcedMaintenanceReport { get; set; }

        /// <summary>
        /// 故障原因编码
        /// </summary>
        public string FaultReasonCode { get; set; }

        /// <summary>
        /// 故障原因名称
        /// </summary>
        public string FaultReasonName { get; set; }

        /// <summary>
        /// 故障原因描述
        /// </summary>
        public string FaultReasonDesc { get; set; }

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
        /// 故障类别编码
        /// </summary>
        public string FaultCategoryCode { get; set; }

        /// <summary>
        /// 故障类别名称
        /// </summary>
        public string FaultCategoryName { get; set; }

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
        /// 故障描述(描述)
        /// </summary>
        public string FaultDescriptionDesc { get; set; }

        /// <summary>
        /// 故障描述(描述)
        /// </summary>
        public string FaultDescriptionCode { get; set; }

        /// <summary>
        /// 故障描述备注
        /// </summary>
        public string FaultDescriptionRemark { get; set; }

        /// <summary>
        /// 维修响应时间
        /// </summary>
        public decimal? RepairResponseTime { get; set; }

        /// <summary>
        /// 维修执行时间
        /// </summary>
        public decimal? RepairExecuteTime { get; set; }

        /// <summary>
        /// 维修执行时间
        /// </summary>
        public decimal? RepairTotalWorkingHour { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string  ProjectCode { get; set; }

        /// <summary>
        /// 项目事项
        /// </summary>
        public string ProjectItemCode{ get; set; }
        #region 子列表

        /// <summary>
        /// 设备维修规程信息
        /// </summary>
        public List<RepairBillProjectResultInfo> BillProjectInfos { get; set; } = new List<RepairBillProjectResultInfo>();

        /// <summary>
        /// 设备维修工时信息
        /// </summary>
        public List<RepairWorkingHoursResultInfo> WorkingHoursInfos { get; set; } = new List<RepairWorkingHoursResultInfo>();

        /// <summary>
        /// 设备维修操作记录信息
        /// </summary>
        public List<RepairOperationRecResultInfo> OperationRecInfos { get; set; } = new List<RepairOperationRecResultInfo>();

        /// <summary>
        /// 设备维修附件信息
        /// </summary>
        public List<RepairAttachmentInfo> AttachmentInfos { get; set; } = new List<RepairAttachmentInfo>();

        /// <summary>
        /// 设备报修附件信息
        /// </summary>
        public List<RepairAttachmentInfo> ApplyRepairAttachmentInfos { get; set; } = new List<RepairAttachmentInfo>();

        /// <summary>
        /// 设备维修备件申请信息
        /// </summary>
        public List<RepairSparePartAplResultInfo> SparePartAplInfos { get; set; } = new List<RepairSparePartAplResultInfo>();

        /// <summary>
        /// 设备维修备件更换信息
        /// </summary>
        public List<RepairSparePartChgResultInfo> SparePartChgInfos { get; set; } = new List<RepairSparePartChgResultInfo>();


        #endregion
    }
}
