using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MES.Routings.RoutingSettings.ApiModels;
using SIE.MES.RoutingSettings;
using SIE.MES.WIP.Runtime;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Routings.RoutingSettings
{
    public class ProductRoutingController : DomainController
    {
        /// <summary>
        /// 获取产品工艺路线设置列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<ProductRouting> GetProductRoutings(ProductRoutingCriteria criteria) {
            var q = Query<ProductRouting>();
            if (criteria.OrderType.HasValue)
            {
                q.Where(p => p.OrderType == criteria.OrderType.Value);
            }
            if (criteria.ProductId.HasValue) {
                q.Where(p => p.ProductId == criteria.ProductId);
            }
            if (criteria.RoutingId.HasValue)
            {
                q.Where(p => p.RoutingId == criteria.RoutingId);
            }
            if (criteria.StartDate.BeginValue.HasValue) {
                q.Where(p => p.StartDate >= criteria.StartDate.BeginValue);
            }
            if (criteria.StartDate.EndValue.HasValue)
            {
                q.Where(p => p.StartDate <= criteria.StartDate.EndValue);
            }
            if (criteria.EndDate.BeginValue.HasValue)
            {
                q.Where(p => p.EndDate >= criteria.EndDate.BeginValue);
            }
            if (criteria.EndDate.EndValue.HasValue)
            {
                q.Where(p => p.EndDate <= criteria.EndDate.EndValue);
            }
            return q.ToList(criteria.PagingInfo,new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取当前时间产品的产品工艺路线设置
        /// </summary>
        /// <param name="productIds">产品Ids</param>
        /// <returns></returns>
        public virtual IEnumerable<ProRoutingInfo> GetNowProductRoutingsByProductIds(IEnumerable<double> productIds)
        {
            List<ProRoutingInfo> proRoutingInfos = new List<ProRoutingInfo>();
            var now = DateTime.Now.Date;
            productIds.SplitDataExecute(tempIds =>
            {
                var q = Query<ProductRouting>().LeftJoin<RoutingVersion>((pr, rv) => pr.RoutingVersionId == rv.Id)
                .Where(pr => pr.ProductId != null && tempIds.Contains((double)pr.ProductId) && pr.StartDate <= now && pr.EndDate >= now)
                .Select<RoutingVersion>((pr, rv) => new
                {
                    OrderType = pr.OrderType,
                    ProductId = pr.ProductId,
                    RoutingId = pr.RoutingId,
                    RoutingVersionId = pr.RoutingVersionId,
                    VersionName = rv.Name,
                    StartDate = pr.StartDate,
                    EndDate = pr.EndDate,
                }).ToList<ProRoutingInfo>();
                proRoutingInfos.AddRange(q);
            });
            return proRoutingInfos.OrderByDescending(p => p.VersionName);
        }

        /// <summary>
        /// 根据版本号获取工艺路线工序清单
        /// </summary>
        /// <param name="versionIds"></param>
        /// <returns></returns>
        public virtual EntityList<RoutingProcess> GetRoutingProcessesByVersionIds(List<double> versionIds)
        {
            return versionIds.SplitContains(tempIds =>
            {
                return Query<RoutingProcess>().Where(p => p.VersionId != null && tempIds.Contains((double)p.VersionId)).ToList();
            });
        }

        /// <summary>
        /// 判断同类型、产品、工艺路线、项目号、开始时间、结束时间的产品工艺路线设置
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="productId">产品</param>
        /// <param name="routingId">工艺路线</param>
        /// <param name="projectId">项目号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public virtual ProductRouting GetSameProductRouting(WorkOrderType type, double productId, double routingId, double projectId, DateTime startTime, DateTime? endTime)
        {
            var q = Query<ProductRouting>().Where(p => p.OrderType == type && p.ProductId == productId && p.RoutingId == routingId && p.ProjectMaintainId == projectId
             && p.StartDate == startTime && p.EndDate == endTime).ToList();
            return q.FirstOrDefault();
        }
    }
}
