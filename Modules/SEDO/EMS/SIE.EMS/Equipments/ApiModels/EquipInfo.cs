using SIE.Core.Enums;
using System;

namespace SIE.EMS.Equipments.ApiModels
{
    /// <summary>
    /// 设备信息
    /// </summary>
    [Serializable]
    public class EquipInfo
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 机台号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备类型ID
        /// </summary>
        public double? EquipTypeId { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipType { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShop { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 类型(0:设备,1:备件)
        /// </summary>
        public int EquipRepairType { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public AccountState State { get; set; }

        /// <summary>
        /// 设备Id（可空）
        /// </summary>
        public double? EquipId { get; set; }

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModelId { get; set; }

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode { get; set; }

        /// <summary>
        /// 使用部门Id
        /// </summary>
        public double? UseDepartmentId { get; set; }
    }

    /// <summary>
    /// 设备台账单元组成信息
    /// </summary>
    [Serializable]
    public class EquipAccountUnitInfo
    {
        /// <summary>
        /// 单元组成ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 单元组成编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 单元组成名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 设备台账信息变更
    /// </summary>
    [Serializable]
    public class EquipChangeInfo
    {
        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 旧使用部门Id
        /// </summary>
        public double? OldUseDepartmentId { get; set; }

        /// <summary>
        /// 旧使用部门名称
        /// </summary>
        public string OldUseDepartmentName { get; set; }

        /// <summary>
        /// 使用部门Id
        /// </summary>
        public double? UseDepartmentId { get; set; }

        /// <summary>
        /// 使用部门名称
        /// </summary>
        public string UseDepartmentName { get; set; }

        /// <summary>
        /// 旧使用责任人Id
        /// </summary>
        public double? OldUserId { get; set; }

        /// <summary>
        /// 旧使用责任人名称
        /// </summary>
        public string OldUserName { get; set; }

        /// <summary>
        /// 使用人Id
        /// </summary>
        public double? UserId { get; set; }

        /// <summary>
        /// 使用人名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 使用状态
        /// </summary>
        public AccountUseState UseState { get; set; }

        /// <summary>
        /// 旧使用状态
        /// </summary>
        public AccountUseState? OldUseState { get; set; }
    }
}