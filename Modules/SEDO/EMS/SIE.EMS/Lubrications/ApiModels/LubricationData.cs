using System;
using System.Collections.Generic;

namespace SIE.EMS.Lubrications.ApiModels
{
    /// <summary>
    /// 润滑记录数据
    /// </summary>
    [Serializable]
    public class LubricationData
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 润滑记录列表数据
        /// </summary>
        public List<EquLubricationInfo> LubricationInfos { get; set; } = new List<EquLubricationInfo>();
    }

    /// <summary>
    /// 润滑记录信息
    /// </summary>
    [Serializable]
    public class EquLubricationInfo
    {
        /// <summary>
        /// 润滑记录id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 润滑记录单号
        /// </summary>
        public string No { get; set; }

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
        /// 设备类型ID
        /// </summary>
        public double EquipTypeId { get; set; }

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
        /// 位置
        /// </summary>
        public string InstallationLocation { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 润滑小结
        /// </summary>
        public string Implementation { get; set; }
    }
}
