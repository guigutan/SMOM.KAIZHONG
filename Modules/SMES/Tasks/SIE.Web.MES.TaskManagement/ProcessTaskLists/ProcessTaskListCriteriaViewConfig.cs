using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.ProcessTaskLists
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessTaskListCriteriaViewConfig : WebViewConfig<ProcessTaskListCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.Detail);

                View.Property(p => p.Product).Show(ShowInWhere.Detail);
                View.Property(p => p.Process).Show(ShowInWhere.Detail);
                View.Property(p => p.FactoryId).UseFactoryEditor().Show(ShowInWhere.Detail);
                View.Property(p => p.WorkShop).UseResourceWorkShopEditor().Cascade(p => p.Resource, null).Show(ShowInWhere.Detail)
                     .UseListSetting(e => { e.HelpInfo = "更改车间清空资源"; });
                View.Property(p => p.Resource).Show(ShowInWhere.Detail).UseDataSource((e, c, r) =>
                {
                    var criteria = e as ProcessTaskListCriteria;
                    if (criteria == null || criteria.WorkShop == null)
                        return new EntityList<WipResource>();

                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.WorkShopId.Value, srcTypeList, c, r);
                }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true)
                .UseListSetting(e => { e.HelpInfo = "显示启用状态不失效且来源类型为企业模型的生产资源"; });
                View.Property(p => p.PlanBeginTime).Show(ShowInWhere.Detail).UseDateRangeEditor(p =>
                {
                    p.Format = "Y/m/d";
                    p.DateRangeType = ObjectModel.DateRangeType.Week;
                });
            }
        }
    }
}
