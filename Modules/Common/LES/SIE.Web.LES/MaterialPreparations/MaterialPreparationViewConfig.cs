using SIE.Core.ProjectMaintains;
using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialPreparations.ViewModels;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using SIE.Web.Common;
using SIE.Web.LES.MaterialPreparations.Commands;
using System;

namespace SIE.Web.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单视图配置
    /// </summary>
    public class MaterialPreparationViewConfig : WebViewConfig<MaterialPreparation>
    {
        /// <summary>
        /// 工单备料
        /// </summary>
        public const string WorkOrderModeViewStr = "WorkOrderModeViewStr";

        /// <summary>
        /// 车间备料
        /// </summary>
        public const string WorkShopModeViewStr = "WorkShopModeViewStr";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.DeclareExtendViewGroup(WorkOrderModeViewStr, WorkShopModeViewStr);
            if (ViewGroup == WorkShopModeViewStr)
            {
                WorkShopMode();
            }
            else if (ViewGroup == WorkOrderModeViewStr)
            {
                WorkOrderMode();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(AddWorkOrderPrepareCommand).FullName, typeof(AddWorkShopPrepareCommand).FullName, "SIE.Web.LES.MaterialPreparations.Commands.EditPrepareCommand", typeof(ListSubmitPreCommand).FullName, typeof(WithDrawPrepareCommand).FullName, typeof(DeletePrepareCommand).FullName, typeof(MaterialPreExportCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).HasLabel("备料单号".L10N() + '*').Readonly().ShowInList(width: 150);
                View.Property(p => p.WorkOrder).HasLabel("工单".L10N() + '*').Readonly().ShowInList(width: 150);
                View.Property(p => p.WorkShop).HasLabel("车间".L10N() + '*').Readonly().ShowInList(width: 150);
                View.Property(p => p.Resource).Readonly().ShowInList(width: 150);
                View.Property(p => p.PrepareStatus).Readonly().ShowInList();
                View.Property(p => p.PrepareType).Readonly().ShowInList();
                View.Property(p => p.Reason).UseCatalogEditor(p => { p.CatalogType = MaterialPreparation.MaterialPreReasonStr; }).Readonly().ShowInList(width: 150);
                View.Property(p => p.ProjectMaintain).Readonly().ShowInList(width: 150);
                View.Property(p => p.PrepareTime).Readonly().ShowInList(width: 150);
                View.Property(p => p.Warehouse).Readonly().ShowInList(width: 150);
                View.Property(p => p.WoProductCode).Readonly().ShowInList(width: 150);
                View.Property(p => p.WoProductName).Readonly().ShowInList(width: 150);
                View.Property(p => p.WoState).Readonly().ShowInList();
                View.Property(p => p.WoPlanQty).Readonly().ShowInList();
                View.Property(p => p.WoFinishQty).Readonly().ShowInList();
                View.Property(p => p.WoType).Readonly().ShowInList();
                View.Property(p => p.WoPlanBeginDate).Readonly().ShowInList(width: 150);
                View.Property(p => p.WoPlanEndDate).Readonly().ShowInList(width: 150);
                View.Property(p => p.WoActuStartDate).Readonly().ShowInList(width: 150);
                View.Property(p => p.WoActuFinishDate).Readonly().ShowInList(width: 150);
                View.Property(p => p.WoSaleOrderNo).Readonly().ShowInList();
                View.Property(p => p.WoCustomerOrderNo).Readonly().ShowInList();
                View.Property(p => p.Factory).Readonly().ShowInList(width: 150);
                View.Property(p => p.ShippingOrderNo).Readonly().ShowInList(width: 150);
                View.ChildrenProperty(p => p.DetailList).UseViewGroup(WebViewConfig.ListView).HasLabel("备料明细".L10N());
                View.ChildrenProperty(p => p.CancelRecordList);
            }
        }

        /// <summary>
        /// 车间备料模式
        /// </summary>
        private void WorkShopMode()
        {
            View.AddBehavior("SIE.Web.LES.MaterialPreparations.Scripts.WorkShopPrepareBehavior");
            View.UseCommands(typeof(SubmitPrepareCommand).FullName, typeof(SavePrepareCommand).FullName, "SIE.Web.LES.MaterialPreparations.Commands.CancelPrepareCommand");
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.PrepareType).HasLabel("备料类型".L10N() + '*').Readonly().ShowInDetail();
                View.Property(p => p.Factory).UseDataSource((e, p, k) =>
                {
                    var list = RT.Service.Resolve<EnterpriseController>().GetFactoriesList(p, k);
                    foreach (var item in list)
                    {
                        item.TreePId = null;
                    }
                    return list;
                }).HasLabel("工厂".L10N() + '*').ShowInDetail();
                View.Property(p => p.WorkShop).UseDataSource((e, p, k) =>
                {
                    var entity = e as MaterialPreparation;
                    var list = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(p, k, entity.FactoryId);
                    foreach (var item in list)
                    {
                        item.TreePId = null;
                    }
                    return list;
                }).HasLabel("车间".L10N() + '*').ShowInDetail();
                View.Property(p => p.PrepareTime).ShowInDetail();
                View.Property(p => p.Warehouse).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetAvailableWarehouses(p, k);
                }).ShowInDetail();

                View.Property(p => p.ProjectMaintain).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProjectMaintainController>().GetProjectMaintains(p, k);
                }).ShowInDetail();
                View.Property(p => p.Reason).UseCatalogEditor(p => { p.CatalogType = MaterialPreparation.MaterialPreReasonStr; p.CatalogReloadData = true; }).HasLabel("备料原因".L10N() + '*').ShowInDetail(columnSpan: 1);
                View.ChildrenProperty(p => p.DetailList).UseViewGroup(MaterialPreparationDetailViewConfig.WorkShopModeStr).HasLabel("备料明细".L10N()).Show(ChildShowInWhere.All);
            }
        }

        /// <summary>
        /// 工单备料
        /// </summary>
        private void WorkOrderMode()
        {
            View.AddBehavior("SIE.Web.LES.MaterialPreparations.Scripts.WorkOrderPrepareBehavior");
            View.UseCommands(typeof(SubmitPrepareCommand).FullName, typeof(SavePrepareCommand).FullName, "SIE.Web.LES.MaterialPreparations.Commands.CancelPrepareCommand");
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.PrepareType).UseEnumEditor(p => p.XType = "MpWorkOrderEnumEditor").HasLabel("备料类型".L10N() + '*').ShowInDetail();
                View.Property(p => p.WoNo).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(MaterialPrepareWoViewModel).FullName;
                    p.DisplayField = MaterialPrepareWoViewModel.WoNoProperty.Name;
                    p.XType = "materialprepareselwoeditor";
                    p.MultiOrSelect = ClientMetaModel.MultiSelect.Select;
                    p.Editable = false;
                }).HasLabel("工单".L10N() + '*').ShowInDetail();
                View.Property(p => p.WoProductCode).Readonly().ShowInDetail();
                View.Property(p => p.WoProductName).Readonly().ShowInDetail();
                View.Property(p => p.WorkShop).Readonly().ShowInDetail();
                View.Property(p => p.Resource).Readonly().ShowInDetail();
                View.Property(p => p.PrepareTime).ShowInDetail();
                View.Property(p => p.Warehouse).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetAvailableWarehouses(p, k);
                }).ShowInDetail();
                View.Property(p => p.ProjectMaintain).Readonly().ShowInDetail();
                View.ChildrenProperty(p => p.DetailList).UseViewGroup(MaterialPreparationDetailViewConfig.WorkOrderModeStr).HasLabel("备料明细".L10N()).Show(ChildShowInWhere.All);
            }
        }
    }
}
