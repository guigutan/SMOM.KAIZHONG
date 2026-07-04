using SIE.Domain;
using SIE.Items;
using SIE.Items.ProductBoms;
using SIE.Web.Data;

namespace SIE.Web.Items.ViewModels
{
    /// <summary>
    /// 产品属性值查询
    /// </summary>
    public class PropertyValueDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取物料属性值
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="definitionId">物料属性定义ID</param>
        /// <returns>物料属性值</returns>
        public EntityList<ItemPropertyValue> GetBomValueProperty(double itemId, double definitionId)
        {
            var bomValueList = RT.Service.Resolve<ItemController>().GetItemPropertyValues(itemId, definitionId);
            return bomValueList;
        }

        /// <summary>
        /// 获取产品BOM明细属性值（该方法中的物料扩展属性对象先不更改，如果引用该方法的js控件确认可以删除，就直接删除控件和该方法）
        /// </summary>
        /// <param name="bomDetailId">产品BOM明细ID</param>
        /// <returns>产品BOM明细属性值</returns>
        public EntityList<ProductBomDetailPropertyValue> GetItemPropertyProperty(double bomDetailId)
        {
            return RT.Service.Resolve<ProductBomController>().GetProductBomDetailPropertyValues(bomDetailId);
        }

        /// <summary>
        /// 判断该物料是否有配置属性
        /// </summary>
        /// <param name="itemid">物料ID</param>
        /// <returns>是否</returns>
        public bool IsExistProperty(double itemid)
        {
            return RT.Service.Resolve<ItemController>().GetItemPropertys(itemid).Count <= 0;
        }
    }
}
