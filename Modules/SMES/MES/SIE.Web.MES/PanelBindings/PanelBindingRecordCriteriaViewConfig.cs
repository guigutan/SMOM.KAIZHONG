using SIE.Domain;
using SIE.MES.PanelBindings;
using SIE.Resources.WipResources;
using System.Collections.Generic;

namespace SIE.Web.MES.PanelBindings
{
    /// <summary>
    /// MES工单条码绑定记录查询实体-界面
    /// </summary>
    internal class PanelBindingRecordCriteriaViewConfig : WebViewConfig<PanelBindingRecordCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.Product).HasLabel("产品名称").Show();
                View.Property(p => p.WorkShop).UseResourceWorkShopEditor().Cascade(p => p.Resource, null)
                    .UseListSetting(e => e.HelpInfo = "更改车间清空资源").Show();
                View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                {
                    var criteria = e as PanelBindingRecordCriteria;
                    if (criteria == null || criteria.WorkShop == null)
                        return new EntityList<WipResource>();
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.WorkShopId.Value, srcTypeList, c, r);
                }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true)
                    .UseListSetting(e => e.HelpInfo = "显示车间下面的资源").Show();
                View.Property(p => p.PlanBeginDate).UseDateRangeEditor().Show();
            }
        }
    }
}
