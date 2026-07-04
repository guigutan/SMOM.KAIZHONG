using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items.ProductBoms.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Items.ProductBoms
{
    /// <summary>
    /// 产品BOM池
    /// </summary>
    [Services.Service(FallbackType = typeof(ProductBomPool), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ProductBomPool
    {
        #region 属性
        /// <summary>
        /// key0:物料ID， value:物料对应的BOM明细,元组1:扩展属性,元组2:物料+扩展属性对应的BOM明细
        /// </summary>
        public Dictionary<double, List<Tuple<ItemUniqueInfo, List<ProductBomRelationViewModel>>>> DicProductBomRel { get; set; }

        /// <summary>
        /// 成品ID/半成品ID， 没有上层物料
        /// </summary>
        protected List<ItemUniqueInfo> ProductIds { get; set; }

        /// <summary>
        /// 低阶码字典 key：物料ID，value：物料对应的低阶码,元组1：扩展属性，元组2：物料+扩展属性对应的低阶码
        /// </summary>
        public Dictionary<double, List<Tuple<ItemUniqueInfo, int>>> DicLLC { get; set; }

        /// <summary>
        /// 产品BOM关系控制器
        /// </summary>
        protected ProductBomRelationController ProductBomRelationCtrl { get; set; }

        /// <summary>
        /// 原材料物料Id
        /// </summary>
        private List<double> _MaterialItemIds;

        /// <summary>
        /// key：产品BOM的ID；value：物料ID,产品BOM明细列表
        /// </summary>
        public Dictionary<double, Tuple<double,List<ProductBomRelationViewModel>>> DicBomIdDetailList { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductBomPool()
        {
            DicProductBomRel = new Dictionary<double, List<Tuple<ItemUniqueInfo, List<ProductBomRelationViewModel>>>>();
            this.DicLLC = new Dictionary<double, List<Tuple<ItemUniqueInfo, int>>>();
            this.ProductIds = new List<ItemUniqueInfo>();
            this.DicBomIdDetailList = new Dictionary<double, Tuple<double, List<ProductBomRelationViewModel>>>();
            ProductBomRelationCtrl = RT.Service.Resolve<ProductBomRelationController>();
        }

        /// <summary>
        /// 加载产品BOM数据
        /// </summary>
        /// <param name="itemIds">物料ID信息</param>
        /// <param name="typeList">来源类型</param>
        /// <param name="itemPropertyValuePool">物料扩展属性池</param>
        public void LoadData(List<double> itemIds, List<ItemSourceType> typeList, ItemPropertyValuePool itemPropertyValuePool = null)
        {
            var tmpItemIds = itemIds.Where(p => !DicProductBomRel.ContainsKey(p)).ToList();
            if (tmpItemIds.IsNullOrEmpty())
            {
                return;
            }

            List<ProductBomRelationViewModel> productBomRelList = ProductBomRelationCtrl.GetChildProductBomList(tmpItemIds, typeList);
            if (itemPropertyValuePool == null)
            {
                List<double> relItemIds = productBomRelList.Select(p => p.ItemId).ToList();
                relItemIds.AddRange(productBomRelList.Where(p => p.ChildItemId.HasValue).Select(p => p.ChildItemId.Value).ToList());
                itemPropertyValuePool = new ItemPropertyValuePool();
                itemPropertyValuePool.Load(relItemIds);
            }

            productBomRelList = productBomRelList.Where(p => !DicProductBomRel.ContainsKey(p.ItemId)).ToList();
            LoadData(tmpItemIds, productBomRelList, itemPropertyValuePool);
        }

        /// <summary>
        /// 加载产品BOM数据
        /// </summary>
        /// <param name="itemIds">物料ID信息</param>
        /// <param name="typeList">物料类型</param>
        /// <param name="itemPropertyValuePool">物料扩展属性池</param>
        public void LoadData(List<double> itemIds, List<ItemType> typeList, ItemPropertyValuePool itemPropertyValuePool = null)
        {
            var tmpItemIds = itemIds.Where(p => !DicProductBomRel.ContainsKey(p)).ToList();
            List<ProductBomRelationViewModel> productBomRelList = ProductBomRelationCtrl.GetChildProductBomList(tmpItemIds, typeList);
            if (itemPropertyValuePool == null)
            {
                List<double> relItemIds = productBomRelList.Select(p => p.ItemId).ToList();
                relItemIds.AddRange(productBomRelList.Where(p => p.ChildItemId.HasValue).Select(p => p.ChildItemId.Value).ToList());
                itemPropertyValuePool = new ItemPropertyValuePool();
                itemPropertyValuePool.Load(relItemIds);
            }

            productBomRelList = productBomRelList.Where(p => !DicProductBomRel.ContainsKey(p.ItemId)).ToList();
            LoadData(tmpItemIds, productBomRelList, itemPropertyValuePool);
        }

        /// <summary>
        /// 加载产品BOM数据
        /// </summary>
        /// <param name="itemIds">物料ID信息</param>
        /// <param name="productBomRelList">产品BOM扁平化结构</param>
        /// <param name="itemPropertyValuePool">物料扩展属性池</param>
        protected virtual void LoadData(List<double> itemIds, List<ProductBomRelationViewModel> productBomRelList, ItemPropertyValuePool itemPropertyValuePool)
        {
            // 根据字符串拆分扩展属性列表
            foreach (var productBomRelation in productBomRelList)
            {
                productBomRelation.ItemPropertyInfoList = productBomRelation.ItemExtProp.ItemExtPropToList(productBomRelation.ItemId);
                productBomRelation.ItemPropertyInfoList = itemPropertyValuePool.SetPropertyGroup(productBomRelation.ItemPropertyInfoList);

                if (productBomRelation.ChildItemId.HasValue)
                {
                    productBomRelation.ChildItemPropertyInfoList = productBomRelation.ChildItemExtProp.ItemExtPropToList(productBomRelation.ChildItemId.Value);
                    productBomRelation.ChildItemPropertyInfoList = itemPropertyValuePool.SetPropertyGroup(productBomRelation.ChildItemPropertyInfoList);
                }
            }

            // 按物料分组
            var itemGroupBoms = productBomRelList.GroupBy(p => p.ItemId).ToList();
            foreach (var itemGroupBom in itemGroupBoms)
            {
                List<Tuple<ItemUniqueInfo, List<ProductBomRelationViewModel>>> itemBomList;
                if (!DicProductBomRel.TryGetValue(itemGroupBom.Key, out itemBomList))
                {
                    itemBomList = new List<Tuple<ItemUniqueInfo, List<ProductBomRelationViewModel>>>();
                    DicProductBomRel.Add(itemGroupBom.Key, itemBomList);
                }

                // 按扩展属性分组
                var itemProps = itemGroupBom.GroupBy(p => p.BomId).ToList();
                foreach (var itemProp in itemProps)
                {
                    ProductBomRelationViewModel bomRelation = itemProp.First();
                    List<ProductBomRelationViewModel> itemPropertyInfo = itemProp.ToList();
                    ItemUniqueInfo itemUniqueInfo = new ItemUniqueInfo() { ItemId = bomRelation.ItemId, ItemExtProp = bomRelation.ItemExtProp, ItemExtPropName = bomRelation.ItemExtPropName, ItemPropertyInfoList = bomRelation.ItemPropertyInfoList };
                    Tuple<ItemUniqueInfo, List<ProductBomRelationViewModel>> tupleValue = Tuple.Create<ItemUniqueInfo, List<ProductBomRelationViewModel>>(itemUniqueInfo, itemPropertyInfo);
                    itemBomList.Add(tupleValue);
                }
            }

            // 获取所有子产品+扩展属性
            Dictionary<double, List<List<ItemPropertyInfo>>> childItemInfos = productBomRelList.Where(p => p.ChildItemId.HasValue)
              .GroupBy(p => new { p.ChildItemId.Value, p.ItemExtProp })
              .Select(p => new
              {
                  ItemId = p.First().ChildItemId.Value,
                  ItemPropertyInfoList = p.First().ChildItemPropertyInfoList
              }).GroupBy(p => p.ItemId).ToDictionary(p => p.Key, p => p.Select(a => a.ItemPropertyInfoList).ToList());

            // 获取没有上层物料的物料ID
            var tmpProductIds = productBomRelList.Where(p => !childItemInfos.ContainsKey(p.ItemId) || !childItemInfos[p.ItemId].Any(a => p.ItemPropertyInfoList.IntersectPropertyGroup(a)))
                .GroupBy(p => new { p.ItemId, p.ItemExtProp })
                .Select(p => new ItemUniqueInfo
                {
                    ItemId = p.First().ItemId,
                    ItemExtProp = p.First().ItemExtProp,
                    ItemExtPropName = p.First().ItemExtPropName,
                    ItemPropertyInfoList = p.First().ItemPropertyInfoList
                }).ToList();

            ProductIds.AddRange(tmpProductIds);
            Dictionary<double, bool> dicProductId = ProductIds.Where(p => p.ItemPropertyInfoList.IsNullOrEmpty()).GroupBy(p => p.ItemId).ToDictionary(p => p.Key, p => false);

            ProductIds.AddRange(itemIds.Where(p => !dicProductId.ContainsKey(p)).Select(p => new ItemUniqueInfo { ItemId = p, ItemPropertyInfoList = new List<ItemPropertyInfo>() }));
        }

        /// <summary>
        /// 根据物料ID获取下一层物料信息
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="itemPropertyInfoList">扩展属性列表</param>
        /// <returns>返回下一层物料信息</returns>
        public List<ProductBomRelationViewModel> GetChildBomList(double itemId, List<ItemPropertyInfo> itemPropertyInfoList)
        {
            List<Tuple<ItemUniqueInfo, List<ProductBomRelationViewModel>>> propBomRels = null;
            List<ProductBomRelationViewModel> result = new List<ProductBomRelationViewModel>();
            if (DicProductBomRel.TryGetValue(itemId, out propBomRels))
            {
                foreach (var propBomRel in propBomRels)
                {
                    if (itemPropertyInfoList.IntersectPropertyGroup(propBomRel.Item1.ItemPropertyInfoList))
                    {
                        result = propBomRel.Item2;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取指定半成品对应某个成品的层级
        /// </summary>
        /// <param name="productId">成品ID</param>
        /// <param name="productPropertyInfoList">成品物料扩展属性</param>
        /// <param name="appointItemId">半成品ID</param>
        /// <param name="itemPropertyInfoList">半成品扩展属性</param>
        /// <param name="level">默认1</param>
        /// <returns>返回指定半成品对应某个成品的层级</returns>
        public virtual int GetItemLevel(double productId, List<ItemPropertyInfo> productPropertyInfoList, double appointItemId, List<ItemPropertyInfo> itemPropertyInfoList, int level = 1)
        {
            if (productId == appointItemId)
                return level;

            List<ProductBomRelationViewModel> bomRelations = GetChildBomList(productId, productPropertyInfoList);
            foreach (var bomRel in bomRelations)
            {
                int result = -1;
                if (bomRel.ChildItemId.HasValue)
                    result = GetItemLevel(bomRel.ChildItemId.Value, bomRel.ChildItemPropertyInfoList, appointItemId, itemPropertyInfoList, level + 1);
                if (result > 0)
                    return result;
            }

            return -1;
        }

        /// <summary>
        /// 验证目前是否有迂回的BOM
        /// </summary>
        /// <returns>返回是否有迂回的BOM</returns>
        public bool Validate()
        {
            const bool result = false;
            // 将原材料的物料低阶码设置为255
            _MaterialItemIds = DicProductBomRel.Values.SelectMany(p => p.SelectMany(a => a.Item2))
                .Where(p => p.ChildItemId.HasValue && p.ChildType == ItemType.Material).Select(p => p.ChildItemId.Value).ToList();

            const int level = 0;
            for (int i = 0; i < ProductIds.Count; i++)
            {
                ItemUniqueInfo tmpItem = ProductIds[i];
                SetItemLLC(tmpItem.ItemId, tmpItem.ItemPropertyInfoList, level);
                List<ItemUniqueInfo> topItemIds = new List<ItemUniqueInfo>();
                topItemIds.Add(tmpItem);
                ValidateBomLevel(tmpItem.ItemId, tmpItem.ItemPropertyInfoList, level + 1, topItemIds);
            }

            return result;
        }

        /// <summary>
        /// 验证产品BOM是否有迂回
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="itemPropertyInfoList">物料扩展属性列表</param>
        /// <param name="level">层级</param>
        /// <param name="topItemInfos">上层物料ID</param>
        private void ValidateBomLevel(double itemId, List<ItemPropertyInfo> itemPropertyInfoList, int level, List<ItemUniqueInfo> topItemInfos)
        {
            List<ProductBomRelationViewModel> childBomList = GetChildBomList(itemId, itemPropertyInfoList);
            for (int i = 0; i < childBomList.Count; i++)
            {
                ProductBomRelationViewModel childBom = childBomList[i];
                if (childBom.ChildItemId.HasValue)
                {
                    var currItemId = childBom.ChildItemId.Value;
                    if (IsExistTopItem(currItemId, childBom.ChildItemPropertyInfoList, topItemInfos))
                    {
                        throw new ValidationException("存在迂回的产品BOM数据，相关物料编码为【{0}】！".L10nFormat(childBom.ChildItemCode));
                    }

                    ItemUniqueInfo currItemInfo = SetItemLLC(currItemId, childBom.ChildItemPropertyInfoList, level);
                    List<ItemUniqueInfo> tmpTopItemInfos = new List<ItemUniqueInfo>();
                    tmpTopItemInfos.AddRange(topItemInfos);
                    tmpTopItemInfos.Add(currItemInfo);
                    ValidateBomLevel(currItemId, childBom.ChildItemPropertyInfoList, level + 1, tmpTopItemInfos);
                }
            }
            ValidateAssignedBomLevel(itemId, level, topItemInfos);
        }

        /// <summary>
        /// 验证产品BOM是否有迂回（指定产品BOM）
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="level">层级</param>
        /// <param name="topItemInfos">上层物料ID</param>
        private void ValidateAssignedBomLevel(double itemId, int level, List<ItemUniqueInfo> topItemInfos)
        {
            List<List<ProductBomRelationViewModel>> childBomLists;
            if (DicBomIdDetailList.Values.GroupBy(p => p.Item1).ToDictionary(p => p.Key, p => p.Select(p2 => p2.Item2).ToList())
                .TryGetValue(itemId, out childBomLists)) 
            {
                childBomLists.ForEach(childBomList =>
                {
                    for (int i = 0; i < childBomList.Count; i++)
                    {
                        ProductBomRelationViewModel childBom = childBomList[i];
                        if (childBom.ChildItemId.HasValue)
                        {
                            var currItemId = childBom.ChildItemId.Value;
                            if (IsExistTopItem(currItemId, childBom.ChildItemPropertyInfoList, topItemInfos))
                            {
                                throw new ValidationException("存在迂回的产品BOM数据，相关物料编码为【{0}】！".L10nFormat(childBom.ChildItemCode));
                            }

                            ItemUniqueInfo currItemInfo = SetItemLLC(currItemId, childBom.ChildItemPropertyInfoList, level);
                            List<ItemUniqueInfo> tmpTopItemInfos = new List<ItemUniqueInfo>();
                            tmpTopItemInfos.AddRange(topItemInfos);
                            tmpTopItemInfos.Add(currItemInfo);
                            ValidateBomLevel(currItemId, childBom.ChildItemPropertyInfoList, level + 1, tmpTopItemInfos);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 是否存在上层同样的物料
        /// </summary>
        /// <param name="itemId">当前物料ID</param>
        /// <param name="itemPropertyInfoList">当前物料扩展属性列表</param>
        /// <param name="topItemInfos">上层物料信息</param>
        /// <returns>返回是否存在上层同样的物料</returns>
        private bool IsExistTopItem(double itemId, List<ItemPropertyInfo> itemPropertyInfoList, List<ItemUniqueInfo> topItemInfos)
        {
            bool result = false;
            foreach (var topItem in topItemInfos)
            {
                if (topItem.ItemId == itemId)
                {
                    if (itemPropertyInfoList.IntersectPropertyGroup(topItem.ItemPropertyInfoList))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 设置物料的低阶码
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="itemPropertyInfoList">扩展属性列表</param>
        /// <param name="level">低阶码</param>
        private ItemUniqueInfo SetItemLLC(double itemId, List<ItemPropertyInfo> itemPropertyInfoList, int level)
        {
            List<Tuple<ItemUniqueInfo, int>> itemUniqueInfos = null;
            int newLevel = level;
            if (_MaterialItemIds.Contains(itemId))
            {
                newLevel = 255;
            }

            if (!DicLLC.TryGetValue(itemId, out itemUniqueInfos))
            {
                itemUniqueInfos = new List<Tuple<ItemUniqueInfo, int>>();
                DicLLC.Add(itemId, itemUniqueInfos);
            }

            Tuple<ItemUniqueInfo, int> currData = null;
            foreach (var itemInfo in itemUniqueInfos)
            {
                if (itemPropertyInfoList.IntersectPropertyGroup(itemInfo.Item1.ItemPropertyInfoList))
                {
                    currData = itemInfo;
                    break;
                }
            }

            if (currData == null)
            {
                ItemUniqueInfo itemUniqueInfo = new ItemUniqueInfo();
                itemUniqueInfo.ItemId = itemId;
                itemUniqueInfo.ItemPropertyInfoList = itemPropertyInfoList;
                currData = Tuple.Create<ItemUniqueInfo, int>(itemUniqueInfo, newLevel);
                itemUniqueInfos.Add(currData);
            }
            else if (currData.Item2 < newLevel)
            {
                itemUniqueInfos.Remove(currData);
                currData = Tuple.Create<ItemUniqueInfo, int>(currData.Item1, newLevel);
                itemUniqueInfos.Add(currData);
            }

            return currData.Item1;
        }

        /// <summary>
        /// 初始化产品BOM及明细字典
        /// </summary>
        /// <param name="bomIds">产品BOM的ID列表</param>
        public void InitDicBomIdDetailList(List<double> bomIds)
        {
            bomIds = bomIds.Distinct().ToList();
            if (bomIds == null || !bomIds.Any())
            {
                return;
            }
            var boms = RT.Service.Resolve<ProductBomController>().GetProductBomsByIds(bomIds);

            foreach (var bom in boms)
            {
                List<ProductBomRelationViewModel> bomRels = new List<ProductBomRelationViewModel>();
                if (bom != null)
                {
                    var details = bom.DetailList;
                    List<double> itemIds = new List<double>();
                    foreach (var item in details)
                    {
                        ProductBomRelationViewModel model = new ProductBomRelationViewModel();
                        model.BomId = bom.Id;
                        model.ItemId = bom.ProductId;
                        model.ItemExtProp = bom.ItemExtProp;
                        model.ItemExtPropName = bom.ItemExtPropName;
                        model.ItemPropertyInfoList = model.ItemExtProp.ItemExtPropToList(model.ItemId);
                        model.ChildItemId = item.ItemId;
                        model.ChildItemExtProp = item.ItemExtProp;
                        model.ChildItemExtPropName = item.ItemExtPropName;
                        model.ChildItemPropertyInfoList = model.ChildItemExtProp.ItemExtPropToList(model.ChildItemId.Value);
                        model.UnitQty = item.UnitQty;
                        model.ChildItemCode = item.Item.Code;
                        model.ChildItemSourceType = item.Item.ItemSourceType;
                        model.ProcessSegmentId = item.ProcessSegmentId;
                        model.ChildType = item.Item.Type;
                        model.IsRecoilItem = item.IsRecoilItem;
                        bomRels.Add(model);

                        if(model.ChildItemId != null)
                        {
                            itemIds.Add(model.ChildItemId.Value);
                        }
                    }
                    if (itemIds.Any())
                    {
                        List<ItemSourceType> typeList = new List<ItemSourceType>() { ItemSourceType.SelfMade, ItemSourceType.Outsourcing, ItemSourceType.OutMade };
                        LoadData(itemIds, typeList);
                    }

                    this.DicBomIdDetailList.Add(bom.Id, new Tuple<double, List<ProductBomRelationViewModel>>(bom.ProductId,bomRels));
                }
            }
        }

        /// <summary>
        /// 匹配产品BOM及其明细字典
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        public List<ProductBomRelationViewModel> GetDicBomIdDetailListValue(double bomId)
        {
            Tuple<double,List<ProductBomRelationViewModel>> tuple;
            if (!DicBomIdDetailList.TryGetValue(bomId, out tuple))
            {
                return new List<ProductBomRelationViewModel>();
            }
            return tuple.Item2;
        }
    }
}