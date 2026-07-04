using SIE.Resources.ShiftTypes;
using System;

namespace SIE.MES.TeamManagement.ShiftSchedules.Models
{
    /// <summary>
    /// 排班信息
    /// </summary>
    [Serializable]
    public class ScheduleStore
    {
        /// <summary>
        /// 排班ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 班组名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 是否整天
        /// </summary>
        public bool AllDay { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 日历ID
        /// </summary>
        public object CalendarId { get; set; }

        /// <summary>
        /// 班次ID
        /// </summary>
        public double ShiftId { get; set; }

        /// <summary>
        /// 班组ID
        /// </summary>
        public double WorkGroupId { get; set; }

        /// <summary>
        /// 班组名称，切换班组显示使用
        /// </summary>
        public string WorkGroupName { get; set; }

        /// <summary>
        /// 是否空闲，未排班则为true
        /// </summary>
        public bool Leisure { get; set; }

        /// <summary>
        /// 是否新增
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double WipResourceId { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 班制ID
        /// </summary>
        public double ShiftTypeId { get; set; }

        /// <summary>
        /// 班次数量
        /// </summary>
        public int ShiftCount { get; set; }
    }

    /// <summary>
    /// 排班班次信息
    /// </summary>
    [Serializable]
    public class SchduleShift
    {
        /// <summary>
        /// 是否过期，为true则在排班班次中过滤掉
        /// </summary>
        public bool IsExpire { get; set; }

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift { get; set; }

        /// <summary>
        /// 班次ID
        /// </summary>
        public double ShiftId { get; set; }
    }
}