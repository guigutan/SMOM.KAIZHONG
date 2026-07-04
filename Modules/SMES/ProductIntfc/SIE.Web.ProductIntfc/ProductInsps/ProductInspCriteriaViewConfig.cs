using SIE.Domain;
using SIE.ProductIntfc.ProductInsps;
using SIE.Resources.WipResources;
using SIE.Web.ProductIntfc._Extentions_;
using System.Collections.Generic;

namespace SIE.Web.ProductIntfc.ProductInsps
{
    /// <summary>
    /// 报检记录查询实体视图配置
    /// </summary>
    internal class ProductInspCriteriaViewConfig : WebViewConfig<ProductInspCriteria>
    {
        /// <summary>
        /// 成品报检查询视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Shop).UseShopLookUpEditor().Cascade(p => p.Resource, null).ShowInDetail()
                    .UseListSetting(e => { e.HelpInfo = "显示企业类型为车间的企业资源,更改车间清空资源"; });
                View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                {
                    var criteria = e as ProductInspCriteria;
                    if (criteria == null || criteria.Shop == null)
                        return new EntityList<WipResource>();
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.ShopId.Value, c, r);
                }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true).ShowInDetail()
                .UseListSetting(e => { e.HelpInfo = "显示当前车间下且启用状态不失效的生产资源"; });
                View.Property(p => p.WorkOrder).ShowInDetail();
                View.Property(p => p.Barcode).ShowInDetail();
                View.Property(p => p.InspNo).HasLabel("成品报检单号").ShowInDetail();
                View.Property(p => p.QmsNo).HasLabel("成品检验单号").ShowInDetail();
            }
        }
    }
}
