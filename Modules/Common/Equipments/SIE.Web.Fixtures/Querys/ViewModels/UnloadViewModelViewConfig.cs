using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.EventMessages.MES.WorkOrders;
using SIE.Fixtures;
using SIE.Fixtures.Querys.ViewModels;
using SIE.Resources.WipResources;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Querys.ViewModels
{
    /// <summary>
    /// 配置出库ViewModel视图
    /// </summary>
    internal class UnloadViewModelViewConfig : WebViewConfig<UnloadViewModel>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(FixtureQueryViewModel));
            View.ClearCommands();
            View.HasDetailColumnsCount(2);
            View.Property(p => p.ResourceId).UseDataSource((e, c, r) =>
            {
                var unloadVM = e as UnloadViewModel;
                if (unloadVM == null)
                    return new EntityList<WipResource>();
                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
                return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, null, srcTypeList, c, r);
            }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true).Cascade(p => p.WorkOrderId, null)
                .UseListSetting(e => { e.HelpInfo = "显示启用状态不失效且企业类型为企业模型的生产资源"; }).HasLabel("产线").Show(ShowInWhere.Detail);
            View.Property(p => p.WorkOrderId).UseDataSource((e, c, r) =>
            {
                var unloadVM = e as UnloadViewModel;
                if (unloadVM == null)
                    return new EntityList<WorkOrder>();
                if (unloadVM.ResourceId.HasValue)
                    return RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderList(null, unloadVM.ResourceId.Value, c, r);
                else
                    return RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderList(null, null, c, r);
            }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true).UsePagingLookUpEditor().HasLabel("工单号").Show(ShowInWhere.Detail);
            View.Property(p => p.EncodeCode).Readonly();
            View.Property(p => p.AccountCode).Readonly();
            View.Property(p => p.UnloadQty).UseSpinEditor(p =>
            {
                p.AllowDecimals = false;
                p.MinValue = 1;
                p.Value = 1;
            }).Readonly(p => p.ManageMode == ManageMode.Number);
            View.Property(p => p.TurnoverToolCode);
        }
    }
}
