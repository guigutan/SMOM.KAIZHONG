using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using System.Collections.Generic;
using System.Runtime.Versioning;

namespace SIE.Web.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 派工管理查询实体视图配置
    /// </summary>
    internal class DispatchTaskCriteriaViewConfig : WebViewConfig<DispatchTaskCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.MES.TaskManagement.Dispatchs.Commands.QueryDispatchTaskInfo");
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.Detail);
                View.Property(p => p.ResourceName).Show(ShowInWhere.Detail);
                View.Property(p => p.OldItem).Show(ShowInWhere.Detail);
                View.Property(p => p.ProductCode).Show(ShowInWhere.Detail);
                View.Property(p => p.ProductName).Show(ShowInWhere.Detail);
                View.Property(p => p.WorkShop)/*.UseResourceWorkShopEditor()*/.Show(ShowInWhere.Detail);
                //.UseListSetting(e => { e.HelpInfo = "更改车间清空资源"; });
                //View.Property(p => p.Resource).Show(ShowInWhere.Detail).UseDataSource((e, c, r) =>
                //{
                //    var criteria = e as DispatchTaskCriteria;
                //    if (criteria == null /*|| criteria.WorkShop == null*/)
                //        return new EntityList<WipResource>();

                //    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                //    var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment, SyncSourceType.LineAndon, SyncSourceType.WorkCenter };
                //    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, null, srcTypeList, c, r);
                //}).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true)
                //.UseListSetting(e => { e.HelpInfo = "显示启用状态不失效且来源类型为企业模型的生产资源"; });
                View.Property(p => p.ResourceCode).Show(ShowInWhere.Detail);
                
                View.Property(p => p.ProductCode).Show(ShowInWhere.Detail);
                View.Property(p => p.PlanBeginTime).Show(ShowInWhere.Detail).UseDateRangeEditor(p =>
                {
                    p.Format = "Y/m/d";
                    p.DateRangeType = ObjectModel.DateRangeType.Week;
                });
                View.Property(p => p.PlanEndTime).Show(ShowInWhere.Detail).UseDateRangeEditor(p =>
                {
                    p.Format = "Y/m/d";
                    p.DateRangeType = ObjectModel.DateRangeType.Week;
                });
                //View.Property(p => p.WorkOrder).UsePagingLookUpEditor().Show(ShowInWhere.Detail);
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.Detail);
                View.Property(p => p.Fevor).Show(ShowInWhere.Detail);
                View.Property(p => p.Process).UsePagingLookUpEditor().Show(ShowInWhere.Detail);
                View.Property(p => p.TaskStatus).UseEnumMutilEditor(x=>x.EnumType=typeof(DispatchTaskStatus)).Show(ShowInWhere.Detail);
                View.Property(p => p.ReportMode).Show(ShowInWhere.Detail);
                View.Property(p => p.AdoName).Show(ShowInWhere.Detail);
                View.Property(p => p.IsSyntype).Show(ShowInWhere.Detail);
                View.Property(p => p.IsVirtualPart).Show(ShowInWhere.Detail);
                View.Property(p => p.IsShowDispatchTask).Show(ShowInWhere.Detail);
                View.Property(p => p.IsSchedulingInfReturn).Show(ShowInWhere.Detail);
                View.Property(p => p.IsClose).Show(ShowInWhere.Detail);
                View.Property(p => p.ImportTime).Show(ShowInWhere.Detail).UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.All; });
            }
        }
    }
}
