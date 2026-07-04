using SIE.Domain;
using SIE.ProductIntfc.ProductStorages;
using SIE.Resources.WipResources;
using SIE.Wpf.Resources;
using System.Collections.Generic;

namespace SIE.Wpf.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 报检记录查询实体视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class ProductStorageCriteriaViewConfig : WPFViewConfig<ProductStorageCriteria>
    {
        #region 清除资源 ClearResource
        /// <summary>
        /// 清除资源
        /// </summary> 
        public static readonly Property<bool> ClearResourceProperty = P<ProductStorageCriteria>.RegisterExtensionReadOnly("ClearResource", typeof(ProductStorageCriteriaViewConfig),
            GetClearResource, ProductStorageCriteria.ShopIdProperty);

        /// <summary>
        /// 清除资源
        /// </summary>
        /// <param name="me">首件报检查询实体</param>
        /// <returns>bool</returns>
        public static bool GetClearResource(ProductStorageCriteria me)
        {
            me.Resource = null;
            return false;
        }
        #endregion

        /// <summary>
        /// 成品报检查询视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Shop).UseShopEditor(e => e.ReloadDataOnPopping = true).ShowInDetail();
                View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                {
                    var criteria = e as ProductStorageCriteria;
                    if (criteria == null || criteria.Shop == null)
                        return new EntityList<WipResource>();

                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.ShopId.Value, c, r);
                }).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; }).ShowInDetail().Readonly(ClearResourceProperty);
                View.Property(p => p.WorkOrder).ShowInDetail();
                View.Property(p => p.Barcode).ShowInDetail();
                View.Property(p => p.State).ShowInDetail();
            }
        }
    }
}