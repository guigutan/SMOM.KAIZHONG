using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.Datas
{
    /// <summary>
    /// 默认模板数据
    /// </summary>
    [Serializable]
    public class DockAppointData
    {
        /// <summary>
        /// 单据ID集合
        /// </summary>
        public double[] BillIds { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        public double PrintTemplateId { get; set; }

        /// <summary>
        /// 原因描述
        /// </summary>
        public string ReasonDesc { get; set; }
    }

    /// <summary>
    /// 月台预约时间数据
    /// </summary>
    public class DockAppointTimeData
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public List<DateTime> BeginTimes { get; set; } = new List<DateTime>();

        /// <summary>
        /// 结束时间
        /// </summary>
        public List<DateTime> EndTimes { get; set; } = new List<DateTime>();
    }

    /// <summary>
    /// 月台处理能力数据
    /// </summary>
    public class DockHandleData
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 送货可预约数
        /// </summary>
        public int ShipAppoNum { get; set; }

        /// <summary>
        /// 提货可预约数
        /// </summary>
        public int ReceiveAppoNum { get; set; }

        /// <summary>
        /// 可用时间
        /// </summary>
        public double UseTime { get; set; }

        /// <summary>
        /// 最大剩余可用时间
        /// </summary>
        public double MaxRestTime { get; set; }

        /// <summary>
        /// 剩余可用时间
        /// </summary>
        public double RemainUseTime { get; set; }

        /// <summary>
        /// 已预约数
        /// </summary>
        public int HasAppointCount { get; set; }

        /// <summary>
        /// 预计占用时间
        /// </summary>
        public double UseHours { get; set; }

        /// <summary>
        /// 月台ID
        /// </summary>
        public double DockId { get; set; }
    }

    /// <summary>
    /// 最大可用时间数据
    /// </summary>
    public class DockMaxRestTimeData
    {
        /// <summary>
        /// 最大可用时间
        /// </summary>
        public double MaxRestTime { get; set; }

        /// <summary>
        /// 月台ID
        /// </summary>
        public double DockId { get; set; }
    }
}