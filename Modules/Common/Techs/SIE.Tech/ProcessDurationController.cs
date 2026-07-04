using SIE.Domain;
using SIE.Items;
using SIE.Tech.Processs;
using System;

namespace SIE.Tech
{
    /// <summary>
    /// 工序加工时长控制器
    /// </summary>
    public class ProcessDurationController : DomainController
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="processDurationCriteria"></param>
        /// <returns></returns>
        public virtual EntityList QueryDatas(ProcessDurationCriteria processDurationCriteria)
        {
            var query = Query<ProcessDuration>();
            if (!processDurationCriteria.ProcessName.IsNullOrEmpty())
            {
                query.Join<Process>((x, y) => x.ProcessId == y.Id)
                    .Where<Process>((x, y) => y.Name.Contains(processDurationCriteria.ProcessName));
            }

            if (!processDurationCriteria.ProductName.IsNullOrEmpty())
            {
                query.Join<Item>((x, y) => x.ProductId == y.Id)
                    .Where<Item>((x, y) => y.Name.Contains(processDurationCriteria.ProductName));
            }

            if (processDurationCriteria.ProductId.HasValue)
            {
                query.Where(x => x.ProductId == processDurationCriteria.ProductId.Value);
            }

            if (processDurationCriteria.ProcessId.HasValue)
            {
                query.Where(x => x.ProcessId == processDurationCriteria.ProcessId.Value);
            }

            return query.ToList(processDurationCriteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取工序加工时长
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        public virtual ProcessDuration GetProcessDurations(double productId, double processId)
        {
            var query = Query<ProcessDuration>()
                .Where(x => x.ProductId == productId && processId == x.ProcessId);

            return query.FirstOrDefault();
        }
    }
}
