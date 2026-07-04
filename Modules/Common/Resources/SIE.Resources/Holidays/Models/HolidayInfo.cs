using System;

namespace SIE.Resources.Holidays
{
    /// <summary>
    /// 法定假期信息
    /// </summary>
    [Serializable]
    public class HolidayInfo
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 假期名称
        /// </summary>
        public string Remark { get; set; }
    }
}