using SIE.EMS.Maintains.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Maintains.ApiModels
{
    /// <summary>
    /// 保养计划列表
    /// </summary>
    [Serializable]
    public class MaintainPlanInfos : MaintainPlanInfo
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName { get; set; }

        /// <summary>
        /// 设备台账ID
        /// </summary>
        public double EquipId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public double? DepartmentId { get; set; }

        /// <summary>
        /// 部门编码
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string Shop { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// 保养时长
        /// </summary>
        public decimal? MaintainTime { get; set; }

        /// <summary>
        /// 设备类型ID
        /// </summary>
        public double EquipTypeId { get; set; }

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public string EquipTypeCode { get; set; }

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName { get; set; }

        /// <summary>
        /// 设备型号ID
        /// </summary>
        public double EquipModelId { get; set; }

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode { get; set; }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipModelName { get; set; }

        /// <summary>
        /// 保养小结
        /// </summary>
        public string MaintainSummary { get; set; }

        /// <summary>
        /// 是否已报修
        /// </summary>
        public int WhetherRepair { get; set; }

        /// <summary>
        /// 保养类型
        /// </summary>
        public string MaintainTypeDisplay { get; set; }

        /// <summary>
        /// 保养类型值
        /// </summary>
        public int MaintainType { get; set; }

        /// <summary>
        /// 保养人
        /// </summary>
        public string MaintainEmployee { get; set; }

        /// <summary>
        /// 是否维修开始
        /// </summary>
        public bool WhetherBegin { get; set; }

        /// <summary>
        /// 保养确认人
        /// </summary>
        public bool MaintainConfirm { get; set; }
    }
}
