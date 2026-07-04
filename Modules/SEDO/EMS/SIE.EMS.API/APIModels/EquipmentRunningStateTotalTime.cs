using SIE.Equipments.Enums;
using System;
using System.Collections.Generic;

namespace SIE.EMS.API.APIModels
{
    /// <summary>
    /// 设备运行状态时间汇总
    /// </summary>
    [Serializable]
    public class EquipmentRunningStateTotalTime
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public double? Id { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备名称名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        public List<TotalTimeOfState> States { get; set; }
    }

    /// <summary>
    /// 运行状态与累计时长
    /// </summary>
    [Serializable]
    public class TotalTimeOfState
    {
        /// <summary>
        /// 运行状态
        /// </summary>
        public EquipRunningState? State { get; set; }

        /// <summary>
        /// 运行状态名称
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 累计时长（秒）
        /// </summary>
        public double TotalSeconds { get; set; }

        /// <summary>
        /// 累计时长（h）
        /// </summary>
        public string TotalHours { get; set; }

        /// <summary>
        /// 占比
        /// </summary>
        public string Percent { get; set; }

    }
}