using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 报修参数实体
    /// </summary>
    [Serializable]
    public class ApplyRepairInfo
    {
        /// <summary>
        /// 维修类型(0:设备维修，1:备件维修)
        /// </summary>
        public int RepairType { get; set; }

        /// <summary>
        /// 紧急程序(0:紧急，1:高，2:一般)
        /// </summary>
        public int UrgentDegree { get; set; }

        /// <summary>
        /// 生产状态(0:停机，1:生产)
        /// </summary>
        public int ProduceState { get; set; }

        /// <summary>
        /// 故障现象ID
        /// </summary>
        public double? DeviceAbnormalId { get; set; }

        /// <summary>
        /// 故障现象(备注)
        /// </summary>
        public string DeviceAbnormalRemark { get; set; }

        /// <summary>
        /// 故障代码
        /// </summary>
        public string DeviceAbnormalCode { get; set; }

        /// <summary>
        /// 设备台账ID
        /// </summary>
        public double? EquipAccountId { get; set; }

        /// <summary>
        /// 备件ID
        /// </summary>
        public double? SparePartId { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        /// 来源类型(0:点检,1:保养,2:手工创建)
        /// </summary>
        public int RepairSourceType { get; set; }

        /// <summary>
        /// 图片附件信息
        /// </summary>
        public List<RepairAttachmentInfo> PhotoInfos { get; set; } = new List<RepairAttachmentInfo>();
    }
}
