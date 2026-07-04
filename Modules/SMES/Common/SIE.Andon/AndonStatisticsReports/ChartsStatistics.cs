using System;

namespace SIE.Andon.AndonStatisticsReports
{
    /// <summary>
    /// 报表数据
    /// </summary>
    [Serializable]
    public class ChartsStatistics
    {
        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 安灯次数
        /// </summary>
        public double AndonNum
        {
            get;
            set;
        }
        /// <summary>
        /// 安灯时长
        /// </summary>
        public double AndonTime
        {
            get;
            set;
        }

        /// <summary>
        /// 停线次数
        /// </summary>
        public double AndonStopNum
        {
            get;
            set;
        }

        /// <summary>
        /// 停线时长
        /// </summary>
        public double AndonStopLine
        {
            get;
            set;
        }

        /// <summary>
        /// 安灯名称变更率
        /// </summary>
        public double TriggerAccuracy
        {
            get;
            set;
        }

    }
}
