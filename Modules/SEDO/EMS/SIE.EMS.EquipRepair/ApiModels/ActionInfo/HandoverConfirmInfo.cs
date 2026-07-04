using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 交机确认信息实体
    /// </summary>
    [Serializable]
    public class HandoverConfirmInfo
    {
        /// <summary>
        /// 维修单ID
        /// </summary>
        public double RepairBillId { get; set; }

        /// <summary>
        /// 交机确认结果(0:OK;1:NG)
        /// </summary>
        public int HandoverConfirmResult { get; set; }

        /// <summary>
        /// 异常情况(0:原故障未解决;1:新故障)
        /// </summary>
        public int HandoverConfirmAbnormal { get; set; }

        /// <summary>
        /// 交机故障现象ID
        /// </summary>
        public double? HandoverDeviceAbnormalId { get; set; }

        /// <summary>
        /// 交机故障现象(备注)
        /// </summary>
        public string HandoverDeviceAbnormalRem { get; set; }

        /// <summary>
        /// 照片上下文
        /// </summary>
        public string PhotoContent { get; set; }

        /// <summary>
        /// 文件名(含扩展名)
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// TPM评分项
        /// </summary>
        public List<TpmScoreInfo> TpmScoreInfos { get; set; } = new List<TpmScoreInfo>();
    }
}
