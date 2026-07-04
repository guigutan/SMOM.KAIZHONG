using SIE.Domain;
using SIE.Items;
using SIE.MES.Outsourcing;
using SIE.MES.ProjectDesigns.Enums;
using SIE.MES.WorkOrders;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.ProjectDesigns.Reports
{
    /// <summary>
    /// 项目号跟踪报表控制器
    /// </summary>
    public class ProjectDesignReportController : DomainController
    {
        /// <summary>
        /// 查询项目号跟踪报表
        /// </summary>
        /// <param name="criteria">项目号跟踪报表查询实体</param>
        /// <returns></returns>
        public virtual EntityList<ProjectDesignReport> QueryProjectDesignReport(ProjectDesignReportCriteria criteria)
        {
            EntityList<ProjectDesignReport> projectDesignReports = new EntityList<ProjectDesignReport>();
            var q = Query<ProjectDesign>();
            if (criteria.ProjectMaintainId != null)
            {
                q.Where(p => p.ProjectMaintainId == criteria.ProjectMaintainId);
            }
            if (criteria.ProductId != null)
            {
                q.Where(p => p.ProductId == criteria.ProductId);
            }
            if (criteria.SaleOrderNo.IsNotEmpty())
            {
                q.Where(p => p.SaleOrderNo.Contains(criteria.SaleOrderNo));
            }
            switch (criteria.DesignStatus)
            {
                case Enums.DesignStatus.Create:
                    q.Where(p => p.ExamineStatus == ExamineStatus.UnExamine
                && p.BaseInfo == ChildInfoStatus.UnFill && p.RoutingInfo == ChildInfoStatus.UnFill && p.BomInfo == ChildInfoStatus.UnFill);
                    break;
                case DesignStatus.DesignIng:
                    q.Where(p => p.ExamineStatus == ExamineStatus.UnExamine
                && (p.BaseInfo == ChildInfoStatus.UnFill || p.RoutingInfo == ChildInfoStatus.UnFill || p.BomInfo == ChildInfoStatus.UnFill));
                    break;
                case DesignStatus.ToExamine:
                    q.Where(p => p.ExamineStatus == ExamineStatus.UnExamine
                && p.BaseInfo == ChildInfoStatus.HasFilled && p.RoutingInfo == ChildInfoStatus.HasFilled && p.BomInfo == ChildInfoStatus.HasFilled);
                    break;
                case DesignStatus.Complete:
                    q.Where(p => p.ExamineStatus == ExamineStatus.Examined
                && p.BaseInfo == ChildInfoStatus.HasFilled && p.RoutingInfo == ChildInfoStatus.HasFilled && p.BomInfo == ChildInfoStatus.HasFilled);
                    break;
                default:
                    break;
            }
            switch (criteria.ProduceStatus)
            {
                case ProduceStatus.UnProduct:
                    q.NotExists<WorkOrder>((a, w) => w.Where(o => o.ProjectMaintainId == a.ProjectMaintainId));
                    break;
                case ProduceStatus.ToProduct:
                    q.Exists<WorkOrder>((p, ew) => ew.Where(x => x.ProjectMaintainId == p.ProjectMaintainId))
                        .NotExists<SIE.Core.WorkOrders.WorkOrder>((p, nw) => nw.Where(y => y.ProjectMaintainId == p.ProjectMaintainId && y.State != Core.WorkOrders.WorkOrderState.Release));
                    break;
                case ProduceStatus.Producting:
                    q.Exists<WorkOrder>((a, w) => w.Where(o => o.ProjectMaintainId == a.ProjectMaintainId && o.State == Core.WorkOrders.WorkOrderState.Producing));
                    break;
                case ProduceStatus.Complete:
                    q.Exists<WorkOrder>((p, ew) => ew.Where(x => x.ProjectMaintainId == p.ProjectMaintainId))
                        .NotExists<SIE.Core.WorkOrders.WorkOrder>((p, nw) => nw.Where(y => y.ProjectMaintainId == p.ProjectMaintainId && y.State != Core.WorkOrders.WorkOrderState.Finish));
                    break;
                default: break;
            }
            if (criteria.DeliveryDate.BeginValue.HasValue)
            {
                q.Where(p => p.DeliveryDate >= criteria.DeliveryDate.BeginValue);
            }
            if (criteria.DeliveryDate.EndValue.HasValue)
            {
                q.Where(p => p.DeliveryDate <= criteria.DeliveryDate.EndValue);
            }
            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            // 查询出项目号对应的工单信息
            var projectWoInfo = GetProjectWoInfos(list.Select(p => p.ProjectMaintainId).Distinct());
            foreach (var i in list)
            {
                var produceStatus = GetProduceStatus(i.ProjectMaintainId, projectWoInfo);
                ProjectDesignReport report = new ProjectDesignReport
                {
                    Id = i.Id.ToString(),
                    ProjectMaintainId = i.ProjectMaintainId,
                    ProjectMaintain = i.ProjectMaintainCode,
                    ProductCode = i.ProductCode,
                    ProductName = i.ProductName,
                    SpecificationModel = i.SpecificationModel,
                    ExamineStatus = i.ExamineStatus,
                    SaleOrderNo = i.SaleOrderNo,
                    CustomerCode = i.CustomerCode,
                    CustomerName = i.CustomerName,
                    Qty = i.Qty,
                    Unit = i.Unit,
                    DeliveryDate = i.DeliveryDate,
                    BaseInfo = i.BaseInfo,
                    RoutingInfo = i.RoutingInfo,
                    BomInfo = i.BomInfo,
                    AttachInfo = i.AttachInfo,
                    ProduceStatus = produceStatus,
                };
                projectDesignReports.Add(report);
            }
            projectDesignReports.SetTotalCount(list.TotalCount);
            return projectDesignReports;
        }

        /// <summary>
        /// 查询项目号工单信息
        /// </summary>
        /// <param name="projectIds">项目Ids</param>
        /// <returns></returns>
        private ILookup<double, ProjectWoInfo> GetProjectWoInfos(IEnumerable<double> projectIds)
        {
            List<ProjectWoInfo> projectWoInfos = new List<ProjectWoInfo>();
            projectIds.SplitDataExecute(temps =>
            {
                var list = Query<WorkOrder>().Where(p => p.ProjectMaintainId != null && temps.Contains((double)p.ProjectMaintainId)).Select(p => new
                {
                    ProjectMaintainId = p.ProjectMaintainId,
                    WoId = p.Id,
                    State = p.State,
                }).ToList<ProjectWoInfo>();
                projectWoInfos.AddRange(list);
            });
            return projectWoInfos.ToLookup(p => p.ProjectMaintainId);
        }

        /// <summary>
        /// 获取生产状态
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="projectWoInfos">项目对应工单状态信息</param>
        /// <returns></returns>
        private ProduceStatus GetProduceStatus(double projectId, ILookup<double, ProjectWoInfo> projectWoInfos)
        {
            var infos = projectWoInfos[projectId];
            if (infos != null)
            {
                if (infos.All(p => p.State == Core.WorkOrders.WorkOrderState.Release)) return ProduceStatus.ToProduct;
                else if (infos.Any(p => p.State == Core.WorkOrders.WorkOrderState.Producing)) return ProduceStatus.Producting;
                else if (infos.All(p => p.State == Core.WorkOrders.WorkOrderState.Finish)) return ProduceStatus.Complete;
                else return ProduceStatus.UnProduct;
            }
            else
            {
                return ProduceStatus.UnProduct;
            }
        }

        /// <summary>
        /// 页签加载项目号需求设计项目关联工单
        /// </summary>
        /// <param name="projectDesignIdStr">项目号需求设计Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<ProjectWorkOrderViewModel> GetProjectWorkOrderViewModels(string projectDesignIdStr, PagingInfo pagingInfo)
        {
            if (Double.TryParse(projectDesignIdStr, out var projectDesignId))
            {
                var projectId = Query<ProjectDesign>().Where(p => p.Id == projectDesignId).Select(p => new { p.ProjectMaintainId }).ToList<double>().FirstOrDefault();
                if (projectId != 0)
                {
                    EntityList<ProjectWorkOrderViewModel> projectWorkOrderViewModels = new EntityList<ProjectWorkOrderViewModel>();
                    var q = Query<WorkOrder>().LeftJoin<Item>((w, i) => w.ProductId == i.Id).Where(w => w.ProjectMaintainId == projectId);
                    var totalCount = q.Count();
                    var projectWoList = q
                        .Select<Item>((w, i) => new
                        {
                            WoNo = w.No,
                            ProductCode = i.Code,
                            ProductName = i.Name,
                            WorkOrderState = w.State,
                            PlanQty = w.PlanQty,
                            FinishQty = w.FinishQty,
                            PlanBeginDate = w.PlanBeginDate,
                            PlanEndDate = w.PlanEndDate,
                        }).ToList<ProjectWorkOrderViewModel>(pagingInfo);
                    projectWorkOrderViewModels.AddRange(projectWoList);
                    projectWorkOrderViewModels.SetTotalCount(totalCount);
                    return projectWorkOrderViewModels;
                }
            }
            return new EntityList<ProjectWorkOrderViewModel>();
        }

        /// <summary>
        /// 页签加载项目号关联工序委外需求的
        /// </summary>
        /// <param name="projectDesignIdStr">项目号需求设计Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<ProjectOutsourcingViewModel> GetProjectOutsourcingViewModels(string projectDesignIdStr, PagingInfo pagingInfo)
        {
            if (Double.TryParse(projectDesignIdStr, out var projectDesignId))
            {
                var projectId = Query<ProjectDesign>().Where(p => p.Id == projectDesignId).Select(p => new { p.ProjectMaintainId }).ToList<double>().FirstOrDefault();
                if (projectId != 0)
                {
                    EntityList<ProjectOutsourcingViewModel> projectOutsourcingViewModels = new EntityList<ProjectOutsourcingViewModel>();
                    var q = Query<OutsourcingRequest>().Join<WorkOrder>((osr, wo) => osr.WorkOrderId == wo.Id && wo.ProjectMaintainId == projectId)
                        .LeftJoin<Process>("sp", (osr, sp) => osr.BeginProcessId == sp.Id)
                        .LeftJoin<Process>("ep", (osr, ep) => osr.EndProcessId == ep.Id)
                    .LeftJoin<WorkOrder, Item>((wo, i) => wo.ProductId == i.Id);
                    var totalCount = q.Count();
                    var outSourceList = q.Select<WorkOrder, Item, Process, Process>((osr, wo, i, sp, ep) => new
                    {
                        No = osr.NO,
                        WoNo = wo.No,
                        ProductCode = i.Code,
                        ProductName = i.Name,
                        OutsourcingState = osr.OutsourcingState,
                        RequestQty = osr.RequestQty,
                        OutboundQty = osr.OutboundQty,
                        WarehousingQty = osr.WarehousingQty,
                        BeginProcess = sp.Name,
                        EndProcess = ep.Name,
                    }).ToList<ProjectOutsourcingViewModel>(pagingInfo);
                    projectOutsourcingViewModels.AddRange(outSourceList);
                    projectOutsourcingViewModels.SetTotalCount(totalCount);
                }
            }
            return new EntityList<ProjectOutsourcingViewModel>();
        }
    }
}
