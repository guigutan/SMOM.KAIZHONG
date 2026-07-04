using System;

namespace SIE.EMS.Checks.ApiModels
{
    /// <summary>
    /// 点检计划列表
    /// </summary>
    [Serializable]
    public class CheckPlanInfos : CheckPlanInfo
    {
        /// <summary>
        /// 点检计划Id
        /// </summary>
        public double CheckPlanId { get; set; }

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
        /// 点检时间
        /// </summary>
        public int? CheckTime { get; set; }

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
        /// 点检小结
        /// </summary>
        public string CheckSummary { get; set; }

        /// <summary>
        /// 是否已报修
        /// </summary>
        public int WhetherRepair { get; set; }

        /// <summary>
        /// 点检执行时间
        /// </summary>
        public string CheckDate { get; set; }

        /// <summary>
        /// 点检人
        /// </summary>
        public string CheckEmployeeName { get; set; }

        /// <summary>
        /// 点检确认人
        /// </summary>
        public bool CheckConfirm { get; set; }

        /// <summary>
        /// 设备使用部门
        /// </summary>
        public double EquipUseDeptId { get; set; }

        /// <summary>
        /// 设备使用部门名称
        /// </summary>
        public string EquipUseDeptName { get; set; }

        /// <summary>
        /// 点检计划类型
        /// </summary>
        public int CheckPlanType { get; set; }

        /// <summary>
        /// RFID
        /// </summary>
        public string RFID { get; set; }
    }
}
