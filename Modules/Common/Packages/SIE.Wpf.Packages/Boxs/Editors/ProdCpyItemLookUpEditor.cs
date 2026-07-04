using SIE.Domain;
using SIE.Items;
using SIE.Items.ProductFamilys;
using SIE.ManagedProperty;
using SIE.Packages.Boxs;
using SIE.Wpf.Editors;

namespace SIE.Wpf.Packages.Packages.Editors
{
    /// <summary>s
    /// 产品族
    /// </summary>
    public class ProductFamilyLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 产品族编辑器名称
        /// </summary>
        public const string EditorName = "ProductFamilyLookUpEditor";

        /// <summary>
        /// 重新加载数据源
        /// </summary>
        /// <param name="source">对象</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <param name="titleProperty">托管属性标记</param>
        /// <returns>列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            return RT.Service.Resolve<ProductFamilyController>().GetProductFamily(keyword, pagingInfo);
        }
    }

    /// <summary>
    /// 产品机型编辑器
    /// </summary>
    public class ProductModelLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ProductModelLookUpEditor";

        /// <summary>
        /// 重新加载数据源
        /// </summary>
        /// <param name="source">对象</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <param name="titleProperty">托管属性标记</param>
        /// <returns>列表</returns>
        protected override EntityList GetDataSourceCore(Entity source, PagingInfo pagingInfo, string keyword, IManagedProperty titleProperty)
        {
            var criteria = source as ProdCpyItemCriteria;
            if (criteria.ProductFamilyId == null) return new EntityList<ProductModel>();
            return RT.Service.Resolve<ItemController>().GetProductModelsByFamily(criteria.ProductFamilyId.Value, keyword, pagingInfo);
        }
    }
}
