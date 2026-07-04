using SIE.Domain;
using SIE.EventMessages.Common.Traces;
using SIE.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Items
{
    /// <summary>
    /// 单位控件器
    /// </summary>
    public class ItemUnitController : DomainController
    {
        /// <summary>
        /// 获取物料ID获取单位精度
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <returns></returns>
        public virtual UnitsModel GetItemUnitPrecision(double itemId)
        {
            UnitsModel unitModel;
            var itemModel = RF.GetById<Item>(itemId, new EagerLoadOptions().LoadWithViewProperty());
            if (itemModel != null)
            {
                unitModel = new UnitsModel()
                {
                    unitPrecsion = itemModel.UnitPrecision != null ? itemModel.UnitPrecision.Value : 3,
                    carry = (int)itemModel.UnitTradeType,
                };
            }
            else
            {
                unitModel = new UnitsModel()
                {
                    unitPrecsion = 3,
                    carry = 0,//四舍五入
                };
            }
            return unitModel;
        }

        /// <summary>
        /// 获取物料IDS获取单位精度
        /// </summary>
        /// <param name="secondUnitIds"></param>
        /// <returns></returns>
        public virtual List<ItemUnitsModel> GetItemUnitPrecision(List<double> secondUnitIds)
        {
            List<ItemUnitsModel> itemUnitsModels = new List<ItemUnitsModel>();
            var secondUnitList = secondUnitIds.SplitContains(tempIds =>
            {
                return Query<Unit>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            secondUnitList.ForEach(item =>
            {
                var itemUnitModel = new ItemUnitsModel
                {
                    SecondUnitId = item.Id,
                    unitPrecsion = item.Precision != null ? item.Precision.Value : 3,
                    carry = (int)item.TradeType,
                };
                itemUnitsModels.Add(itemUnitModel);
            });
            return itemUnitsModels;
        }

        /// <summary>
        /// 获取物料ID获取单位精度
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <returns></returns>
        public virtual ItemUnitPrecsionInfo GetItemPrecision(double itemId)
        {
            ItemUnitPrecsionInfo unitModel;
            var itemModel = RF.GetById<Item>(itemId, new EagerLoadOptions().LoadWithViewProperty());
            if (itemModel != null)
            {
                unitModel = new ItemUnitPrecsionInfo()
                {
                    unitPrecsion = itemModel.UnitPrecision != null ? itemModel.UnitPrecision.Value : 3,
                    carry = (int)itemModel.UnitTradeType,
                };
            }
            else
            {
                unitModel = new ItemUnitPrecsionInfo()
                {
                    unitPrecsion = 3,
                    carry = 0,//四舍五入
                };
            }
            return unitModel;
        }

        /// <summary>
        /// 获取单位ID获取单位精度
        /// </summary>
        /// <param name="itemUnitId">物料ID</param>
        /// <returns></returns>
        public virtual UnitsModel GetUnitPrecision(double itemUnitId)
        {
            UnitsModel unitModel;

            var itemModel = RF.GetById<Unit>(itemUnitId);
            if (itemModel != null)
            {
                unitModel = new UnitsModel()
                {
                    unitPrecsion = itemModel.Precision != null ? itemModel.Precision.Value : 3,
                    carry = (int)itemModel.TradeType,
                };
            }
            else
            {
                unitModel = new UnitsModel()
                {
                    unitPrecsion = 3,
                    carry = 0,//四舍五入
                };
            }
            return unitModel;
        }
        /// <summary>
        /// 返回通过物料单位转换后的值
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="number">值</param>
        /// <returns></returns>
        public virtual double SetItemPrecisionValue(double itemId, double number)
        {
            UnitsModel unitsModel = GetItemUnitPrecision(itemId);
            return RoundToDecimalPlaces(number, (int)unitsModel.unitPrecsion, unitsModel.carry);
        }

        /// <summary>
        /// 计算单位转化后的值
        /// </summary>
        /// <param name="number"></param>
        /// <param name="decimalPlaces"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual double CalculateRoundToDecimalPlaces(double number, int decimalPlaces, double type)
        {
            return RoundToDecimalPlaces(number, decimalPlaces, type);
        }

        /// <summary>
        /// 进位处理
        /// </summary>
        /// <param name="number">值</param>
        /// <param name="decimalPlaces">精度</param>
        /// <param name="type">进位类型（0代表四舍五入; 1代表舍位; 2代表进位）</param>
        /// <returns></returns>

        static double RoundToDecimalPlaces(double number, int decimalPlaces, double type)
        {
            double multiplier = Math.Pow(10, decimalPlaces);
            double numberDecimal = 0;
            switch (type)
            {
                case (double)CarryType.Round:
                    numberDecimal = Math.Round(number * multiplier) / multiplier;
                    break;

                case (double)CarryType.Floor:
                    numberDecimal = Math.Floor(number * multiplier) / multiplier;
                    break;

                case (double)CarryType.Ceiling:
                    numberDecimal = Math.Ceiling(number * multiplier) / multiplier;
                    break;
            }
            return numberDecimal;
        }

        /// <summary>
        /// 转换按取舍后的数量
        /// </summary>
        /// <param name="qty">数量</param>
        /// <param name="precision">精度</param>
        /// <param name="tradeType">取舍类型</param>
        /// <returns>取舍后的数量</returns>
        public virtual decimal TrancateTradeQty(decimal qty, int? precision, TradeType? tradeType)
        {
            if (qty == 0)
                return qty;
            if (precision == null)
                precision = 3;
            if (tradeType == null)
                tradeType = TradeType.HalfAdjust;
            int ra = (int)(Math.Pow(10, precision.Value));
            if (tradeType == TradeType.Rounding)
                return Math.Floor(qty * ra) / ra;
            else if (tradeType == TradeType.Carry)
                return Math.Ceiling(qty * ra) / ra;
            else
                return Math.Round(qty, precision.Value);
        }

        /// <summary>
        /// 通过物料获取转换后的值
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual decimal TrancateTradeQtyByItem(decimal qty,double itemId)
        {

            if (qty == 0)
                return qty;
            TradeType tradeType = TradeType.HalfAdjust;
            UnitsModel unitsModel = GetItemUnitPrecision(itemId);
            tradeType = (TradeType)unitsModel.carry;
            int ra = (int)(Math.Pow(10, unitsModel.unitPrecsion));
            if (tradeType == TradeType.Rounding)
                return Math.Floor(qty * ra) / ra;
            else if (tradeType == TradeType.Carry)
                return Math.Ceiling(qty * ra) / ra;
            else
                return Math.Round(qty, Convert.ToInt32(unitsModel.unitPrecsion));
        }

        /// <summary>
        /// 检查默认值精度与单位精度值是否一致
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="number">值</param>
        /// <returns></returns>
        public virtual bool CheckIsDefaultValue(double itemId, double number)
        {
            //判断默认值的小数位是否大于单位精度小数位
            UnitsModel unitsModel = GetItemUnitPrecision(itemId);
            string nameAsString = number.ToString();
            int decimalPlaces = 0;
            if (nameAsString.Contains("."))
            {
                decimalPlaces = nameAsString.Length - nameAsString.IndexOf(".") - 1;
            }
            if (decimalPlaces <= unitsModel.unitPrecsion)
                return true;
            return false;
        }

        /// <summary>
        /// 检查是否辅助单位
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public virtual bool CheckIsDefaultUnit(double itemId, double unitId)
        {
            var item = RF.GetById<Item>(itemId);

            return unitId == item?.SecondUnitId;
        }

        /// <summary>
        /// 检查是否辅助单位
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public virtual bool CheckIsDefaultItemUnit(double itemId, double unitId)
        {
            var item = RF.GetById<Item>(itemId);
            var isDefault = true;
            if (item != null && unitId != item?.SecondUnitId)
            {
                item.SecondUnitId = null;
                RF.Save(item);
                isDefault = false;
            }
            return isDefault;
        }

        /// <summary>
        /// 修改物料辅助单位
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <returns></returns>
        public virtual void UpdateSecondUnitId(double? itemId)
        {
            var item = RF.GetById<Item>(itemId);
            if (item != null)
            {
                item.SecondUnitId = null;
                RF.Save(item);
            }
        }

        /// <summary>
        /// 物料单位
        /// </summary>
        /// <param name="unitIds">单位</param>
        /// <returns></returns>
        public virtual EntityList<ItemUnit> GetItemUnits(List<double> unitIds)
        {
            unitIds = unitIds.Distinct().ToList();
            return Query<ItemUnit>().Where(p => unitIds.Contains(p.UnitId)).ToList();
        }

        /// <summary>
        /// 获取所有的物料单位
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ItemUnit> GetItemUnits()
        {
            return Query<ItemUnit>().ToList(new PagingInfo() { PageNumber = 1, PageSize = int.MaxValue - 1 });
        }

        /// <summary>
        /// 物料单位
        /// </summary>
        /// <param name="itemUnit">单位转换</param>
        /// <returns></returns>
        public virtual bool GetItemUnits(ItemUnit itemUnit)
        {
            var isData = true;
            var itemUnits = Query<ItemUnit>().Where(p => p.MainUnitId == itemUnit.MainUnitId && p.UnitId == itemUnit.UnitId && p.IsBaseUnit).ToList();
            if (!itemUnits.Any())
            {
                var itemUnitList = Query<ItemUnit>().Where(p => p.MainUnitId == itemUnit.MainUnitId && p.UnitId == itemUnit.UnitId && !p.IsBaseUnit && p.ItemId == itemUnit.ItemId && p.Id != itemUnit.Id).ToList();
                if (!itemUnitList.Any())
                {
                    isData = false;
                }
            }
            return isData;
        }

        /// <summary>
        /// 物料设置辅助单位，更新单位转换
        /// </summary>
        /// <param name="itemId">物料</param>
        /// <param name="unitId">单位</param>
        public virtual void SetDefaultUnit(double itemId, double? unitId)
        {
            var unitChanges = GetUnitChanges(itemId);
            var defChange = unitChanges.FirstOrDefault(p => p.IsDefault);
            if (defChange != null)
            {
                if (defChange.UnitId != unitId)
                {
                    defChange.IsDefault = false;
                    if (unitId.HasValue)
                    {
                        var unitChange = unitChanges.FirstOrDefault(p => p.UnitId == unitId);
                        if (unitChange != null)
                        {
                            unitChange.IsDefault = true;
                        }
                    }
                    RF.Save(unitChanges);
                }
            }
            else
            {
                var itemUnit = unitChanges.FirstOrDefault(p => p.UnitId == unitId);
                if (itemUnit != null)
                {
                    itemUnit.IsDefault = true;
                    RF.Save(itemUnit);
                }

            }
        }

        /// <summary>
        /// 物料设置辅助单位，更新单位转换
        /// </summary>
        /// <param name="itemId">物料</param>
        /// <param name="unitId">单位</param>
        public virtual bool UpdateDefaultItemUnit(double itemId, double unitId)
        {
            if (CheckIsDefaultUnit(itemId, unitId))
            {
                DB.Update<ItemUnit>().Set(p => p.IsDefault, true).Where(p => p.ItemId == itemId && p.UnitId == unitId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 物料设置辅助单位，更新单位转换
        /// </summary>
        /// <param name="itemId">物料</param>
        /// <param name="unitId">单位</param>
        public virtual void UpdateDefaultUnit(double itemId, double unitId)
        {
            using (var tran = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
            {
                UpdateItemUnit(itemId, unitId);
                tran.Complete();
            }
        }

        /// <summary>
        /// 物料设置辅助单位，更新单位转换
        /// </summary>
        /// <param name="id">单位转换ID</param>
        public virtual void UpdateDefaultUnit(double id)
        {
            using (var tran = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
            {
                var itemUnit = RF.GetById<ItemUnit>(id);
                UpdateItemUnit(itemUnit.ItemId.Value, itemUnit.UnitId);
                DB.Update<ItemUnit>().Set(p => p.IsDefault, true).Where(p => p.ItemId == itemUnit.ItemId && p.UnitId == itemUnit.UnitId).Execute();
                tran.Complete();
            }
        }

        /// <summary>
        /// 物料设置辅助单位，更新单位转换
        /// </summary>
        /// <param name="itemId">物料</param>
        /// <param name="unitId">单位</param>
        public virtual void UpdateItemUnit(double itemId, double unitId)
        {
            var defChange = GetDefaultItemUnit(itemId);
            if (defChange != null && defChange.UnitId != unitId)
            {
                DB.Update<ItemUnit>().Set(p => p.IsDefault, false).Where(p => p.ItemId == itemId && p.UnitId == defChange.UnitId).Execute();
            }
            DB.Update<Item>().Set(p => p.SecondUnitId, unitId).Where(p => p.Id == itemId).Execute();
        }

        /// <summary>
        /// 获取转换单位
        /// </summary>
        /// <param name="itemId">单位</param>
        /// <returns>单位</returns>
        public virtual EntityList<ItemUnit> GetUnitChanges(double itemId)
        {
            return Query<ItemUnit>().Where(p => p.ItemId == itemId).ToList();
        }

        /// <summary>
        /// 获取物料默认辅助单位
        /// </summary>
        /// <param name="itemId">单位</param>
        /// <returns>默认辅助单位</returns>
        public virtual ItemUnit GetDefaultItemUnit(double itemId)
        {
            return Query<ItemUnit>().Where(p => p.ItemId == itemId && p.IsDefault).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取物料默认辅助单位或基准单位
        /// </summary>
        /// <param name="itemId">单位</param>
        /// <returns>默认辅助单位</returns>
        public virtual EntityList<ItemUnit> GetDefaultAndBaseItemUnit(double itemId)
        {
            return Query<ItemUnit>().Where(p => p.ItemId == itemId && p.IsDefault || p.IsBaseUnit).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取辅助单位
        /// </summary>
        /// <param name="itemId">物料</param>
        /// <param name="secondUnitId">辅助单位</param>
        /// <param name="mainUnitId">主单位</param>
        /// <returns></returns>
        public virtual ItemUnit GetSecondUnit(double itemId, double mainUnitId, double secondUnitId)
        {
            return Query<ItemUnit>().Where(p => (p.ItemId == itemId || p.IsBaseUnit) && p.MainUnitId == mainUnitId && p.UnitId == secondUnitId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

        }

        /// <summary>
        /// 获取单位
        /// </summary>
        /// <param name="itemId">物料</param>
        /// <param name="keyWrod">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="itemMainUnitId"></param>
        /// <param name="isFromBill">来源单据选择</param>
        /// <returns></returns>
        public virtual EntityList<Unit> GetSecondUnits(double itemId, string keyWrod, PagingInfo pagingInfo, double itemMainUnitId, bool isFromBill = true)
        {
            var query = Query<Unit>().Join<ItemUnit>((x, y) => y.UnitId == x.Id && (y.ItemId == itemId || y.IsBaseUnit && y.MainUnitId == itemMainUnitId));

            if (keyWrod.IsNotEmpty())
            {
                if (!keyWrod.Contains("%"))
                    keyWrod = "%" + keyWrod + "%";
                query.Where(p => p.Name.Contains(keyWrod));
            }
            var rst = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            if (rst.Count == 0 && isFromBill)
            {
                var defUnit = Query<Unit>().Join<Item>((x, y) => y.UnitId == x.Id && y.Id == itemId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
                rst.Add(defUnit);
                rst.SetTotalCount(1);
            }
            return rst;
        }

        /// <summary>
        /// 获取默认单位
        /// </summary>
        /// <param name="itemIds">物料</param>
        /// <returns>默认单位</returns>
        public virtual EntityList<ItemUnit> GetDefaultItemUnits(List<double> itemIds)
        {
            List<double?> itemid = new List<double?>();
            itemIds.Distinct().ForEach(p =>
            {
                itemid.Add(p);
            });
            return itemid.SplitContains(ids =>
             {
                 return Query<ItemUnit>().Where(p => ids.Contains(p.ItemId) && p.IsDefault || p.IsBaseUnit).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
             });
        }

        /// <summary>
        /// 获取默认单位
        /// </summary>
        /// <param name="itemIds">物料</param>
        /// <returns>默认单位</returns>
        public virtual EntityList<ItemUnit> GetAllItemUnits(List<double> itemIds)
        {
            List<double?> itemid = new List<double?>();
            //var secondUnitList = secondUnitIds.SplitContains(tempIds =>
            //{
            //    return Query<Unit>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //});
            itemIds.ForEach(p =>
            {
                itemid.Add(p);
            });
            return itemid.SplitContains(tempIds =>
            {
                return Query<ItemUnit>().Where(p => tempIds.Contains(p.ItemId) || p.IsBaseUnit).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

        }


    }
}
