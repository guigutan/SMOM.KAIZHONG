using SIE.Domain;
using System;
using System.Linq;

namespace SIE.Core.ApiLogs
{
    /// <summary>
    /// API日志控制器
    /// </summary>
    public partial class ApiLogController : DomainController
    {
        /// <summary>
        /// 获取API日志数据
        /// </summary>       
        public virtual EntityList<ApiLog> GetApiLogs(ApiLogCriteria criteria)
        {
            var query = Query<ApiLog>();
            if (criteria.ApiName.IsNotEmpty())
            {
                criteria.ApiName = "%" + criteria.ApiName + "%";
                query.Where(f => f.ApiName.Contains(criteria.ApiName));
            }
            if (criteria.Controller.IsNotEmpty())
                query.Where(f => f.Controller.Contains(criteria.Controller));
            if (criteria.Method.IsNotEmpty())
                query.Where(f => f.Method.Contains(criteria.Method));
            if (criteria.KeyValue.IsNotEmpty())
            {
                criteria.KeyValue = "%" + criteria.KeyValue + "%";
                query.Where(f => f.Key1.Contains(criteria.KeyValue) ||
                f.Key2.Contains(criteria.KeyValue) ||
                   f.Key3.Contains(criteria.KeyValue) ||
                      f.Key4.Contains(criteria.KeyValue) ||
                        f.Key5.Contains(criteria.KeyValue)
                );
            }
            if (criteria.CreateByName.IsNotEmpty())
                query.Where(f => f.Employee.Name.Contains(criteria.CreateByName));
            if (criteria.StartTime.BeginValue.HasValue)
                query.Where(f => f.StartTime >= criteria.StartTime.BeginValue);
            if (criteria.StartTime.EndValue.HasValue)
                query.Where(f => f.StartTime <= criteria.StartTime.EndValue);
            if (criteria.EndTime.BeginValue.HasValue)
                query.Where(f => f.EndTime >= criteria.EndTime.BeginValue);
            if (criteria.EndTime.EndValue.HasValue)
                query.Where(f => f.EndTime <= criteria.EndTime.EndValue);

            if (criteria.OrderInfoList.Count == 0)
                criteria.OrderInfoList.Add(new OrderInfo() { Property = "StartTime", SortIndex = 0, SortOrder = System.ComponentModel.ListSortDirection.Descending });
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
