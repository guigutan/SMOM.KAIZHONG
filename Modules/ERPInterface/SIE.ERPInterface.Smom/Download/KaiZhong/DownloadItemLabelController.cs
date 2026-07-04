using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using NPOI.SS.Formula.PTG;
using SIE.Core.Labels;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.Inventory.Commom;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.Packages.ItemLabels;
using SIE.Resources.Enterprises;
using SIE.Resources.Skills;
using SIE.Warehouses;
using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Item = SIE.Items.Item;
using ItemLabel = SIE.Packages.ItemLabels.ItemLabel;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    /// <summary>
    /// 物料标签下载控制器
    /// </summary>
    public class DownloadItemLabelController : DomainController
    {

        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveItemLabel(List<ItemLabelData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<ItemLabelData> list = new List<ItemLabelData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.ItemLabel, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //获取全部标签
                    //var exidvs = datas.Select(p => p.EXIDV).Distinct().ToList();
                    var exidvs = datas.Select(p => (p.EXIDV2.IsNullOrEmpty() ? p.EXIDV : p.EXIDV2)).Distinct().ToList();
                    var labels = RT.Service.Resolve<ItemLabelController>().GetItemLabelDatas(exidvs);

                    //获取物料
                    var matnrs = datas.Select(p => p.MATNR).Distinct().ToList();
                    var items = RT.Service.Resolve<ItemController>().GetItems(matnrs, new EagerLoadOptions().LoadWithViewProperty());
                    //获取库存资料
                    var itemIds = items.Select(p => p.Id).Distinct().ToList();
                    var itemStocksDic = RT.Service.Resolve<ItemStockBaseController>().GetItemStockDataBases(itemIds).ToDictionary(p => p.ItemId, p => p.IsSerialNumber == true);
                    //获取库位
                    //var storageLocationCodes = datas.Select(p => p.LGORT).Distinct().ToList();
                    //var storageLocations = RT.Service.Resolve<WarehouseController>().GetStorageLocations(storageLocationCodes, new EagerLoadOptions().LoadWithViewProperty());

                    //获取工厂
                    var factoryCodes = datas.Select(p => p.WERKS).Distinct().ToList();
                    var factorys = RT.Service.Resolve<EnterpriseController>().GetFactoryByCode(factoryCodes);

                    //随机获取一个库区
                    //var area = Query<StorageArea>().Where(p => p.LibraryType == LibraryType.Entity && p.Warehouse.LibraryType == LibraryType.Entity).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

                    foreach (var item in datas)
                    {
                        try
                        {
                            var exidv = (item.EXIDV2.IsNullOrEmpty() ? item.EXIDV : item.EXIDV2);
                            var label = labels.FirstOrDefault(p => p.Label == exidv);
                            var product = items.FirstOrDefault(p => p.Code == item.MATNR);
                            if (product == null)
                                throw new ValidationException("物料[{0}]不存在".L10nFormat(item.MATNR));

                            itemStocksDic.TryGetValue(product.Id, out bool isSer);

                            //var storageLocation = storageLocations.FirstOrDefault(p => p.Code == item.LGORT);
                            //if (storageLocation == null)
                            //{
                            //    if (area == null)
                            //        throw new ValidationException("请先维护一个实体类型的库区和仓库".L10N());
                            //    storageLocation = CreateStorageLocation(item.LGORT, area);
                            //    if (storageLocations.All(p => p.Id != storageLocation.Id))
                            //        storageLocations.Add(storageLocation);
                            //}

                            var factory = factorys.FirstOrDefault(p => p.Code == item.WERKS);
                            if (factory == null)
                                throw new ValidationException("企业模型不存在编码为[{0}]的工厂".L10nFormat(item.WERKS));

                            //创建新数据
                            label = CreateItemLabel(item, label, product, isSer, factory);
                            if (labels.All(p => p.Id != label.Id))
                                labels.Add(label);

                            list.Add(item);
                            apiResult.SuccessList.Add(item);
                        }
                        catch (Exception ex)
                        {
                            throw new ValidationException($"EXIDV:{item.EXIDV},EXIDV2:{item.EXIDV2}:" + ex.GetBaseException()?.Message);
                        }
                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<ItemLabelData>(erpDataInfLog, list, apiResult);

                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能为空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorObjList.Clear();
                apiResult.ErrorObjList.AddRange(datas);
                apiResult.ErrorList.Add(ex.Message);
                logController.UpadateLogData<ItemLabelData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }

        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveItemLabel(List<ItemLabelData> datas, ref InfNcDataLogGroup erpDataInfLog)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };

            List<ItemLabelData> list = new List<ItemLabelData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            int failCount = 0;

            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //获取全部标签
                    //var exidvs = datas.Select(p => p.EXIDV).Distinct().ToList();
                    var exidvs = datas.Select(p => (p.EXIDV2.IsNullOrEmpty() ? p.EXIDV : p.EXIDV2)).Distinct().ToList();
                    var labels = RT.Service.Resolve<ItemLabelController>().GetItemLabelDatas(exidvs);

                    //获取物料
                    var matnrs = datas.Select(p => p.MATNR).Distinct().ToList();
                    var items = RT.Service.Resolve<ItemController>().GetItems(matnrs, new EagerLoadOptions().LoadWithViewProperty());
                    //获取库存资料
                    var itemIds = items.Select(p => p.Id).Distinct().ToList();
                    var itemStocksDic = RT.Service.Resolve<ItemStockBaseController>().GetItemStockDataBases(itemIds).ToDictionary(p => p.ItemId, p => p.IsSerialNumber == true);
                    //获取库位
                    //var storageLocationCodes = datas.Select(p => p.LGORT).Distinct().ToList();
                    //var storageLocations = RT.Service.Resolve<WarehouseController>().GetStorageLocations(storageLocationCodes, new EagerLoadOptions().LoadWithViewProperty());

                    //获取工厂
                    var factoryCodes = datas.Select(p => p.WERKS).Distinct().ToList();
                    var factorys = RT.Service.Resolve<EnterpriseController>().GetFactoryByCode(factoryCodes);

                    //随机获取一个库区
                    //var area = Query<StorageArea>().Where(p => p.LibraryType == LibraryType.Entity && p.Warehouse.LibraryType == LibraryType.Entity).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

                    foreach (var item in datas)
                    {
                        try
                        {
                            if (item != null)
                            {
                                var exidv = (item.EXIDV2.IsNullOrEmpty() ? item.EXIDV : item.EXIDV2);
                                var label = labels.FirstOrDefault(p => p.Label == exidv);
                                var product = items.FirstOrDefault(p => p.Code == item.MATNR);
                                if (product == null)
                                    throw new ValidationException("物料[{0}]不存在".L10nFormat(item.MATNR));

                                itemStocksDic.TryGetValue(product.Id, out bool isSer);

                                //var storageLocation = storageLocations.FirstOrDefault(p => p.Code == item.LGORT);
                                //if (storageLocation == null)
                                //{
                                //    if (area == null)
                                //        throw new ValidationException("请先维护一个实体类型的库区和仓库".L10N());
                                //    storageLocation = CreateStorageLocation(item.LGORT, area);
                                //    if (storageLocations.All(p => p.Id != storageLocation.Id))
                                //        storageLocations.Add(storageLocation);
                                //}

                                var factory = factorys.FirstOrDefault(p => p.Code == item.WERKS);
                                if (factory == null)
                                    throw new ValidationException("企业模型不存在编码为[{0}]的工厂".L10nFormat(item.WERKS));

                                //创建新数据
                                label = CreateItemLabel(item, label, product, isSer, factory);
                                if (labels.All(p => p.Id != label.Id))
                                    labels.Add(label);

                                list.Add(item);
                                apiResult.SuccessList.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            apiResult.ErrorList.Add($"EXIDV:{item.EXIDV},EXIDV2:{item.EXIDV2}:" + ex.GetBaseException()?.Message);
                            failCount++;
                            continue;
                        }

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
                erpDataInfLog = RT.Service.Resolve<InfNcDataLogGroupController>().UpdateInfNcDataLogGroupData<ItemLabelData>(erpDataInfLog, datas.Count, list, apiResult, false);
            }
            return apiResult;
        }

        /// <summary>
        /// 创建库位
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private StorageLocation CreateStorageLocation(string key, StorageArea area)
        {
            StorageLocation storageLocation = new StorageLocation();

            storageLocation.Code = key;
            storageLocation.Name = key;
            storageLocation.IsFrozen = false;
            storageLocation.State = State.Enable;
            storageLocation.LibraryType = LibraryType.Entity;
            storageLocation.AreaId = area.Id;
            storageLocation.Area = area;
            storageLocation.Warehouse = area.Warehouse;
            storageLocation.WarehouseId = area.WarehouseId;
            storageLocation.PersistenceStatus = PersistenceStatus.New;
            RF.Save(storageLocation);

            return storageLocation;
        }

        /// <summary>
        /// 创建物料标签
        /// </summary>
        /// <param name="data"></param>
        /// <param name="itemLabel"></param>
        /// <param name="item"></param>
        /// <param name="isSer"></param>
        /// <param name="storageLocation"></param>
        /// <returns></returns>
        private ItemLabel CreateItemLabel(ItemLabelData data, ItemLabel itemLabel, Item item, bool isSer, Enterprise factory)
        {
            if (itemLabel == null)
            {
                var exidv = (data.EXIDV2.IsNullOrEmpty() ? data.EXIDV : data.EXIDV2);
                itemLabel = new ItemLabel
                {
                    Label = exidv,//data.EXIDV,
                    Qty = data.VEMNG,
                    InitialQty = data.VEMNG,
                    ItemId = item.Id,
                    Item = item,
                    SourceType = LabelSource.Import
                };
            }
            itemLabel.Exidv = data.EXIDV;
            itemLabel.Exidv2 = data.EXIDV2;
            itemLabel.Lgort = data.LGORT;
            itemLabel.Lot = data.CHARG;
            itemLabel.FactoryId = factory?.Id;
            itemLabel.Factory = factory;
            //itemLabel.SupplierId = supplier?.Id;
            //itemLabel.Supplier = supplier;
            itemLabel.IsSerialNumber = isSer;
            //itemLabel.StorageLocationId = storageLocation.Id;
            //itemLabel.StorageLocation = storageLocation;
            //itemLabel.WarehouseId = storageLocation.WarehouseId;
            //itemLabel.ProductionDate = productTime,
            itemLabel.ItemLabelState = ItemLabelState.Receive;
            itemLabel.Licha = data.LICHA;
            if (itemLabel.PersistenceStatus != PersistenceStatus.Unchanged)
                RF.Save(itemLabel);
            return itemLabel;
        }

    }
}
