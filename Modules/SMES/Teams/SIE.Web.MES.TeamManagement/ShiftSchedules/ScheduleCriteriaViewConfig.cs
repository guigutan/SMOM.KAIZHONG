using SIE.Domain;
using SIE.MES.TeamManagement.ShiftSchedules;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 排班查询实体视图配置
    /// </summary>
    internal class ScheduleCriteriaViewConfig : WebViewConfig<ScheduleCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.RequirModuleResource("SIE.Web.MES.TeamManagement.ShiftSchedules.Commands.ScheduleQueryInfo.js");
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleQueryInfo");
            View.Property(p => p.WorkShop).HasLabel("车间".L10N() + "*").UseResourceWorkShopEditor().Cascade(p => p.WipResource, null).ShowInDetail()
                .UseListSetting(e => { e.HelpInfo = "显示企业类型为车间的企业资源，更改车间清空资源"; });
            View.Property(p => p.WipResource).HasLabel("资源".L10N() + "*").UseDataSource((e, p, s) =>
            {
                var criteria = e as ScheduleCriteria;
                if (!criteria.WorkShopId.HasValue)
                    return new EntityList<WipResource>();
                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                var sourceType = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
                return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.WorkShopId.Value, sourceType, p, s);
            }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true).ShowInDetail()
            .UseListSetting(e => { e.HelpInfo = "显示启用状态不失效的生产资源"; });
            View.Property(p => p.ScheduleDate).HasLabel("排班日期".L10N()+"*").UseDateRangeEditor(e => { e.DateFormat = "Y/m/d"; e.DateRangeType = ObjectModel.DateRangeType.Month; }).ShowInDetail();
        }
    }
}