using SIE.AbnormalInfo.AbnormalInfos;
using System;

namespace SIE.AbnormalInfo.Reports
{
    /// <summary>
    /// 异常信息报表数据
    /// </summary>
    [Serializable]
    public class AbnormalReportCloseDto
    {
        /// <summary>
        /// 异常发生数
        /// </summary>
        public int TotalQty { get; set; }

        /// <summary>
        /// 异常关闭数
        /// </summary>
        public int CloseQty { get; set; }

        /// <summary>
        /// 异常关闭率
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// 异常发生日期
        /// </summary>
        public DateTime Date { get; set; }
    }

    /// <summary>
    /// 异常信息状态及发生日期
    /// </summary>
    [Serializable]
    public class AbnormalReportStateTime
    {
        /// <summary>
        /// 异常状态
        /// </summary>
        public AbnormalStatus AbnormalStatus { get; set; }

        /// <summary>
        /// 异常发生时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
