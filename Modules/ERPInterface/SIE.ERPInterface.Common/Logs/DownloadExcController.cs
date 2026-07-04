using SIE.Domain;
using System;
using System.Linq;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 接口下载异常管理控制器
    /// </summary>
    public partial class DownloadExcController : DomainController
    {
        /// <summary>
        /// 查询任务下载记录
        /// </summary>
        /// <param name="criteria">接口下载异常查询实体</param>
        /// <returns>任务下载记录</returns>
        public virtual EntityList<DownloadJobLog> GetDownloadJobLogs(DownloadExcViewModelCriteria criteria)
        {
            var query = Query<DownloadJobLog>();
            if (criteria.JobType != null)
                query.Where(p => p.JobType == criteria.JobType);
            if (criteria.LogDate.BeginValue != null)
                query.Where(p => p.BeginDate >= criteria.LogDate.BeginValue);
            if (criteria.LogDate.EndValue != null)
                query.Where(p => p.EndDate <= criteria.LogDate.EndValue);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询接口下载异常记录
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>实体列表</returns>
        public virtual DownloadExcReportViewModel GetDownloadExcs(DownloadExcViewModelCriteria criteria)
        {
            var viewModel = new DownloadExcReportViewModel();
            viewModel.LayoutFileName = viewModel.GetType().Name;

            var list = GetDownloadJobLogs(criteria);

            var groupList = list.GroupBy(p => new { JoinType = p.JobType })
                .Select(p => new DownloadExcViewModel
                {
                    JobType = p.Key.JoinType.ToLabel(),
                    FailCount = p.Sum(t => t.FailCount),
                    SuccessCount = p.Sum(t => t.SuccessCount),
                    DataCount = p.Sum(t => t.DataCount)
                }).ToList();

            groupList.ForEach(group =>
            {
                var untreatedlist = list.Where(p => p.State == Enums.ProcessState.Unprocessed).ToList();
                group.UntreatedCount = untreatedlist.Sum(p => p.DataCount);
            });

            viewModel.DownloadExcList.AddRange(groupList);
            viewModel.Criteria = criteria;
            viewModel.MarkSaved();
            return viewModel;
        }

        /// <summary>
        /// 查询任务下载记录
        /// </summary>
        /// <param name="criteria">接口下载异常查询实体</param>
        /// <returns>任务下载记录</returns>
        public virtual EntityList<DownloadJobTime> GetDownloadJobTimes(DownloadJobTimeCriteria criteria)
        {
            var query = Query<DownloadJobTime>();
            if (criteria.JobType.HasValue)
                query.Where(p => p.JobType == criteria.JobType);
            if (criteria.RequestDate.BeginValue.HasValue || criteria.RequestDate.EndValue.HasValue)
            {
                var dtlList = Query<DownloadJobTimeDetail>().Select(x => x.DownloadJobTimeId).WhereIf(criteria.RequestDate.BeginValue.HasValue, x => x.RequestDate >= criteria.RequestDate.BeginValue).WhereIf(criteria.RequestDate.EndValue.HasValue, x => x.RequestDate <= criteria.RequestDate.EndValue).Distinct().ToList();

                query.Where(p => dtlList.Contains(p.Id));
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取接口任务时间戳明细
        /// </summary>
        /// <param name="id">接口任务时间戳ID</param>
        /// <param name="requestStr">接口任务时间戳明细请求报文</param>
        /// <returns></returns>
        public virtual EntityList<DownloadJobTimeDetail> GetDownloadJobTimeDetails(double id, string requestStr)
        {
            var query = Query<DownloadJobTimeDetail>().Where(p => p.DownloadJobTimeId == id);
            if (!string.IsNullOrEmpty(requestStr))
                query.Where(p => p.RequestStr.Contains("%" + requestStr + "%"));
            return query.ToList();
        }
    }
}
