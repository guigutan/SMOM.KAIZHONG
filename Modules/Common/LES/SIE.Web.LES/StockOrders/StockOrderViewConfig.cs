using AngleSharp.Dom;
using SIE.Core.WorkOrders;
using SIE.Defects;
using SIE.Domain;
using SIE.EventMessages.MES.WorkOrders;
using SIE.LES;
using SIE.LES.StockOrders;
using SIE.LES.StockOrders.Service;
using SIE.LES.StockOrders.WorkOrders;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.LES.StockOrders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.LES.StockOrders
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class StockOrderViewConfig : WebViewConfig<StockOrder>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(AddStockOrderCommand).FullName, typeof(EditStockOrderCommand).FullName, typeof(ReCallStockOrderCommand).FullName);
            View.UseCommands(typeof(SubmitStockOrderCommand).FullName, typeof(AduitStockOrderCommand).FullName, typeof(IssuedStockOrderCommand).FullName, typeof(ForceCloseStockOrderCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(150);
                View.Property(p => p.StockState);
                View.Property(p => p.StockType);
                View.Property(p => p.FactoryId);
                //View.Property(p => p.ResourceId);
                View.Property(p => p.ResourceName).HasLabel("生产资源");
                View.Property(p => p.WorkOrderId).ShowInList(120);
                View.Property(p => p.PlanBeginDate).ShowInList(150);
                View.Property(p => p.ProductCode).ShowInList(120);
                View.Property(p => p.ProductName).ShowInList(120);
                View.Property(p => p.WoQty);
                View.Property(p => p.WorkShopId);
                View.Property(p => p.BillSource);
                View.Property(p => p.TriggerMode);
                View.Property(p => p.DemandMode);
                //View.Property(p => p.NumberOfSets);
                View.Property(p => p.Remark);
            }

            View.ChildrenProperty(p => p.StockOrderDetailList).UseViewGroup(StockOrderDetailViewConfig.ReadonlyView).OrderNo = 0;
            View.AttachChildrenProperty(typeof(StockOrderSn), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var bill = args.Parent.CastTo<StockOrder>();
                if (bill == null)
                {
                    return new EntityList<StockOrderSn>();
                }

                return RT.Service.Resolve<StockOrderSnService>().GetStockOrderSns(bill.Id, args.PagingInfo);
            }, StockOrderSnViewConfig.ReadonlyView).HasLabel("接收记录").OrderNo = 1;
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(4);
            View.AddBehavior("SIE.Web.LES.StockOrder.StockOrderBehavior");
            View.UseCommands(typeof(SaveStockOrderCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInDetail().Readonly();
                View.Property(p => p.StockState).ShowInDetail().Readonly();
                View.Property(p => p.StockType).Cascade(p => p.WorkOrderId, null).Cascade(p => p.WoNo, null)
                  .ShowInDetail().HasLabel("*"+"备料模式".L10N()).Readonly(p => p.StockState != StockState.Created);
                View.Property(p => p.WoNo).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(StockOrderWoViewModel).FullName;
                    p.DisplayField = StockOrderWoViewModel.WoNoProperty.Name;
                    p.XType = "selectworkordereditor";
                    p.MultiOrSelect = ClientMetaModel.MultiSelect.Select;
                    p.Editable = false;
                }).Cascade(p => p.FactoryId, null).Cascade(p => p.WorkShopId, null).Cascade(p => p.ResourceId, null)
                .Cascade(p => p.PlanBeginDate, null).Cascade(p => p.ProductName, null).Cascade(p => p.WoQty, null).ShowInDetail().Readonly(p => p.StockType == PrepareItemType.Pull || p.StockState != StockState.Created);
                View.Property(p => p.FactoryId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var enterpriseList = RT.Service.Resolve<EnterpriseController>().GetEmployeeFactoriesList(pagingInfo, keyword);
                    if (enterpriseList == null || enterpriseList.Count <= 0)
                        return new EntityList<Enterprise>();
                    for (var i = 0; i < enterpriseList.Count; i++)
                    {
                        enterpriseList[i].TreePId = null;
                    }
                    return enterpriseList;
                }).ShowInDetail().Cascade(p => p.WorkShop, null).Cascade(p => p.Resource, null).Readonly(p => p.StockState != StockState.Created || p.StockType != PrepareItemType.Pull);
                View.Property(p => p.WorkShopId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var bill = source as StockOrder;
                    if (bill == null || bill.FactoryId < 0)
                    {
                        return new EntityList<Enterprise>();
                    }

                    var workshop = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword, bill.FactoryId);
                    if (workshop == null || workshop.Count <= 0)
                        return new EntityList<Enterprise>();
                    workshop.ForEach(p => p.TreePId = null);
                    return workshop;
                }).Cascade(p => p.Resource, null).ShowInDetail().Readonly(p => p.StockState != StockState.Created || p.StockType != PrepareItemType.Pull);
                View.Property(p => p.ResourceId).UseDataSource((e, c, r) =>
                {
                    var workOrder = e as StockOrder;
                    if (workOrder == null)
                        return new EntityList<WipResource>();
                    return RT.Service.Resolve<StockOrderService>().GetActiveWipResource(workOrder.WorkShopId, c, r);
                }).UsePagingLookUpEditor(p => p.DisplayField = "Name").ShowInDetail().Readonly(p => p.StockState != StockState.Created || p.StockType != PrepareItemType.Pull);
                
                View.Property(p => p.PlanBeginDate).UseDateTimeEditor().ShowInDetail().Readonly();
                View.Property(p => p.ProductName).ShowInDetail().Readonly();
                View.Property(p => p.WoQty).ShowInDetail().Readonly();
                View.Property(p => p.BillSource).ShowInDetail().Readonly();
                View.Property(p => p.TriggerMode).ShowInDetail().Readonly().HasLabel("*"+"触发方式".L10N());
                View.Property(p => p.PushDemandMode).ShowInDetail().Readonly(p => p.StockState != StockState.Created).Visibility(p => p.StockType == PrepareItemType.Push).HasLabel("*"+"需求计算方式".L10N());
                View.Property(p => p.PullDemandMode).ShowInDetail().Readonly(p => p.StockState != StockState.Created).Visibility(p => p.StockType == PrepareItemType.Pull).HasLabel("*"+"需求计算方式".L10N());
                View.Property(p => p.DemandMode).UseEnumEditor(p => p.FilterCategoery = "Manual").ShowInDetail().Readonly(p => p.StockState != StockState.Created).Visibility(p => p.StockType == PrepareItemType.OverBom).HasLabel("*" + "需求计算方式".L10N());
                View.Property(p => p.NumberOfSets).Visibility(p => p.DemandMode == SIE.LES.Commons.DemandMode.ManualSetQuantity).ShowInDetail().UseFormSetting(p=>p.HelpInfo="手工填写-按套数备料时可编辑");
            }

            View.ChildrenProperty(p => p.StockOrderDetailList).Show(ChildShowInWhere.All);
        }
    }
}