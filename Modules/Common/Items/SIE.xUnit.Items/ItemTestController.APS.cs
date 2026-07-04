using SIE.Common;
using SIE.Domain;
using SIE.Items;
using SIE.Resources.ProcessSegments;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.xUnit.Items
{
    /// <summary>
    /// APS 物料 控制器
    /// </summary>
    public partial class ItemTestController
    {
        /// <summary>
        /// 创建单位
        /// </summary>
        /// <returns>单位</returns>
        public virtual EntityList<Unit> CreateAPSUnits()
        {
            List<string> unitCodes = new List<string>() { };
            for (int i = 0; i < 10; i++)
            {
                unitCodes.Add("APSUnit" + i);
            }

            EntityList<Unit> units = Query<Unit>().Where(p => unitCodes.Contains(p.Code)).ToList();
            if (units.Count != 10)
            {
                foreach (string unitCode in unitCodes.Where(p => !units.Any(q => q.Code == p)))
                {
                    var unitTypes = GetUnitTypes();
                    Unit unit = new Unit();
                    unit.GenerateId();
                    unit.Code = unitCode;
                    unit.Name = unitCode;
                    unit.Type = unitTypes.Count > 0 ? unitTypes[0] : "";
                    unit.Precision = 1;
                    units.Add(unit);
                }
            }

            return units;
        }

        /// <summary>
        /// 创建产品机型
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ProductModel> CreateAPSProductModel()
        {
            List<string> proModelCodes = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                proModelCodes.Add("APSProductModel" + i);
            }

            EntityList<ProductModel> productModels = Query<ProductModel>().Where(p => proModelCodes.Contains(p.Code)).ToList();
            if (productModels.Count != 10)
            {
                foreach (string proModelCode in proModelCodes.Where(p => !productModels.Any(q => q.Code == p)))
                {
                    ProductModel productModel = new ProductModel();
                    productModel.GenerateId();
                    productModel.Code = proModelCode;
                    productModel.Name = proModelCode;
                    productModel.WorkingHours = 1;
                    productModel.SendingHours = 1;
                    productModels.Add(productModel);
                }
            }

            return productModels;
        }

        /// <summary>
        /// 创建物料
        /// </summary>
        /// <param name="units">单位</param>
        /// <param name="productModels">产品机型</param>
        /// <returns>物料</returns>
        public virtual EntityList<Item> CreateAPSItem(EntityList<Unit> units, EntityList<ProductModel> productModels)
        {
            List<string> productCode = new List<string>();
            // 创建20个成品
            for (int i = 0; i < 20; i++)
            {
                string itemCode = "APS_Item" + i.ToString().PadLeft(2, '0');
                productCode.Add(itemCode);
            }

            // 创建60个半成品
            List<string> semiFinishedCode = new List<string>();
            for (int i = 0; i < productCode.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    string itemCode = productCode[i] + "_Child" + j.ToString().PadLeft(3, '0');
                    semiFinishedCode.Add(itemCode);
                }
            }

            // 创建180个原材料
            List<string> materialCode = new List<string>();
            for (int i = 0; i < semiFinishedCode.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    string itemCode = semiFinishedCode[i] + "_" + j.ToString().PadLeft(3, '0');
                    materialCode.Add(itemCode);
                }
            }

            List<string> itemCodes = new List<string>();
            itemCodes.AddRange(productCode);
            itemCodes.AddRange(semiFinishedCode);
            itemCodes.AddRange(materialCode);

            EntityList<Item> itemList = GetAPSItem(itemCodes);
            Random rd = new Random();
            int productLen = productCode.First().Length;
            int semiFinishedLen = semiFinishedCode.First().Length;
            if (itemList.Count != itemCodes.Count)
            {
                foreach (var itemCode in itemCodes.Where(p => !itemList.Any(q => q.Code == p)))
                {
                    ItemType itemType;
                    if (itemCode.Length == productLen) itemType = ItemType.Product;
                    else if (itemCode.Length == semiFinishedLen) itemType = ItemType.SemiFinished;
                    else itemType = ItemType.Material;

                    int num = rd.Next(0, 10);
                    Item mainItem = new Item();
                    mainItem.GenerateId();
                    mainItem.Code = itemCode;
                    mainItem.Name = itemCode;
                    mainItem.Type = itemType;
                    mainItem.ItemSourceType = ItemSourceType.SelfMade;
                    if (materialCode.Contains(itemCode)) mainItem.ItemSourceType = ItemSourceType.Outsourcing;

                    mainItem.Unit = units[num];
                    mainItem.UnitId = mainItem.Unit.Id;
                    mainItem.Model = productModels[num];
                    mainItem.ModelId = mainItem.Model.Id;

                    itemList.Add(mainItem);
                }

                RF.Save(itemList);
            }

            return itemList;
        }

        /// <summary>
        /// 创建多个产品BOM
        /// </summary>
        /// <param name="processSegments">工段</param>
        /// <returns>返回新建多个的产品BOM</returns>
        public virtual EntityList<ProductBom> CreateAPSProductBom(EntityList<ProcessSegment> processSegments)
        {
            EntityList<Unit> units = CreateAPSUnits();
            EntityList<ProductModel> productModels = CreateAPSProductModel();
            EntityList<Item> itemList = CreateAPSItem(units, productModels);

            List<Item> productItemList = itemList.Where(p => p.Type == ItemType.Product).OrderBy(p => p.Code).ToList();
            List<Item> semiFinishedItemList = itemList.Where(p => p.Type == ItemType.SemiFinished).ToList();
            List<Item> materialItemList = itemList.Where(p => p.Type == ItemType.Material).ToList();

            // 产品BOM编号
            List<string> poBomCodeList = new List<string>();
            List<string> poChildBomCodeList = new List<string>();
            for (int i = 0; i < productItemList.Count; i++)
            {
                string tmpCode = "APSPOBOM" + i.ToString().PadLeft(3, '0');
                poBomCodeList.Add(tmpCode);
                poChildBomCodeList.Add(tmpCode + "_Child" + "0".PadLeft(3, '0'));
                poChildBomCodeList.Add(tmpCode + "_Child" + "1".PadLeft(3, '0'));
                poChildBomCodeList.Add(tmpCode + "_Child" + "2".PadLeft(3, '0'));
            }

            EntityList<ProductBom> poBomList = GetAPSProductBom(poBomCodeList.Union(poChildBomCodeList).ToList());
            if (poBomCodeList.Count != poBomList.Count)
            {
                // 创建成品的产品BOM
                for (int i = 0; i < poBomCodeList.Count; i++)
                {
                    string poBomCode = poBomCodeList[i];
                    if (!poBomList.Any(p => p.Code == poBomCodeList[i]))
                    {
                        // 产品BOM
                        Item proItem = productItemList[i];
                        ProductBom mainProductBom = CreateAPSProductBom(poBomCode, proItem);

                        // 半成品
                        List<Item> detailItems = semiFinishedItemList.Where(p => p.Code.Contains(proItem.Code)).ToList();
                        for (int j = 0; j < detailItems.Count; j++)
                        {
                            // 添加产品BOM明细
                            Item detailItem = detailItems[j];
                            ProductBomDetail detail = CreateAPSProductBomDetail(processSegments[j], detailItem, mainProductBom);
                            mainProductBom.DetailList.Add(detail);

                            // 创建产品BOM明细
                            string childBomCode = poChildBomCodeList[i * 3 + j];
                            ProductBom childProductBom = CreateAPSProductBom(childBomCode, detailItem);
                            List<Item> tempItems = materialItemList.Where(p => p.Code.Contains(detailItem.Code)).ToList();
                            // 创建原材料
                            for (int z = 0; z < tempItems.Count; z++)
                            {
                                Item tempItem = tempItems[z];
                                ProductBomDetail tempDetail = CreateAPSProductBomDetail(processSegments[j], tempItem, childProductBom);
                                childProductBom.DetailList.Add(tempDetail);
                            }

                            poBomList.Add(childProductBom);
                        }

                        poBomList.Add(mainProductBom);
                    }
                }
            }

            return poBomList;
        }

        /// <summary>
        /// 创建APS生产订单BOM
        /// </summary>
        /// <param name="poBomCode">产品Bom编号</param>
        /// <param name="proItem">物料</param>
        /// <returns>返回APS生产订单BOM</returns>
        private ProductBom CreateAPSProductBom(string poBomCode, Item proItem)
        {
            ProductBom mainProductBom = new ProductBom();
            mainProductBom.GenerateId();
            mainProductBom.Code = poBomCode;
            mainProductBom.Name = poBomCode;
            mainProductBom.SourceType = SourceType.Internal;
            mainProductBom.Product = proItem;
            mainProductBom.ProductId = proItem.Id;

            return mainProductBom;
        }

        /// <summary>
        /// 创建产品BOM明细
        /// </summary>
        /// <param name="processSegment">工段</param>
        /// <param name="item">明细对应物料</param>
        /// <param name="poBom">产品BOM</param>
        /// <returns></returns>
        private ProductBomDetail CreateAPSProductBomDetail(ProcessSegment processSegment, Item item, ProductBom poBom)
        {
            ProductBomDetail tempDetail = new ProductBomDetail();
            tempDetail.GenerateId();
            tempDetail.ProcessSegment = processSegment;
            tempDetail.ProcessSegmentId = processSegment.Id;
            tempDetail.UnitQty = 1;
            tempDetail.LossRate = 1;
            tempDetail.Item = item;
            tempDetail.ItemId = item.Id;
            tempDetail.ProductBom = poBom;
            tempDetail.ProductBomId = poBom.Id;

            return tempDetail;
        }

        /// <summary>
        /// 根据物料编号获取物料
        /// </summary>
        /// <param name="codes">物料编号</param>
        /// <returns>返回物料</returns>
        public virtual EntityList<Item> GetAPSItem(List<string> codes)
        {
            return Query<Item>().Where(p => codes.Contains(p.Code)).ToList();
        }

        /// <summary>
        /// 根据产品BOM编号获取产品BOM
        /// </summary>
        /// <param name="bomId">产品BOM编号</param>
        /// <returns>返回产品BOM</returns>
        public virtual EntityList<ProductBom> GetAPSProductBom(List<string> codes)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(ProductBom.ProductProperty);
            elo.LoadWith(ProductBom.DetailListProperty);
            elo.LoadWith(ProductBomDetail.ItemProperty);
            elo.LoadWith(Item.UnitProperty);

            return Query<ProductBom>().Where(p => codes.Contains(p.Code)).ToList(null, elo);
        }
    }
}