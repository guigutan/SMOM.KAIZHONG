using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock.Gantt.Areas.Dock.ViewModel
{
    /// <summary>
    /// 加载的数据
    /// </summary>
    [Serializable]
    public class LoadData
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 月台信息
        /// </summary>
        public Resources resources { get; set; } = new Resources();

        /// <summary>
        /// 预约信息
        /// </summary>
        public Events events { get; set; } = new Events();

        /// <summary>
        /// 装卸能力
        /// </summary>
        public Calendars calendars { get; set; } = new Calendars();
    }
    [Serializable]
    public class Resources
    {
        public List<DockData> rows { get; set; } = new List<DockData>();
    }

    [Serializable]
    public class DockData
    {
        public double id { get; set; }

        public string name { get; set; }

        public string groupName { get; set; }

        public string calendar { get; set; }

        public string state { get; set; }

        public string type { get; set; }
    }

    [Serializable]
    public class Events
    {
        public List<SaveAppointModel> rows { get; set; } = new List<SaveAppointModel>();
    }

    [Serializable]
    public class Calendars
    {
        public List<WorkTime> rows { get; set; } = new List<WorkTime>();
    }

    [Serializable]
    public class WorkTime
    {
        public string id { get; set; }

        public string name { get; set; }

        public bool unspecifiedTimeIsWorking { get; set; }

        public List<Intervals> intervals { get; set; } = new List<Intervals>();
    }

    [Serializable]
    public class Intervals
    {
        public string recurrentStartDate { get; set; }
        public string recurrentEndDate { get; set; }
        public bool isWorking { get; set; } = true;

    }
}
