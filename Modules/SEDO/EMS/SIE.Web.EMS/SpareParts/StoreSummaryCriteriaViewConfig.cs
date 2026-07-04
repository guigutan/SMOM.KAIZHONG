using SIE.EMS.SpareParts;
using SIE.Items;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.DevicePurs
{
    /// <summary>
    /// 备件库存查询实体视图配置
    /// </summary>
    internal class StoreSummaryCriteriaViewConfig : WebViewConfig<StoreSummaryCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.SparePartId).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<SparePartController>().GetSpareParts(p, k);
            }).UsePagingLookUpEditor((m, r) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(r.SparePartName), nameof(r.SparePart.SparePartName));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.SparePartName);
            View.Property(p => p.OrderNumber);
            View.Property(p => p.ItemCategory).UseDataSource((e, p, k) =>
            {
                var list = RT.Service.Resolve<ItemController>().GetItemCategoryByItemType(ItemType.SparePart, SIE.Items.Items.CategoryType.Item, k, p);
                var itemIds = list.Select(p => p.Id).ToList();
                foreach (var item in list)
                {
                    if (!itemIds.Contains(item.TreePId ?? 0))
                    {
                        item.TreePId = null;
                    }
                }
                return list;
            });
        }
    }
}
