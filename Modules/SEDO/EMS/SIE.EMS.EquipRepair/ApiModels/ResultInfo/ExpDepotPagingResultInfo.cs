using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 经验库实体查询结果分页实体
    /// </summary>
    [Serializable]
    public class ExpDepotPagingResultInfo : PagingResultInfo
    {
        /// <summary>
        /// 经验库实体查询结果实体信息列表
        /// </summary>
        public List<ExpDepotResultInfo> ExpDepotResultInfos { get; set; } = new List<ExpDepotResultInfo>();
    }

    /// <summary>
    /// 经验库实体查询结果实体
    /// </summary>
    [Serializable]
    public class ExpDepotResultInfo
    {
        /// <summary>
        /// 经验库Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 经验库编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备台账编码
        /// </summary>
        public string EquipAccountCode { get; set; }

        /// <summary>
        /// 设备台账名称
        /// </summary>
        public string EquipAccountName { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName { get; set; }

        /// <summary>
        /// 故障原因名称
        /// </summary>
        public string FaultReasonName { get; set; }

        /// <summary>
        /// 故障原因描述
        /// </summary>
        public string FaultReasonDesc { get; set; }

        /// <summary>
        /// 故障类别名称
        /// </summary>
        public string FaultCategoryName { get; set; }

        /// <summary>
        /// 故障现象描述
        /// </summary>
        public string DeviceAbnormalDesc { get; set; }

        /// <summary>
        /// 故障现象(备注)
        /// </summary>
        public string DeviceAbnormalRemark { get; set; }

        /// <summary>
        /// 故障描述(描述)
        /// </summary>
        public string FaultDescriptionDesc { get; set; }

        /// <summary>
        /// 故障描述(备注)
        /// </summary>
        public string FaultDescriptionRemark { get; set; }

        /// <summary>
        /// 故障部位
        /// </summary>
        public string FaultPart { get; set; }

        /// <summary>
        /// 维修方法
        /// </summary>
        public string RepairMethod { get; set; }

        /// <summary>
        /// 预防措施
        /// </summary>
        public string PreventionAdvice { get; set; }

        /// <summary>
        /// 故障代码
        /// </summary>
        public string FaultCode { get; set; }
    }
}
