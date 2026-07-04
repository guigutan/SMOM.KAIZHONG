using SIE.Domain;
using SIE.MES.WorkOrderArchives;
using SIE.MES.WorkOrderArchives.Services;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案视图配置
    /// </summary>
    public class WorkOrderArchiveViewConfig : WebViewConfig<WorkOrderArchive>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(width: 150);
                View.Property(p => p.ProCode).ShowInList(width: 150);
                View.Property(p => p.ProName).ShowInList(width: 150);
                View.Property(p => p.WoState).ShowInList(width: 150);
                View.Property(p => p.PlanQty).ShowInList(width: 150);
                View.Property(p => p.FinishQty).ShowInList(width: 150);
                View.Property(p => p.ScrapQty).ShowInList(width: 150);
                View.Property(p => p.WoType).ShowInList(width: 150);
                View.Property(p => p.PlanBeginDate).ShowInList(width: 150);
                View.Property(p => p.PlanEndDate).ShowInList(width: 150);
                View.Property(p => p.ActuStartDate).ShowInList(width: 150);
                View.Property(p => p.ActuFinishDate).ShowInList(width: 150);
                View.Property(p => p.ItemExtPropName).HasLabel("物料拓展属性").ShowInList(width: 150);
                View.Property(p => p.RetrospectType).ShowInList(width: 150);
                View.Property(p => p.Factory).ShowInList(width: 150);
                View.Property(p => p.WorkShop).ShowInList(width: 150);
                View.Property(p => p.Resource).ShowInList(width: 150);
                View.Property(p => p.Version).ShowInList(width: 150);
                AttachView();
            }
        }

        private void AttachView()
        {
            using (View.OrderProperties())
            {
                View.UseClientOrder();
                View.AttachChildrenProperty(typeof(WoOrderArchiveItemCostViewModel), (e) =>
                {
                    var archiveProduce = e.Parent as WorkOrderArchive;
                    var args = e as ChildPagingDataArgs;
                    if (archiveProduce == null)
                    {
                        return new EntityList<WoOrderArchiveItemCostViewModel>();
                    }
                    else
                    {
                        return RT.Service.Resolve<WorkOrderArchiveService>().QueryWoOrderArchiveItemCostList(archiveProduce, args.SortInfo, args.PagingInfo);
                    }
                }).HasLabel("物料耗用").HasOrderNo(10);
                View.AttachChildrenProperty(typeof(WoOrderArchiveProcessViewModel), (e) =>
                {
                    var archiveProduce = e.Parent as WorkOrderArchive;
                    var args = e as ChildPagingDataArgs;
                    if (archiveProduce == null)
                    {
                        return new EntityList<WoOrderArchiveProcessViewModel>();
                    }
                    else
                    {
                        return RT.Service.Resolve<WorkOrderArchiveService>().QueryWoOrderArchiveProcessList(archiveProduce.Id, args.SortInfo, args.PagingInfo);
                    }
                }).HasLabel("生产采集").HasOrderNo(20);
                View.AttachChildrenProperty(typeof(WoOrderArchiveProduceViewModel), (e) =>
                {
                    var archiveProduce = e.Parent as WorkOrderArchive;
                    var args = e as ChildPagingDataArgs;
                    if (archiveProduce == null)
                    {
                        return new EntityList<WoOrderArchiveProduceViewModel>();
                    }
                    else
                    {
                        return RT.Service.Resolve<WorkOrderArchiveService>().QueryWoOrderArchiveProduceList(archiveProduce, args.SortInfo, args.PagingInfo);
                    }
                }).HasLabel("工单产出").HasOrderNo(30);
                View.AttachChildrenProperty(typeof(WoOrderArchiveReportViewModel), (e) =>
                {
                    var archiveProduce = e.Parent as WorkOrderArchive;
                    var args = e as ChildPagingDataArgs;
                    if (archiveProduce == null)
                    {
                        return new EntityList<WoOrderArchiveReportViewModel>();
                    }
                    else
                    {
                        return RT.Service.Resolve<WorkOrderArchiveService>().QueryWoOrderArchiveReportList(archiveProduce.Id, args.SortInfo, args.PagingInfo);
                    }
                }).HasLabel("报工记录").HasOrderNo(40);
                View.AttachChildrenProperty(typeof(WorkOrderArchiveItemlLabelViewModel), (e) =>
                {
                    var archiveProduce = e.Parent as WorkOrderArchive;
                    var args = e as ChildPagingDataArgs;
                    if (archiveProduce == null)
                    {
                        return new EntityList<WorkOrderArchiveItemlLabelViewModel>();
                    }
                    else
                    {
                        return RT.Service.Resolve<WorkOrderArchiveService>().QueryWorkOrderArchiveItemlLabelList(archiveProduce.Id, args.SortInfo, args.PagingInfo);
                    }
                }).HasLabel("待用标签").HasOrderNo(50);
                View.AttachChildrenProperty(typeof(WorkOrderArchiveItemShortViewModel), (e) =>
                {
                    var archiveProduce = e.Parent as WorkOrderArchive;
                    var args = e as ChildPagingDataArgs;
                    if (archiveProduce == null)
                    {
                        return new EntityList<WorkOrderArchiveItemShortViewModel>();
                    }
                    else
                    {
                        return RT.Service.Resolve<WorkOrderArchiveService>().QueryWorkOrderArchiveItemShortList(archiveProduce.Id, args.SortInfo, args.PagingInfo);
                    }
                }).HasLabel("缺料情况").HasOrderNo(60);
            }
        }
    }
}
