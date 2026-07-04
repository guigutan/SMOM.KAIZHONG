using SIE.Core.ProjectMaintains;
using SIE.LES.MaterialReturnApplys;
using SIE.LES.MaterialReturnApplys.Enums;
using SIE.LES.MaterialReturnApplys.ViewModels;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using SIE.Web.Common;
using SIE.Web.LES.MaterialReturnApplys.Commands;
using System;

namespace SIE.Web.LES.MaterialReturnApplys
{
    /// <summary>
    /// 退料申请视图配置
    /// </summary>
    public class MaterialReturnApplyViewConfig : WebViewConfig<MaterialReturnApply>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
        }


        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(MaterialReAddCommand).FullName, "SIE.Web.LES.MaterialReturnApplys.Commands.MaterialReturnEditCommand",
                typeof(MaterialReturnDeleteCommand).FullName, typeof(MaterialReturnListSubmitCommand).FullName, typeof(MaterialReturnCancelCommand).FullName,
                "SIE.Web.LES.MaterialReturnApplys.Commands.ViewLabelCommand", typeof(MaterialReturnExportCommand).FullName);
            using(View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(width: 150);
                View.Property(p => p.ReStatus).ShowInList();
                View.Property(p => p.WoNo).ShowInList();
                View.Property(p => p.WorkShop).ShowInList();
                View.Property(p => p.WipResource).ShowInList();
                View.Property(p => p.Warehouse).ShowInList();
                View.Property(p => p.StorageLocation).ShowInList();
                View.Property(p => p.Reason).UseCatalogEditor(p => { p.CatalogType = MaterialReturnApply.MaterialReturnReasonStr; p.CatalogReloadData = true; }).ShowInList();
                View.ChildrenProperty(p => p.DetailList).Show(ChildShowInWhere.List);
            }
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.LES.MaterialReturnApplys.Scripts.MaterialReturnBehavior");
            View.UseCommands(typeof(MaterialReturnSubmitCommand).FullName, typeof(MaterialReturnSaveCommand).FullName, "SIE.Web.LES.MaterialReturnApplys.Commands.CancelReturnCommand");
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().ShowInDetail();
                View.Property(p => p.ReType).ShowInDetail();
                View.Property(p => p.WoNo).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(MaterialReturnWoViewModel).FullName;
                    p.DisplayField = MaterialReturnWoViewModel.WoNoProperty.Name;
                    p.XType = "materialreturnselwoeditor";
                    p.MultiOrSelect = ClientMetaModel.MultiSelect.Select;
                    p.Editable = false;
                }).HasLabel("工单").Readonly(p => p.ReType == ReType.WorkShopReturn).ShowInDetail();
                View.Property(p => p.WipResource).Readonly().ShowInDetail();
                View.Property(p => p.WorkShop).UseDataSource((e, p, k) =>
                {
                    var list = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(p, k);
                    foreach (var i in list)
                    {
                        i.TreePId = null;
                    }
                    return list;
                }).Readonly(p => p.ReType == ReType.WorkOrderReturn).HasLabel("车间".L10N() + "*").ShowInDetail();
                
                View.Property(p => p.Warehouse).Readonly().ShowInDetail();
                View.Property(p => p.StorageLocation).Readonly().ShowInDetail();
                View.Property(p => p.ReceiveWarehouse).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetAvailableWarehouses(p, k);
                });
                View.Property(p => p.Project).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProjectMaintainController>().GetProjectMaintains(p, k);
                }).Readonly(p => p.ReType == ReType.WorkOrderReturn).ShowInDetail();
                View.Property(p => p.Reason).HasLabel("退料原因".L10N() + "*").UseCatalogEditor(p => { p.CatalogType = MaterialReturnApply.MaterialReturnReasonStr; p.CatalogReloadData = true; }).ShowInDetail();
                View.ChildrenProperty(p => p.DetailList).UseViewGroup(MaterialReturnApplyDetailViewConfig.EditViewStr).Show(ChildShowInWhere.Detail);
            }
        }
    }
}
