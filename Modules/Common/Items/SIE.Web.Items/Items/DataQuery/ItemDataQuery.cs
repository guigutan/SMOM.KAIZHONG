using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using SIE.Web.Data;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SIE.Web.Items.Items.DataQuery
{
    /// <summary>
    /// 产品等级查询器
    /// </summary>
    public class ItemDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取产品等级
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>产品等级</returns>
        public EntityList<ProductGrade> GetProductGrades(ProductGradeCriteria criteria, double itemId)
        {
            return RT.Service.Resolve<ItemController>().GetProductGrades(criteria, itemId);
        }

        /// <summary>
        /// 获取快码名称
        /// </summary>
        /// <param name="catalogTypeId"></param>
        /// <param name="catalogCode"></param>
        /// <returns></returns>
        public string GetCatalogCode(double catalogTypeId, string catalogCode)
        {
            var catalogs = RT.Service.Resolve<CatalogController>().GetCatalogList(catalogTypeId);
            return catalogs.FirstOrDefault(p => p.Code == catalogCode)?.Name;
        }

        /// <summary>
        /// 获取默认转换单位
        /// </summary>
        /// <param name="itemId">物料</param>
        /// <returns>单位</returns>
        public ItemUnitData GetDefaultItemUnit(double itemId)
        {
            if (itemId > 0)
            {
                var itemunits = RT.Service.Resolve<ItemUnitController>().GetDefaultAndBaseItemUnit(itemId);
                if (itemunits.Count == 0)
                {//没有任何数据，返回物料的主单位
                    var item = RF.GetById<Item>(itemId, new EagerLoadOptions().LoadWith(Item.UnitProperty));

                    return new ItemUnitData() { ItemId = itemId, UnitId = item.UnitId.Value, UnitName = item.Unit.Name, Trade = item.Unit.TradeType, Precision = item.Unit.Precision };
                }
                else
                {//有维护数据，而且有默认单位，则返回默认单位
                    var defItemUnit = itemunits.FirstOrDefault(a => a.ItemId == itemId);
                    if (defItemUnit != null)
                        return new ItemUnitData
                        {
                            ItemId = defItemUnit.ItemId.Value,
                            Precision = defItemUnit.SecondUnitPrecision,
                            UnitId = defItemUnit.UnitId,
                            Trade = defItemUnit.SecondTrade,
                            UnitName = defItemUnit.UnitName,
                        };
                    else
                    {//有数据，但是没有默认单位。看下物料是否选择了 基准单位作为默认单位
                        var item = RF.GetById<Item>(itemId, new EagerLoadOptions().LoadWith(Item.UnitProperty));
                        if (item.SecondUnitId > 0)
                        {
                            var baseItem = itemunits.FirstOrDefault(a => a.MainUnitId == item.UnitId.Value && a.UnitId == item.SecondUnitId && a.IsBaseUnit);
                            if (baseItem != null)
                                return new ItemUnitData
                                {
                                    ItemId = itemId,
                                    UnitName = baseItem.UnitName,
                                    Trade = baseItem.SecondTrade,
                                    Precision = baseItem.SecondUnitPrecision,
                                    UnitId = baseItem.UnitId,
                                };
                        }
                        return new ItemUnitData() { ItemId = itemId, UnitId = item.UnitId.Value, UnitName = item.Unit.Name, Trade = item.Unit.TradeType, Precision = item.Unit.Precision };

                    }
                }
            }
            return null;
        }

        [Serializable]
        public class ItemUnitData
        {
            public double ItemId { get; set; }

            public double UnitId { get; set; }

            public string UnitName { get; set; }

            public TradeType Trade { get; set; }

            public int? Precision { get; set; }
        }

        /// <summary>
        /// 获取辅助单位数据
        /// </summary>
        /// <param name="itemId">物料</param>
        /// <param name="unitId">单位</param>
        /// <returns>辅助单位</returns>
        public ItemUnit GetSecondUnit(double itemId, double itemUnitId, double unitId)
        {
            var rst = RT.Service.Resolve<ItemUnitController>().GetSecondUnit(itemId, itemUnitId, unitId);
            return rst;
        }

        /// <summary>
        /// 检查是否默认单位
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public bool CheckIsDefaultUnit(double itemId, double unitId)
        {
            return RT.Service.Resolve<ItemUnitController>().CheckIsDefaultItemUnit(itemId, unitId);
        }

        /// <summary>
        /// 获取默认辅助单位
        /// </summary>
        /// <param name="itemIds">物料</param>
        /// <returns></returns>
        public object GetDefaultItemUnits(List<double> itemIds)
        {
            return RT.Service.Resolve<ItemUnitController>().GetDefaultItemUnits(itemIds);
        }

    }
}
