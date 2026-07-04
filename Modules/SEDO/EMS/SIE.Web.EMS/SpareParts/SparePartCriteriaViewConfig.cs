using SIE.EMS.SpareParts;
using SIE.Equipments.EquipModels;
using SIE.Items;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.SpareParts
{

    /// <summary>
    /// 备件入库查询实体视图配置
    /// </summary>
    internal class SparePartCriteriaViewConfig : WebViewConfig<SparePartCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.SparePartCode);
            View.Property(p => p.SparePartName);
            View.Property(p => p.SpartType);
            View.Property(p => p.SpartEquipModel).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EquipModelController>().GetEquipModels(pagingInfo, keyword);
            });
            View.Property(p => p.ItemCategory).UseDataSource((e, p, k) =>
            {
                var list = RT.Service.Resolve<ItemController>().GetItemSmallCategory(SIE.Items.Items.CategoryType.Item, ItemType.SparePart, k, p);
                var itemIds = list.Select(p => p.Id).ToList();
                foreach (var item in list)
                {
                    if (!itemIds.Contains(item.TreePId??0)) 
                    {
                        item.TreePId = null;
                    }
                }
                return list;
            });
            View.Property(p => p.ControlMethod);
            View.Property(p => p.StorageState);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
            {
                p.DateRangeType = ObjectModel.DateRangeType.All;
            }).Show(ShowInWhere.All);
        }
    }
}
