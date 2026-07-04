using SIE.Domain;
using SIE.ProductIntfc.InspLogs;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.ProductIntfc._Extentions_;
using System.Collections.Generic;

namespace SIE.Web.ProductIntfc.InspLogs
{
    /// <summary>
    /// 报检单查询实体视图配置
    /// </summary>
    internal class InspLogCriteriaViewConfig : WebViewConfig<InspLogCriteria>
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
                View.Property(p => p.Shop).UseShopLookUpEditor(p => p.DisplayField = Enterprise.NameProperty.Name).ShowInDetail()
                .UseListSetting(e => { e.HelpInfo = "显示企业类型为车间的企业资源"; });
                View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                {
                    var criteria = e as InspLogCriteria;
                    if (criteria == null || criteria.Shop == null)
                        return new EntityList<WipResource>();
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.ShopId.Value, c, r);
                }).UsePagingLookUpEditor().ShowInDetail()
                .UseListSetting(e => { e.HelpInfo = "显示启用状态不失效的生产资源"; });
                View.Property(p => p.InspUser).ShowInDetail();
                View.Property(p => p.BatchNo).ShowInDetail();
                View.Property(p => p.InspDate).UseDateRangeEditor(e => { e.DateFormat = "Y/m/d"; e.DateRangeType = ObjectModel.DateRangeType.Month; }).ShowInDetail();
            }
        }
    }
}
