using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 经验库实体
    /// </summary>
    [Serializable]
    public class ExperienceDepotInfo
    {
        /// <summary>
        /// 维修单号
        /// </summary>
        public string RepairBillNo { get; set; }

        /// <summary>
        /// 设备台账ID
        /// </summary>
        public double EquipAccountId { get; set; }

        /// <summary>
        /// 设备型号ID
        /// </summary>
        public double? EquipModelId { get; set; }

        /// <summary>
        /// 设备类型ID
        /// </summary>
        public double? EquipTypeId { get; set; }

        /// <summary>
        /// 故障原因编码
        /// </summary>
        public string FaultReasonCode { get; set; }

        /// <summary>
        /// 故障类别ID
        /// </summary>
        public double FaultCategoryId { get; set; }

        /// <summary>
        /// 故障部位
        /// </summary>
        public string FaultPart { get; set; }

        /// <summary>
        /// 维修方法
        /// </summary>
        public string RepairMethod { get; set; }

        /// <summary>
        /// 预防建议
        /// </summary>
        public string PreventionAdvice { get; set; }

        /// <summary>
        /// 故障代码
        /// </summary>
        public string FaultCode { get; set; }

        /// <summary>
        /// 故障现象ID
        /// </summary>
        public double? DeviceAbnormalId { get; set; }

        /// <summary>
        /// 故障现象(备注)
        /// </summary>
        public string DeviceAbnormalRemark { get; set; }

        /// <summary>
        /// 故障描述ID
        /// </summary>
        public double? FaultDescriptionId { get; set; }

        /// <summary>
        /// 故障描述(备注)
        /// </summary>
        public string FaultDescriptionRemark { get; set; }


        /// <summary>
        /// 图片附件信息
        /// </summary>
        public List<RepairAttachmentInfo> PhotoInfos { get; set; } = new List<RepairAttachmentInfo>();
    }
}
