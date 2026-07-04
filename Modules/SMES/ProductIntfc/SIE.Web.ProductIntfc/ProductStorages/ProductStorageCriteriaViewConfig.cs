using SIE.Domain;
using SIE.ProductIntfc.ProductStorages;
using SIE.Resources.WipResources;
using SIE.Web.ProductIntfc._Extentions_;
using System.Collections.Generic;

namespace SIE.Web.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 报检记录查询实体视图配置
    /// </summary>
    internal class ProductStorageCriteriaViewConfig : WebViewConfig<ProductStorageCriteria>
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
                    var criteria = e as ProductStorageCriteria;
                    if (criteria == null || criteria.Shop == null)
                        return new EntityList<WipResource>();

                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.ShopId.Value, c, r);
                }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true).ShowInDetail()
                .UseListSetting(e => { e.HelpInfo = "显示启用状态不失效的生产资源"; });
                View.Property(p => p.WorkOrder).ShowInDetail();
                View.Property(p => p.Barcode).ShowInDetail();
                View.Property(p => p.State).ShowInDetail();
            }
        }
    }
}
