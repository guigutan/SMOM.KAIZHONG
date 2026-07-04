using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.Items;
using SIE.Items.ProductFamilys;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Item = SIE.Items.Item;
using ItemController = SIE.Items.ItemController;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadItemController : DomainController
    {

        /// <summary>
        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveItemsFactory(List<ItemData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };

            List<ItemData> list = new List<ItemData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.Item, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //获取物料数据
                    var ctl = RT.Service.Resolve<ItemController>();
                    var itemCodeList = datas.Select(p => p.MATNR).Where(p => !p.IsNullOrEmpty()).Distinct().ToList();
                    var items = ctl.GetItemListByCodeNoViewProperty(itemCodeList);//已存在的数据
                    var itemsDic = items.ToDictionary(p => p.Code);

                    //var units = ctl.GetUnitList(datas.Select(p => p.measdoc).Distinct().ToList());//已存在的单位数据
                    //所有单位
                    var units = RF.GetAll<Unit>(null, new EagerLoadOptions().LoadWithViewProperty());// 已存在的单位数据
                    var unitsDic = units.ToDictionary(p => p.Code, p => p);

                    var productFamily = RT.Service.Resolve<ProductFamilyController>().GetProductFamilyFirst();
                    if (productFamily == null)
                        throw new ValidationException("请维护一个产品族信息!".L10N());

                    var itemCategoryList = ctl.GetAllItemCategorys(CategoryType.Item);
                    var itemCategoryDic = itemCategoryList.ToDictionary(p => p.Code);
                    EntityList<Item> saveList = new EntityList<Item>();
                    EntityList<ItemCategoryRelation> catRelationList = new EntityList<ItemCategoryRelation>();
                    //EntityList<Core.Items.ItemBatchRule> batchList = new EntityList<Core.Items.ItemBatchRule>();
                    foreach (var item in datas)
                    {
                        try
                        {
                            if (item != null)
                            {
                                #region 验证数据
                                //如果单位不存在就创建一个新的单位
                                if (!item.MEINS.IsNullOrEmpty() && !unitsDic.ContainsKey(item.MEINS))
                                {
                                    var unit = CreateUnit(item.MEINS);
                                    unitsDic.Add(item.MEINS, unit);
                                }
                                string valires = ValidateData(item, unitsDic, itemCategoryDic);
                                if (valires != null)
                                {
                                    apiResult.ErrorList.Add(valires);
                                    apiResult.ErrorObjList.Add(item);
                                    failCount++;
                                    continue;
                                }
                                #endregion 验证数据
                                Item itemData = null;
                                //新增
                                if (!itemsDic.ContainsKey(item.MATNR))
                                {
                                    itemData = new Item();
                                    itemData.GenerateId();
                                    itemData.Code = item.MATNR;
                                    itemData.ConsumeMode = ConsumeMode.Pull;
                                    itemData.SourceType = SIE.Common.SourceType.External;
                                    itemsDic.Add(item.MATNR, itemData);
                                    //默认生成生产批次规则
                                    //ItemBatchRule itemBatchRule = new SIE.Core.Items.ItemBatchRule() { RetrospectType = RT.Service.Resolve<ItemController>().GetRetrospectTypeConfig(), Item = itemData, ItemId = itemData.Id };
                                    //batchList.Add(itemBatchRule);
                                }
                                else
                                {
                                    //更新
                                    itemData = itemsDic[item.MATNR];
                                }

                                //物料分类
                                var curItemCategory = itemCategoryDic[item.MATKL];
                                if (curItemCategory == null)
                                {
                                    apiResult.ErrorList.Add("物料编码:{0}的分类编码{1}不存在!".L10nFormat(item.MATNR, item.MATKL));
                                    failCount++;
                                    continue;
                                }
                                itemData.Name = item.MAKTX;
                                itemData.ItemSourceType = ItemSourceType.SelfMade;

                                //单位
                                itemData.Unit = unitsDic[item.MEINS];

                                //创建物料与分类关系
                                var itemCategorys = itemData.GetLazyList(ItemExtCategoryListProperty.ItemCategoryListProperty);
                                if (itemCategorys != null && itemData.PersistenceStatus == PersistenceStatus.New)
                                {
                                    itemCategorys.Add(new ItemCategoryRelation() { Type = Items.Items.CategoryType.Item, Item = itemData });
                                    itemCategorys.Add(new ItemCategoryRelation() { Type = Items.Items.CategoryType.Quality, Item = itemData });
                                    itemCategorys.Add(new ItemCategoryRelation() { Type = Items.Items.CategoryType.Kit, Item = itemData });
                                }
                                if (itemCategorys != null)
                                {
                                    itemCategorys.OfType<ItemCategoryRelation>()
                                        .FirstOrDefault(x => x.Type == CategoryType.Item).ItemCategory = curItemCategory;
                                }
                                if (!item.ZMODEL.IsNullOrEmpty())
                                    itemData.SpecificationModel = item.ZMODEL;
                                if (!item.ZGG.IsNullOrEmpty())
                                {
                                    if (itemData.SpecificationModel.IsNullOrEmpty())
                                        itemData.SpecificationModel = item.ZGG;
                                    else
                                        itemData.SpecificationModel += "," + item.ZGG;
                                }
                                itemData.State = State.Enable;
                                itemData.ShortDescription = item.BISMT;
                                itemData.Mtart = item.MTART;
                                itemData.Zmodel = item.ZMODEL;
                                itemData.Zgg = item.ZGG;
                                if (item.BSTRF > 0)
                                    itemData.MinPackingQty = item.BSTRF;
                                if (item.MTART == "KZ01")
                                    itemData.Type = ItemType.Product;
                                else if (item.MTART == "KZ03" || item.MTART == "KZ02")
                                    itemData.Type = ItemType.SemiFinished;
                                else
                                    itemData.Type = ItemType.Material;
                                if (item.MSTAE == "Z1" || item.MSTAE == "Z2")
                                    itemData.GroupState = State.Enable;
                                else
                                    itemData.GroupState = State.Disable;
                                if (item.MMSTA == "Z1" || item.MMSTA == "Z2")
                                    itemData.FactoryState = State.Enable;
                                else
                                    itemData.FactoryState = State.Disable;

                                itemData.MrpController = item.DISPO;
                                itemData.SuccessorItem = item.NFMAT;

                                if (DateTime.TryParseExact(item.AUSDT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                                {
                                    itemData.SuccessorEffeTime = result;
                                }
                                if (item.NTGEW > 0)
                                    itemData.Weight = (decimal?)item.NTGEW;
                                itemData.WeightUnit = item.GEWEI;
                                itemData.ProductFamilyId = productFamily.Id;
                                itemData.Zmc = item.ZMC;
                                itemData.Normt = item.NORMT;
                                itemData.Zkhxhgy = item.ZKHXHGY;
                                itemData.Zjdlx = item.ZJDLX;
                                if (!item.ZKUNAG.IsNullOrEmpty() || !item.ZQTTX.IsNullOrEmpty())
                                {

                                    //当客户为空，特性不为空时
                                    if (item.ZKUNAG.IsNullOrEmpty() && !item.ZQTTX.IsNullOrEmpty())
                                    {
                                        if (!itemData.CustomFeatureRelList.Any(p => p.Zqttx == item.ZQTTX && (p.Customer == null || p.Customer == "")))
                                        {
                                            var itemCusotmer = new CustomFeatureRel();
                                            itemCusotmer.ItemId = itemData.Id;
                                            itemCusotmer.Item = itemData;
                                            itemCusotmer.Zqttx = item.ZQTTX;
                                            itemCusotmer.PersistenceStatus = PersistenceStatus.New;
                                            itemData.CustomFeatureRelList.Add(itemCusotmer);
                                        }
                                    }
                                    //当客户不为空，特性为空时
                                    if (!item.ZKUNAG.IsNullOrEmpty() && item.ZQTTX.IsNullOrEmpty())
                                    {
                                        if (!itemData.CustomFeatureRelList.Any(p => p.Customer == item.ZKUNAG && (p.Zqttx == null || p.Zqttx == "")))
                                        {
                                            var itemCusotmer = new CustomFeatureRel();
                                            itemCusotmer.ItemId = itemData.Id;
                                            itemCusotmer.Item = itemData;
                                            itemCusotmer.Customer = item.ZKUNAG;
                                            itemCusotmer.PersistenceStatus = PersistenceStatus.New;
                                            itemData.CustomFeatureRelList.Add(itemCusotmer);
                                        }
                                    }
                                    //当客户为空，特性为空
                                    if (!item.ZKUNAG.IsNullOrEmpty() && !item.ZQTTX.IsNullOrEmpty())
                                    {
                                        if (!itemData.CustomFeatureRelList.Any(p => p.Customer == item.ZKUNAG && p.Zqttx == item.ZQTTX))
                                        {
                                            var itemCusotmer = new CustomFeatureRel();
                                            itemCusotmer.Customer = item.ZKUNAG;
                                            itemCusotmer.Zqttx = item.ZQTTX;
                                            itemCusotmer.ItemId = itemData.Id;
                                            itemCusotmer.Item = itemData;
                                            itemCusotmer.PersistenceStatus = PersistenceStatus.New;
                                            itemData.CustomFeatureRelList.Add(itemCusotmer);
                                        }
                                    }

                                }

                                saveList.Add(itemData);
                                list.Add(item);
                                RF.Save(itemData);
                                apiResult.SuccessList.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            apiResult.ErrorList.Add($"物料编码{item.MATNR}:" + ex.GetBaseException()?.Message);
                            apiResult.ErrorObjList.Add(item);
                            failCount++;
                            continue;
                        }

                    }
                    if (saveList.Count > 0)
                    {
                        //using (var tran = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
                        //{
                        //    RF.Save(batchList);
                        //    tran.Complete();
                        //}
                        logController.UpadateLogData<ItemData>(erpDataInfLog, list, apiResult);
                    }
                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能未空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorList.Add(ex.Message);
                apiResult.ErrorObjList.Clear();
                apiResult.ErrorObjList.AddRange(datas);
                logController.UpadateLogData<ItemData>(erpDataInfLog, null, apiResult, ex.Message, 1);
            }
            return apiResult;
        }

        /// <summary>
        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="erpDataInfLog">日志</param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveItems(List<ItemData> datas, ref InfNcDataLogGroup erpDataInfLog)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };

            List<ItemData> list = new List<ItemData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            int failCount = 0;
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //获取物料数据
                    var ctl = RT.Service.Resolve<ItemController>();
                    var itemCodeList = datas.Select(p => p.MATNR).Where(p => !p.IsNullOrEmpty()).Distinct().ToList();
                    var items = ctl.GetItemListByCodeNoViewProperty(itemCodeList);//已存在的数据
                    var itemsDic = items.ToDictionary(p => p.Code);

                    //var units = ctl.GetUnitList(datas.Select(p => p.measdoc).Distinct().ToList());//已存在的单位数据
                    //所有单位
                    var units = RF.GetAll<Unit>(null, new EagerLoadOptions().LoadWithViewProperty());
                    var unitsDic = units.ToDictionary(p => p.Name, p => p);
                    var itemCategoryList = ctl.GetAllItemCategorys(CategoryType.Item);
                    var itemCategoryDic = itemCategoryList.ToDictionary(p => p.Code);

                    var productFamily = RT.Service.Resolve<ProductFamilyController>().GetProductFamilyFirst();
                    if (productFamily == null)
                        throw new ValidationException("请维护一个产品族信息!".L10N());

                    EntityList<Item> saveList = new EntityList<Item>();
                    EntityList<ItemCategoryRelation> catRelationList = new EntityList<ItemCategoryRelation>();
                    //EntityList<Core.Items.ItemBatchRule> batchList = new EntityList<Core.Items.ItemBatchRule>();
                    foreach (var item in datas)
                    {
                        try
                        {
                            if (item != null)
                            {
                                #region 验证数据
                                //如果单位不存在就创建一个新的单位
                                if (!item.MEINS.IsNullOrEmpty() && !unitsDic.ContainsKey(item.MEINS))
                                {
                                    var unit = CreateUnit(item.MEINS);
                                    unitsDic.Add(item.MEINS, unit);
                                }
                                string valires = ValidateData(item, unitsDic, itemCategoryDic);
                                if (valires != null)
                                {
                                    apiResult.ErrorList.Add(valires);
                                    apiResult.ErrorObjList.Add(item);
                                    failCount++;
                                    continue;
                                }
                                #endregion 验证数据
                                Item itemData = null;
                                //新增
                                if (!itemsDic.ContainsKey(item.MATNR))
                                {
                                    itemData = new Item();
                                    itemData.GenerateId();
                                    itemData.Code = item.MATNR;
                                    itemData.ConsumeMode = ConsumeMode.Pull;

                                    itemData.SourceType = SIE.Common.SourceType.External;
                                    itemsDic.Add(item.MATNR, itemData);

                                    //ItemBatchRule itemBatchRule = new SIE.Core.Items.ItemBatchRule() { RetrospectType = RT.Service.Resolve<ItemController>().GetRetrospectTypeConfig(), Item = itemData, ItemId = itemData.Id };
                                    //batchList.Add(itemBatchRule);
                                }
                                else
                                {
                                    //更新
                                    itemData = itemsDic[item.MATNR];
                                }


                                //物料分类
                                var curItemCategory = itemCategoryDic[item.MATKL];
                                if (curItemCategory == null)
                                {
                                    apiResult.ErrorList.Add("物料编码:{0}的分类编码{1}不存在!".L10nFormat(item.MATNR, item.MATKL));
                                    failCount++;
                                    continue;
                                }
                                itemData.Name = item.MAKTX;
                                itemData.ItemSourceType = ItemSourceType.SelfMade;

                                //单位
                                itemData.Unit = unitsDic[item.MEINS];

                                //创建物料与分类关系
                                var itemCategorys = itemData.GetLazyList(ItemExtCategoryListProperty.ItemCategoryListProperty);
                                if (itemCategorys != null && itemData.PersistenceStatus == PersistenceStatus.New)
                                {
                                    itemCategorys.Add(new ItemCategoryRelation() { Type = Items.Items.CategoryType.Item, Item = itemData });
                                    itemCategorys.Add(new ItemCategoryRelation() { Type = Items.Items.CategoryType.Quality, Item = itemData });
                                    itemCategorys.Add(new ItemCategoryRelation() { Type = Items.Items.CategoryType.Kit, Item = itemData });
                                }
                                if (itemCategorys != null)
                                {
                                    itemCategorys.OfType<ItemCategoryRelation>()
                                        .FirstOrDefault(x => x.Type == CategoryType.Item).ItemCategory = curItemCategory;
                                }

                                if (!item.ZMODEL.IsNullOrEmpty())
                                    itemData.SpecificationModel = item.ZMODEL;
                                if (!item.ZGG.IsNullOrEmpty())
                                {
                                    if (itemData.SpecificationModel.IsNullOrEmpty())
                                        itemData.SpecificationModel = item.ZGG;
                                    else
                                        itemData.SpecificationModel += "," + item.ZGG;
                                }
                                itemData.State = State.Enable;
                                itemData.ShortDescription = item.BISMT;
                                itemData.Mtart = item.MTART;
                                if (item.BSTRF > 0)
                                    itemData.MinPackingQty = item.BSTRF;
                                if (item.MTART == "KZ01")
                                    itemData.Type = ItemType.Product;
                                else if (item.MTART == "KZ03" || item.MTART == "KZ02")
                                    itemData.Type = ItemType.SemiFinished;
                                else
                                    itemData.Type = ItemType.Material;
                                if (item.MSTAE == "Z1" || item.MSTAE == "Z2")
                                    itemData.GroupState = State.Enable;
                                else
                                    itemData.GroupState = State.Disable;
                                if (item.MMSTA == "Z1" || item.MMSTA == "Z2")
                                    itemData.FactoryState = State.Enable;
                                else
                                    itemData.FactoryState = State.Disable;

                                itemData.MrpController = item.DISPO;
                                itemData.SuccessorItem = item.NFMAT;

                                if (DateTime.TryParseExact(item.AUSDT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                                {
                                    itemData.SuccessorEffeTime = result;
                                }
                                if (item.NTGEW > 0)
                                    itemData.Weight = (decimal?)item.NTGEW;
                                itemData.WeightUnit = item.GEWEI;
                                itemData.ProductFamilyId = productFamily.Id;
                                itemData.Zmc = item.ZMC;
                                itemData.Normt = item.NORMT;
                                itemData.Zkhxhgy = item.ZKHXHGY;
                                itemData.Zjdlx = item.ZJDLX;
                                itemData.Zmodel = item.ZMODEL;
                                itemData.Zgg = item.ZGG;

                                if (!item.ZKUNAG.IsNullOrEmpty() || !item.ZQTTX.IsNullOrEmpty())
                                {

                                    //当客户为空，特性不为空时
                                    if (item.ZKUNAG.IsNullOrEmpty() && !item.ZQTTX.IsNullOrEmpty())
                                    {
                                        if (!itemData.CustomFeatureRelList.Any(p => p.Zqttx == item.ZQTTX && (p.Customer == null || p.Customer == "")))
                                        {
                                            var itemCusotmer = new CustomFeatureRel();
                                            itemCusotmer.ItemId = itemData.Id;
                                            itemCusotmer.Item = itemData;
                                            itemCusotmer.Zqttx = item.ZQTTX;
                                            itemCusotmer.PersistenceStatus = PersistenceStatus.New;
                                            itemData.CustomFeatureRelList.Add(itemCusotmer);
                                        }
                                    }
                                    //当客户不为空，特性为空时
                                    if (!item.ZKUNAG.IsNullOrEmpty() && item.ZQTTX.IsNullOrEmpty())
                                    {
                                        if (!itemData.CustomFeatureRelList.Any(p => p.Customer == item.ZKUNAG && (p.Zqttx == null || p.Zqttx == "")))
                                        {
                                            var itemCusotmer = new CustomFeatureRel();
                                            itemCusotmer.ItemId = itemData.Id;
                                            itemCusotmer.Item = itemData;
                                            itemCusotmer.Customer = item.ZKUNAG;
                                            itemCusotmer.PersistenceStatus = PersistenceStatus.New;
                                            itemData.CustomFeatureRelList.Add(itemCusotmer);
                                        }
                                    }
                                    //当客户为空，特性为空
                                    if (!item.ZKUNAG.IsNullOrEmpty() && !item.ZQTTX.IsNullOrEmpty())
                                    {
                                        if (!itemData.CustomFeatureRelList.Any(p => p.Customer == item.ZKUNAG && p.Zqttx == item.ZQTTX))
                                        {
                                            var itemCusotmer = new CustomFeatureRel();
                                            itemCusotmer.Customer = item.ZKUNAG;
                                            itemCusotmer.Zqttx = item.ZQTTX;
                                            itemCusotmer.ItemId = itemData.Id;
                                            itemCusotmer.Item = itemData;
                                            itemCusotmer.PersistenceStatus = PersistenceStatus.New;
                                            itemData.CustomFeatureRelList.Add(itemCusotmer);
                                        }
                                    }
                                }

                                saveList.Add(itemData);
                                RF.Save(itemData);
                                list.Add(item);
                                apiResult.SuccessList.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            apiResult.ErrorList.Add($"物料编码{item.MATNR}:" + ex.GetBaseException()?.Message);
                            failCount++;
                            continue;
                        }

                    }
                    if (saveList.Count > 0)
                    {
                        //using (var tran = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
                        //{
                        //    RF.Save(batchList);
                        //    tran.Complete();
                        //}
                    }
                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能未空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorList.Add(ex.Message);
            }
            finally
            {
                erpDataInfLog = RT.Service.Resolve<InfNcDataLogGroupController>().UpdateInfNcDataLogGroupData<ItemData>(erpDataInfLog, datas.Count, list, apiResult, false);
            }
            return apiResult;
        }

        /// <summary>
        /// 创建单位
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual Unit CreateUnit(string key)
        {
            Unit unit = new Unit();
            unit.Code = key;
            unit.Name = key;
            unit.UnitSource = UnitSource.Interface;
            unit.TradeType = TradeType.HalfAdjust;
            unit.IsInit = false;
            unit.PersistenceStatus = PersistenceStatus.New;
            RF.Save(unit);
            return unit;
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="unitsDic"></param>
        /// <param name="productProjectMdmCodeDic"></param>
        /// <param name="mdmItemCategoryLookup"></param>
        /// <returns></returns>
        public virtual string ValidateData(ItemData item, Dictionary<string, Unit> unitsDic, Dictionary<string, ItemCategory> itemCategoryDic)
        {
            if (item.MATNR.IsNullOrEmpty())
                return "物料编码不能为空!".L10N();

            if (item.MAKTX.IsNullOrEmpty())
                return "物料编码:{0}的名称不能为空!".L10nFormat(item.MATNR);

            if (item.MEINS.IsNullOrEmpty())
                return "物料编码:{0}的单位为空!".L10nFormat(item.MATNR);

            if (!unitsDic.ContainsKey(item.MEINS) || unitsDic[item.MEINS] == null)
                return "物料编码:{0}的单位{1}不存在!".L10nFormat(item.MATNR, item.MEINS);
            if(item.MATKL.IsNullOrEmpty())
                return "物料编码:{0}的分类编码不能为空!".L10nFormat(item.MATNR, item.MATKL);
            if (!itemCategoryDic.ContainsKey(item.MATKL) || itemCategoryDic[item.MATKL] == null)
                return "物料编码:{0}的分类编码{1}不存在!".L10nFormat(item.MATNR, item.MATKL);

            return null;
        }



    }
}
