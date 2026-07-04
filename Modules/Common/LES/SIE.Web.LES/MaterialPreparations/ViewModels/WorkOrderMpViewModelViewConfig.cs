using SIE.Domain;
using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialPreparations.ViewModels;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.LES.MaterialPreparations.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialPreparations.ViewModels
{
    /// <summary>
    /// 工单备料汇总
    /// </summary>
    public class WorkOrderMpViewModelViewConfig : WebViewConfig<WorkOrderMpViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(WorkOrderMpViewExportCommand).FullName);
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).HasLabel("工单").ShowInList(width: 150);
                View.Property(p => p.ProductCode).ShowInList();
                View.Property(p => p.ProductName).ShowInList();
                View.Property(p => p.WorkShopName).ShowInList();
                View.Property(p => p.ResourceName).ShowInList();
                View.Property(p => p.WoState).ShowInList();
                View.Property(p => p.PlanQty).ShowInList();
                View.Property(p => p.FinishQty).ShowInList();
                View.Property(p => p.WoType).ShowInList();
                View.Property(p => p.PlanBeginDate).ShowInList(width: 150);
                View.Property(p => p.PlanEndDate).ShowInList(width: 150);
                View.Property(p => p.ActuStartDate).ShowInList(width: 150);
                View.Property(p => p.ActuFinishDate).ShowInList(width: 150);
                View.Property(p => p.SaleOrderNo).ShowInList();
                View.Property(p => p.CustomerOrderNo).ShowInList();
                View.Property(p => p.FactoryName).ShowInList();
                View.AttachChildrenProperty(typeof(WorkOrderMpDetailViewModel), (e) =>
                {
                    var args = e as ChildPagingDataArgs;
                    var parent = e.Parent as WorkOrderMpViewModel;
                    return RT.Service.Resolve<MaterialPreparationController>().GetWorkOrderMpDetailViewModels(parent.Id, args.PagingInfo);
                }).Show(ChildShowInWhere.All);
            }
        }
    }


    /// <summary>
    /// 备料需求单汇总
    /// </summary>
    public class WorkOrderMpViewModelCriteriaViewConfig : WebViewConfig<WorkOrderMpViewModelCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WoNo).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.Factory).UseDataSource((e, p, k) =>
                {
                    var list = RT.Service.Resolve<EnterpriseController>().GetEmployeeFactoriesList(p, k);
                    foreach (var factory in list)
                    {
                        factory.TreePId = 0;
                    }
                    return list;
                }).Cascade(p => p.WorkshopId, null).Cascade(p => p.WipResourceId, null).Show();
                View.Property(p => p.Workshop).UseDataSource((e, p, k) =>
                {
                    var entity = e as MaterialPrepareWoCriteria;
                    var list = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(p, k, entity.FactoryId);
                    foreach (var workshop in list)
                    {
                        workshop.TreePId = 0;
                    }
                    return list;
                }).Cascade(p => p.WipResourceId, null).Show();
                View.Property(p => p.WipResource).UseDataSource((e, p, k) =>
                {
                    var entity = e as MaterialPrepareWoCriteria;
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
                    if (entity.WorkshopId.HasValue)
                    {
                        return RT.Service.Resolve<WipResourceController>()
                            .GetWipResourcesByWorkShopId(stateList, new List<double?> { entity.WorkshopId.Value },
                            srcTypeList, p, k);
                    }

                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, entity.WorkshopId, srcTypeList, p, k);
                }).Show();
                View.Property(p => p.PlanBeginDateRange).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month).Show();
            }
        }
    }

    /// <summary>
    /// 工单备料汇总明细视图配置
    /// </summary>
    public class WorkOrderMpDetailViewModelViewConfig : WebViewConfig<WorkOrderMpDetailViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Show();
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();
                View.Property(p => p.UnitName).Show();
                View.Property(p => p.ItemExtPropName).Show();
                View.Property(p => p.ConsumeMode).Show();
                View.Property(p => p.BomNeedQty).Show();
                View.Property(p => p.HasQty).Show();
                View.Property(p => p.CanPrepareQty).Show();
                View.Property(p => p.HasReceiveQty).Show();
                View.Property(p => p.ToReceiveQty).Show();
                View.Property(p => p.HasShippingQty).Show();
                View.Property(p => p.MoveInQty).Show();
                View.Property(p => p.MoveOutQty).Show();
                View.Property(p => p.CancelQty).Show();
                View.Property(p => p.ReturnQty).Show();
            }
        }
    }
}
