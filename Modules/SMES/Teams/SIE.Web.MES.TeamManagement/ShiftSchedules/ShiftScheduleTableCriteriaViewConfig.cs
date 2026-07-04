using SIE.Domain;
using SIE.MES.TeamManagement.ShiftSchedules;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 排班表查询实体视图配置
    /// </summary>
    internal class ShiftScheduleTableCriteriaViewConfig : WebViewConfig<ShiftScheduleTableCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleQuery");
            View.Property(p => p.WorkGroup).ShowInDetail();
            View.Property(p => p.WorkShop).UseDataSource((e, p, s) =>
            {
                var res = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(p, s);
                res.ForEach(f => f.TreePId = null);
                return res;
            }).UsePagingLookUpEditor().Cascade(p => p.WipResource, null).ShowInDetail()
            .UseListSetting(e => { e.HelpInfo = "显示企业类型为车间的企业资源，更改车间清空资源"; });
            View.Property(p => p.WipResource).UseDataSource((e, p, s) =>
            {
                var criteria = e as ShiftScheduleTableCriteria;
                if (!criteria.WorkShopId.HasValue)
                    return new EntityList<WipResource>();
                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                var sourceType = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
                return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.WorkShopId.Value, sourceType, p, s);
            }).UsePagingLookUpEditor().ShowInDetail()
            .UseListSetting(e => { e.HelpInfo = "显示启用状态不失效的生产资源"; });
            View.Property(p => p.ScheduleDate).HasLabel("排班日期").UseDateRangeEditor(e => { e.DateFormat = "Y/m/d"; e.DateRangeType = ObjectModel.DateRangeType.Month; }).ShowInDetail();
        }
    }
}