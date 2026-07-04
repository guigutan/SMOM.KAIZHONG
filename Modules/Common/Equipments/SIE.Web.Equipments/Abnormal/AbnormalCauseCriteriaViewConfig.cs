using SIE.Domain;
using SIE.Equipments.Abnormal;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.Common;
using System.Collections.Generic;

namespace SIE.Web.Equipments.Abnormal
{
    /// <summary>
    /// 异常停线查询视图类
    /// </summary>
    internal class AbnormalCauseCriteriaViewConfig : WebViewConfig<AbnormalCauseCriteria>
    {
        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.SourceType).Show(ShowInWhere.All);
                View.Property(p => p.EquipAccountId).Show(ShowInWhere.All);
                View.Property(p => p.ProcessId).Show(ShowInWhere.All);
                View.Property(p => p.ProcessSegmentId).Show(ShowInWhere.All);
                View.Property(p => p.ResourceId).UseDataSource((e, c, r) =>
                {
                    var eq = e as AbnormalCauseCriteria;
                    if (eq == null)
                        return new EntityList<Resource>();
                    return AppRuntime.Service.Resolve<WipResourceController>().GetWipResources(new List<ResourceState>() { ResourceState.Actived }, null, new List<SyncSourceType>() { SyncSourceType.Enterprise }, c, r);
                }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true).Show(ShowInWhere.All)
                .UseListSetting(e => { e.HelpInfo = "显示启用状态为已启用的生产资源"; });
                View.Property(p => p.AlertManageId).Show(ShowInWhere.All);
                View.Property(p => p.AlerterId).Show(ShowInWhere.All);
                View.Property(p => p.ExceptionStopType).Show(ShowInWhere.All);
                View.Property(p => p.BeginDate).Show(ShowInWhere.Detail).UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.Week; });
            }
        }
    }
}
