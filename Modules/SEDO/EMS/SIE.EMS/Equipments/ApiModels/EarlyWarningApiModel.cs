using SIE.EMS.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Equipments.ApiModels
{
    /// <summary>
    /// 报警汇总查询信息
    /// </summary>
    [Serializable]
    public class AlarmSummaryQueryInfo
    {
        /// <summary>
        /// 设备编码\名称
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 设备类型id
        /// </summary>
        public double? EquipTypeId { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public double? DepartmentId { get; set; }

        /// <summary>
        /// 查询时间
        /// </summary>
        public AlarmQueryTime QueryTime { get; set; }

        /// <summary>
        /// 严重性
        /// </summary>
        public AlarmLevel? AlarmLevel { get; set; }

        /// <summary>
        /// 报警状态
        /// </summary>
        public AlarmState? AlarmState { get; set; }
    }

    /// <summary>
    /// 报警查询时间
    /// </summary>
    public enum AlarmQueryTime
    {
        /// <summary>
        /// 今日
        /// </summary>
        [Label("今日")]
        Today = 1,

        /// <summary>
        /// 本周
        /// </summary>
        [Label("本周")]
        ThisWeek = 2,

        /// <summary>
        /// 本月
        /// </summary>
        [Label("本月")]
        ThisMonth = 3,

        /// <summary>
        /// 近7天
        /// </summary>
        [Label("近7天")]
        NearlySevenDays = 4,

        /// <summary>
        /// 近30天
        /// </summary>
        [Label("近30天")]
        NearlyThirtyDays = 5,

        /// <summary>
        /// 3个月
        /// </summary>
        [Label("3个月")]
        ThreeMonths = 6,

        /// <summary>
        /// 6个月
        /// </summary>
        [Label("6个月")]
        SixMonths = 7,

        /// <summary>
        /// 本年
        /// </summary>
        [Label("本年")]
        ThisYear = 8,
    }

    /// <summary>
    /// 报警汇总信息
    /// </summary> 
    [Serializable]
    public class AlarmSummaryInfo
    {
        /// <summary>
        /// 统计设备台数
        /// </summary>
        public int EquipCount { get; set; }

        /// <summary>
        /// 开启状态数量
        /// </summary>
        public int OpenQty { get; set; }

        /// <summary>
        /// 关闭状态数量
        /// </summary>
        public int CloseQty { get; set; }

        /// <summary>
        /// 提示-数量
        /// </summary>
        public int InfoQty { get; set; }

        /// <summary>
        /// 轻微-数量
        /// </summary>
        public int MinorQty { get; set; }

        /// <summary>
        /// 一般-数量
        /// </summary>
        public int MediumQty { get; set; }

        /// <summary>
        /// 严重-数量
        /// </summary>
        public int MajorQty { get; set; }

        /// <summary>
        /// 紧急-数量
        /// </summary>
        public int SeriousQty { get; set; }

        /// <summary>
        /// 报警类别-数量列表
        /// </summary>
        public List<WarningChartInfo> AlarmCategorys { get; set; } = new List<WarningChartInfo>();

        /// <summary>
        /// 设备-报警数列表
        /// </summary>
        public List<WarningChartInfo> EquipAlarms { get; set; } = new List<WarningChartInfo>();

        /// <summary>
        /// 报警明细
        /// </summary>
        public List<AlarmDetailInfo> AlarmDetails { get; set; } = new List<AlarmDetailInfo>();
    }

    /// <summary>
    /// 报警明细
    /// </summary>
    [Serializable]
    public class AlarmDetailInfo
    {
        /// <summary>
        /// 设备报警记录ID
        /// </summary>
        public double EquipAlarmRecordId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public double EquipAccountId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipModelName { get; set; }

        /// <summary>
        /// 报警编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 报警类型
        /// </summary>
        public string AlarmType { get; set; }

        /// <summary>
        /// 报警内容
        /// </summary>
        public string AlarmContent { get; set; }

        /// <summary>
        /// 报警时间
        /// </summary>
        public DateTime AlarmTime { get; set; }

        /// <summary>
        /// 报警级别
        /// </summary>
        public string AlarmLevel { get; set; }

        /// <summary>
        /// 报警级别(0-严重,1-紧急,2-一般，3-轻微，4-提示)
        /// </summary>
        public int AlarmLevelValue { get; set; }

        /// <summary>
        /// 报警状态
        /// </summary>
        public string AlarmState { get; set; }

        /// <summary>
        /// 报警状态值
        /// </summary>
        public AlarmState AlarmStateValue { get; set; }

        /// <summary>
        /// 关闭时间
        /// </summary>
        public DateTime? CloseTime { get; set; }

        /// <summary>
        /// 报警持续时间
        /// </summary>
        public string Duration { get; set; }
    }

    /// <summary>
    /// 预警图表信息
    /// </summary>
    [Serializable]
    public class WarningChartInfo
    {
        /// <summary>
        /// 显示标签
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }
    }
}
