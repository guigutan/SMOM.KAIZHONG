using Microsoft.CodeAnalysis;
using SIE.Common;
using SIE.Core.Common.Controllers;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.EventMessages.Inspection;
using SIE.Items;
using SIE.MES.Routings.RoutingSettings;
using SIE.MES.Routings.RoutingSettings.ApiModels;
using SIE.Resources.Enterprises;
using SIE.Resources.ProcessSegments;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.RoutingSettings
{
    /// <summary>
    /// 工艺路线设置控制器
    /// </summary>
    public class RoutingSettingController : DomainController
    {
        /// <summary>
        /// 获取同一产品，工单类型，工艺路线，制程工艺、有效期的产品工艺路线数量
        /// </summary>
        /// <param name="routing">产品工艺路线</param>
        /// <exception cref="ArgumentNullException">参数不能为null</exception>
        /// <returns>true存在工艺路线，否则返回false</returns>
        public virtual int CountProductRouting(ProductRouting routing)
        {
            Check.NotNull(routing, nameof(routing));
            return Query<ProductRouting>().Where(p => p.Id != routing.Id && p.ProductId == routing.ProductId && p.OrderType == routing.OrderType && p.ProjectMaintainId == routing.ProjectMaintainId && p.ProcessSegmentId == routing.ProcessSegmentId && p.StartDate <= routing.EndDate && p.EndDate >= routing.StartDate).Count();
        }

        /// <summary>
        /// 获取同一产线，工单类型，工艺路线，有效期的产线工艺路线数量
        /// </summary>
        /// <param name="routing">产线工艺路线</param>
        /// <exception cref="ArgumentNullException">参数不能为null</exception>
        /// <returns>true存在工艺路线，否则返回false</returns>
        public virtual int CountResourceRouting(ResourceRouting routing)
        {
            Check.NotNull(routing, nameof(routing));
            return Query<ResourceRouting>().Where(p => p.Id != routing.Id && p.ResourceId == routing.ResourceId && p.OrderType == routing.OrderType && p.StartDate <= routing.EndDate && p.EndDate >= routing.StartDate).Count();
        }

        #region GetRoutingVersions 获取工艺路线版本
        /// <summary>
        /// 获取工艺路线版本（自动匹配优先取产品工艺路线）
        /// </summary>
        /// <param name="type">工单类型</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="productId">产品Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <param name="processSegmentId">工段Id</param>
        /// <param name="projectId">项目Id</param>
        /// <returns>工艺路线版本列表</returns>
        public virtual EntityList<RoutingVersion> AutoRoutingVersions(WorkOrderType type, DateTime startDate, double? productId = 0, double? resourceId = 0, double? processSegmentId = null, double? projectId = null)
        {
            var versionList = new EntityList<RoutingVersion>();
            ////优先取产品工艺路线
            if (productId.HasValue && productId > 0)
            {
                versionList.AddRange(GetProductRoutingVersion(type, startDate, productId.Value, processSegmentId, projectId));
            }

            if (!versionList.Any() && resourceId.HasValue && resourceId > 0)
            {
                versionList.AddRange(GetResourceRoutingVersions(type, startDate, resourceId.Value));
            }

            var results = new EntityList<RoutingVersion>();
            results.AddRange(versionList.Distinct((x, y) => x.Id == y.Id && x.Name == y.Name).OrderBy(p => p.Name).ToList());
            return results;
        }

        /// <summary>
        /// 获取工艺路线版本（界面选择含产品工艺路线及产线工艺路线）
        /// </summary>
        /// <param name="type">工单类型</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="productId">产品Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <param name="processSegmentId">工段Id</param>
        /// <param name="projectId">项目Id</param>
        /// <returns>工艺路线版本列表</returns>
        public virtual EntityList<RoutingVersion> GetRoutingVersions(WorkOrderType type, DateTime startDate, double? productId = 0, double? resourceId = 0, double? processSegmentId = null, double? projectId = null)
        {
            var versionList = new EntityList<RoutingVersion>();

            if (productId.HasValue && productId > 0)
            {
                versionList.AddRange(GetProductRoutingVersion(type, startDate, productId.Value, processSegmentId, projectId));
            }

            if (resourceId.HasValue && resourceId > 0)
            {
                versionList.AddRange(GetResourceRoutingVersions(type, startDate, resourceId.Value));
            }

            var results = new EntityList<RoutingVersion>();
            results.AddRange(versionList.Distinct((x, y) => x.Id == y.Id && x.Name == y.Name).OrderBy(p => p.Name).ToList());
            return results;
        }

        /// <summary>
        /// 获取产品工艺路线版本（获取对应类型、产品、项目、时间范围内的版本）
        /// </summary>
        /// <param name="type">工单类型</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="productId">产品Id</param>
        /// <param name="processSegmentId">工段Id</param>
        /// <param name="projectId">项目Id</param>
        /// <returns>工艺路线版本列表</returns>
        protected virtual EntityList<RoutingVersion> GetProductRoutingVersion(WorkOrderType type, DateTime startDate, double productId, double? processSegmentId = null, double? projectId = null)
        {
            var product = GetById<Item>(productId);
            if (product == null)
                throw new EntityNotFoundException(typeof(Item), productId);
            var productRouting = Query<ProductRouting>()
                .Where(p => p.StartDate <= startDate && p.EndDate >= startDate && p.OrderType == type && p.ProductId == productId && p.ProcessSegmentId == processSegmentId && p.ProjectMaintainId == projectId)
                .Select(p => new { Routing_Id = p.RoutingId, Routing_Version_Id = p.RoutingVersionId }).FirstOrDefault();
            var versionId = productRouting != null ? productRouting.RoutingVersionId : null;
            var routingId = productRouting != null ? productRouting.RoutingId : null;
            // 默认获取产品工艺路线设置的版本
            var versionList = Query<RoutingVersion>().Where(p => p.RoutingId == routingId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            EntityList<RoutingVersion> orderList = new EntityList<RoutingVersion>();
            var targetVersion = versionList.FirstOrDefault(p => p.Id == versionId);
            if (targetVersion != null)
            {
                orderList.Add(targetVersion);
            }
            foreach (var i in versionList)
            {
                if (i.Id != versionId)
                {
                    orderList.Add(i);
                }
            }
            return versionList;
        }

        /// <summary>
        /// 获取资源工艺路线版本
        /// </summary>
        /// <param name="type">工单类型</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="resourceId">资源Id</param>
        /// <returns>工艺路线版本列表</returns>
        protected virtual IEnumerable<RoutingVersion> GetResourceRoutingVersions(WorkOrderType type, DateTime startDate, double resourceId)
        {
            var resource = GetById<WipResource>(resourceId);
            if (resource == null)
                throw new EntityNotFoundException(typeof(Resource), resourceId);
            var query = Query<ResourceRouting>();
            query.Where(p => p.StartDate <= startDate && p.EndDate >= startDate && p.OrderType == type && p.ResourceId == resourceId);
            var resourceRoutingList = query.ToList();
            ////return resourceRoutingList.Where(p => p.Routing != null && p.Routing.DefaultVersion != null && p.Routing.DefaultVersion.State == RoutingState.Release).Select(p => p.Routing.DefaultVersion);
            return resourceRoutingList.Where(p => p.Routing != null && p.Routing.DefaultVersion != null && p.Routing.DefaultVersion.State == RoutingState.Release).SelectMany(p => p.Routing.VersionList)
                .Where(x => x.State == RoutingState.Release).OrderByDescending(x => x.IsDefault);
        }

        /// <summary>
        /// 获取工艺路线版本
        /// </summary>
        /// <param name="routingId">产品工艺路线ID</param>
        /// <param name="orderInfos">排序条件</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <returns>工艺路线版本列表</returns>
        public virtual EntityList<RoutingVersionViewModel> GetRoutingVersionViewModels(double routingId, IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            var versions = Query<RoutingVersion>()
                .Join<ProductRouting>((v, r) => v.RoutingId == r.RoutingId && r.Id == routingId).OrderBy(orderInfos).ToList(pagingInfo);
            var result = new EntityList<RoutingVersionViewModel>();
            versions.ForEach(version =>
            {
                result.Add(new RoutingVersionViewModel()
                {
                    VersionName = version.Name,
                    IsDefault = version.IsDefault
                });
            });
            return result;
        }

        /// <summary>
        /// 获取对应工艺路线版本的工序bom工序清单(装配、包装)
        /// </summary>
        /// <param name="versionId">版本Id</param>
        /// <param name="processTypes">工序类型</param>
        /// <returns></returns>
        public virtual EntityList<RoutingProcess> GetRoutingProcesses(double versionId, IEnumerable<ProcessType?> processTypes)
        {
            return Query<RoutingProcess>().Where(p => p.VersionId == versionId && processTypes.Contains(p.Process.Type)).OrderBy(p => p.Index).ToList();
        }

        /// <summary>
        /// 获取对应工艺路线版本的工序bom工序清单(装配、包装)
        /// </summary>
        /// <param name="versionIds">版本Ids</param>
        /// <param name="processTypes">工序类型</param>
        /// <returns></returns>
        public virtual EntityList<RoutingProcess> GetRoutingProcesses(IEnumerable<double?> versionIds, IEnumerable<ProcessType?> processTypes)
        {
            return versionIds.SplitContains(tempIds => { return Query<RoutingProcess>().Where(p => versionIds.Contains(p.VersionId) && processTypes.Contains(p.Process.Type)).OrderBy(p => p.Index).ToList(); });
        }

        /// <summary>
        /// 页签加载当前产品工艺路线设置版本的工序清单
        /// </summary>
        /// <param name="routingId">产品工艺路线设置Id</param>
        /// <param name="orderInfos">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<RoutingProcessViewModel> GetProductRoutingProcesses(double routingId, IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            var q = Query<RoutingProcess>().As("rp")
                .Join<RoutingVersion>((rp, rv) => rp.VersionId == rv.Id)
                .LeftJoin<RoutingVersion, Routing>((rv, r) => rv.RoutingId == r.Id)
                .LeftJoin<Routing, ProductRouting>((r, pr) => r.Id == pr.RoutingId)
                .Where<RoutingVersion, ProductRouting>((rp, rv, pr) => pr.Id == routingId && rv.Id == pr.RoutingVersionId)
                .LeftJoin<Process>((rp, p) => rp.ProcessId == p.Id)
                .LeftJoin<Process, ProcessSegment>((p, ps) => p.SegmentId == ps.Id)
                .Select<RoutingVersion, Routing, ProductRouting, Process, ProcessSegment>((rp, rv, r, pr, p, ps) => new
                {
                    RoutingProcessId = rp.Id,
                    Index = rp.Index,
                    ProcessName = rp.Name,
                    ProcessType = p.Type,
                    SegmentName = ps.Name,
                    IsOptional = rp.IsOptional,
                    Outsourcing = rp.Outsourcing,
                    IsGenerateTask = rp.IsGenerateTask,
                    IsRequirementTask= rp.IsRequirementTask,
                });
            var count = q.Count();
            var list = q.OrderBy(rp => rp.Index).OrderBy(orderInfos).ToList<RoutingProcessViewModel>(pagingInfo).AsEntityList();
            list.SetTotalCount(count);
            return list;
        }
        #endregion

        /// <summary>
        /// 根据引用判断是否可以删除企业模型和设备模型
        /// </summary>
        /// <param name="id">来源Id</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns>true,false</returns>
        public virtual bool IsHasUsedResourse(double id, SyncSourceType sourceType)
        {
            var res = RT.Service.Resolve<WipResourceController>().GetWipResource(id, sourceType);
            if (res == null) return true;
            return Query<ResourceRouting>().Where(p => p.ResourceId == res.Id).FirstOrDefault() == null;
        }

        /// <summary>
        /// 判断产线工艺路线设置是否引用指定的生产资源
        /// </summary>
        /// <param name="wipResourceId">生产资源Id</param>
        /// <returns>bool: false--工单未引用生产资源；true--工单已引用生产资源</returns>
        public virtual bool ResourceRoutingHasUsedWipResource(double wipResourceId)
        {
            var resourceRouting = Query<ResourceRouting>().Where(x => x.ResourceId == wipResourceId).FirstOrDefault();
            return resourceRouting != null;
        }

        /// <summary>
        /// 获取产品工艺路线版本
        /// </summary>        
        /// <param name="productIds">产品Id列表</param>        
        /// <returns>工艺路线版本列表</returns>
        public virtual EntityList<ProductRouting> GetProductRoutings(List<double> productIds)
        {
            return productIds.Select(x => (double?)x).SplitContains(tempIds =>
            {
                return Query<ProductRouting>()
                    .Where(x => tempIds.Contains(x.ProductId))
                    .ToList();
            });
        }

        /// <summary>
        /// 获取资源工艺路线版本
        /// </summary>        
        /// <param name="resourceIds">资源Id</param>
        /// <returns>工艺路线版本列表</returns>
        public virtual EntityList<ResourceRouting> GetResourceRoutings(List<double> resourceIds)
        {
            return resourceIds.Select(x => (double?)x).SplitContains(tempIds =>
            {
                return Query<ResourceRouting>()
                    .Where(p => tempIds.Contains(p.ResourceId))
                    .ToList();
            });
        }

        /// <summary>
        /// 获取工艺路线
        /// </summary>
        /// <param name="routingIds">工艺路线ID列表</param>
        /// <returns></returns>
        public virtual EntityList<RoutingVersion> GetRoutingDefaultVersions(List<double> routingIds)
        {
            return routingIds.SplitContains(tempIds =>
            {
                return Query<RoutingVersion>()                    
                    .Where(x => x.State == RoutingState.Release
                        && x.IsDefault== YesNo.Yes
                        && tempIds.Contains(x.RoutingId))
                    .ToList();
            });
        }

        /// <summary>
        /// 获取工艺路线默认版本Id对应工序清单
        /// </summary>
        /// <param name="routingIds">工艺路线Ids</param>
        /// <returns></returns>
        public virtual Dictionary<double, List<DefaultRoutingProcessInfo>> GetDefaultVersionProcessDic(IEnumerable<double> routingIds)
        {
            List<DefaultRoutingProcessInfo> defaultRoutingProcessInfos = new List<DefaultRoutingProcessInfo>();
            routingIds.SplitDataExecute(tempIds =>
            {
                var list = Query<RoutingProcess>()
                .Join<RoutingVersion>((rp, rv) => rp.VersionId == rv.Id)
                .Join<RoutingVersion, Routing>((rv, r) => rv.Id == r.DefaultVersionId)
                .Where<Routing>((rp, r) => tempIds.Contains(r.Id))
                .Select<RoutingVersion, Routing>((rp, rv, r) => new
                {
                    RoutingProcessId = rp.Id,
                    DefaultVersionId = r.DefaultVersionId,
                    RoutingId = r.Id,
                }).ToList<DefaultRoutingProcessInfo>();
                defaultRoutingProcessInfos.AddRange(list);
            });
            return defaultRoutingProcessInfos.GroupBy(p => p.RoutingId).ToDictionary(p => p.Key, p => p.ToList());
        }

        /// <summary>
        /// 保存产品工艺路线设置并生成默认版本工序清单信息
        /// </summary>
        /// <param name="productRoutings">产品工艺路线保存数据</param>
        public virtual void SavingProductRouting(EntityList<ProductRouting> productRoutings)
        {
            // 生成工艺路线工序清单数据Id, 需删除原工序清单
            var deleteProcessListIds = new List<double>();
            // 找出版本为空的工艺路线Id
            var nullVersionDatas = productRoutings.Where(p => p.RoutingId != null && p.RoutingVersionId == null);
            var routingIds = nullVersionDatas.Select(p => (double)p.RoutingId).ToList();
            // 获取工艺路线默认版本工序清单
            var defaultProcessDic = GetDefaultVersionProcessDic(routingIds);
            // 创建工艺路线工序清单
            foreach (var pr in nullVersionDatas)
            {
                if (defaultProcessDic.TryGetValue(pr.RoutingId.Value, out var processList))
                {
                    pr.RoutingVersionId = processList.FirstOrDefault()?.DefaultVersionId;
                }
            }

            using(var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                // 保存
                RF.Save(productRoutings);
                tran.Complete();
            }
        }
    }
}