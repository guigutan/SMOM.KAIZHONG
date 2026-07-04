using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.Controller
{
    /// <summary>
    /// 接口预警 控制器
    /// </summary>
    public class InterfaceAlertController : DomainController
    {
        /// <summary>
        /// 查询调度失败下载记录
        /// </summary>
        /// <returns></returns>
        public virtual IList<DownloadFailStatistics> GetDownloadFailStatistics(DateTime? beginDate, DateTime? endDate)
        {
            var q = Query<DownloadJobLog>();
            q.Where(p => p.State == Enums.ProcessState.Failed);
            q.Where(p => p.OperationType == Enums.OperationType.Scheduling);

            if (beginDate.HasValue)
                q.Where(p => p.BeginDate >= beginDate);
            if (endDate.HasValue)
                q.Where(p => p.EndDate <= endDate);

            q.GroupBy(p => new { p.JobType, p.JobDirection }).Select(p => new { p.JobType, p.JobDirection, FailCount = p.FailCount.SUM() });

            return q.ToList<DownloadFailStatistics>();
        }

        /// <summary>
        /// 查询调度失败下载记录
        /// </summary>
        /// <returns></returns>
        public virtual IList<UploadFailStatistics> GetUploadFailStatistics(DateTime? beginDate, DateTime? endDate)
        {
            var q = Query<UploadTransaction>();
            q.Where(p => p.State == Enums.ProcessState.Failed);

            if (beginDate.HasValue)
                q.Where(p => p.TransactionDate >= beginDate);
            if (endDate.HasValue)
                q.Where(p => p.TransactionDate <= endDate);

            q.GroupBy(p => new { p.OrderType, p.TransactionType }).Select(p => new { p.OrderType, p.TransactionType, FailCount = p.OrderType.COUNT() });

            return q.ToList<UploadFailStatistics>();
        }
    }
}
