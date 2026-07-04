using SIE.Domain;
using SIE.MES.WIP.PackRecombine.Relations;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;

namespace SIE.Web.MES.WIP.PackRecombine.Relations
{
    /// <summary>
    /// 包装关系查询体视图配置
    /// </summary>
    internal class PackingRelationQueryCriteriaViewConfig : WebViewConfig<PackingRelationQueryCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageNo).Show(ShowInWhere.Detail);
                View.Property(p => p.Sn).Show(ShowInWhere.Detail);
                View.Property(p => p.WorkOrderNo).Show(ShowInWhere.Detail);
                View.Property(p => p.ProductId).UseProductCombinationEditor().HasLabel("产品编码").Show(ShowInWhere.Detail);
                View.Property(p => p.ResourceId).UseDataSource((e, c, r) =>
                {
                    var criteria = e as PackingRelationQueryCriteria;
                    if (criteria == null)
                        return new EntityList<WipResource>();                    
                    return RT.Service.Resolve<WipResourceController>().GetWipResourcesByKeyword(c, r);
                }).Cascade(p => p.StationId, null).UseListSetting(e => { e.HelpInfo = "显示启用状态的生产资源"; }).HasLabel("产线").Show(ShowInWhere.Detail);
                View.Property(p => p.ProcessId).UseDataSource((e, c, r) =>
                {
                    var criteria = e as PackingRelationQueryCriteria;
                    if (criteria == null)
                        return new EntityList<Process>();
                    return RT.Service.Resolve<ProcessController>().GetProcessList(c, r);
                }).Cascade(p => p.StationId, null).HasLabel("工序").Show(ShowInWhere.Detail);
                View.Property(p => p.StationId).UseDataSource((e, c, r) =>
                {
                    var criteria = e as PackingRelationQueryCriteria;
                    if (criteria == null)
                        return new EntityList<Station>();
                    return RT.Service.Resolve<ProcessController>().GetStationList(criteria.ResourceId, criteria.ProcessId, c, r);
                }).HasLabel("工位").Show(ShowInWhere.Detail);
                View.Property(p => p.IsPacked).UseEnumEditor().Show(ShowInWhere.Detail);
                View.Property(p => p.PackedDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.Month;
                }).Show(ShowInWhere.All);
            }
        }
    }
}
