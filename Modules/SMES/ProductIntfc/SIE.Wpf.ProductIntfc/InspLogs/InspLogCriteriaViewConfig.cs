using SIE.Domain;
using SIE.ProductIntfc.InspLogs;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Wpf.Resources;
using System.Collections.Generic;

namespace SIE.Wpf.ProductIntfc.InspLogs
{
    /// <summary>
    /// 报检单查询实体视图配置
    /// </summary>
    internal class InspLogCriteriaViewConfig : WPFViewConfig<InspLogCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.InspNo).ShowInDetail();
                View.Property(p => p.WoNo).ShowInDetail();
                View.Property(p => p.InspType).ShowInDetail();
                View.Property(p => p.ProductCode).ShowInDetail();
                View.Property(p => p.ProductName).ShowInDetail();
                View.Property(p => p.Shop).UseShopEditor(e =>
                {
                    e.ReloadDataOnPopping = true;
                    e.DisplayMember = nameof(Enterprise.Name);
                }).ShowInDetail();
                View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                {
                    var criteria = e as InspLogCriteria;
                    if (criteria == null || criteria.Shop == null)
                        return new EntityList<WipResource>();
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.ShopId.Value, c, r);
                }).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; }).ShowInDetail();
                View.Property(p => p.InspUser).ShowInDetail();
                View.Property(p => p.BatchNo).ShowInDetail();
                View.Property(p => p.InspDate).UseDateRangeEditor(e => e.DateTimePart = ObjectModel.DateTimePart.Date).ShowInDetail();
            }
        }
    }
}