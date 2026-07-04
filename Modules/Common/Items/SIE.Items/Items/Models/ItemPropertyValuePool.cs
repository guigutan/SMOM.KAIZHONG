using SIE.Items.ProductBoms.Models;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Items
{
    /// <summary>
    /// 物料扩展属性池
    /// </summary>
    public class ItemPropertyValuePool
    {
        #region 属性
        /// <summary>
        /// 物料扩展属性 key:物料ID value:物料扩展属性
        /// </summary>
        protected Dictionary<double, List<ItemPropertyInfo>> DicItemPropertyInfo { get; set; }

        /// <summary>
        /// 物料替代控制器
        /// </summary>
        protected ItemController ItemCtrl { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemPropertyValuePool()
        {
            ItemCtrl = RT.Service.Resolve<ItemController>();
        }

        /// <summary>
        /// 数据加载
        /// </summary>
        /// <param name="itemIds">根据物料Id加载扩展属性信息</param>
        public virtual void Load(List<double> itemIds = null)
        {
            List<ItemPropertyInfo> itemPropertyInfos = ItemCtrl.GetPropertyValueListByItemId(itemIds);
            DicItemPropertyInfo = itemPropertyInfos.GroupBy(p => p.RelationId).ToDictionary(p => p.Key, p => p.ToList());
        }

        /// <summary>
        /// 设置物料扩展属性组
        /// </summary>
        /// <param name="itemPropertys">物料扩展属性</param>
        public virtual List<ItemPropertyInfo> SetPropertyGroup(List<ItemPropertyInfo> itemPropertys)
        {
            if (itemPropertys == null)
            {
                itemPropertys = new List<ItemPropertyInfo>();
            }
            foreach (var itemProperty in itemPropertys)
            {
                List<ItemPropertyInfo> tmpPropertyInfos = null;
                if (DicItemPropertyInfo.TryGetValue(itemProperty.RelationId, out tmpPropertyInfos))
                {
                    ItemPropertyInfo tmpProperty = tmpPropertyInfos.FirstOrDefault(p => p.DefinitionId == itemProperty.DefinitionId && p.Value == itemProperty.Value);
                    if (tmpProperty != null)
                    {
                        itemProperty.PropertyGroup = tmpProperty.PropertyGroup;
                    }
                }
            }

            itemPropertys = itemPropertys.OrderBy(p => p.PropertyGroup).ThenBy(p => p.DefinitionId).ThenBy(p => p.Value).ToList();

            return itemPropertys;
        }
    }
}