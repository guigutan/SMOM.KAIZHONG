using SIE.Common.Catalogs;
using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.Items;
using SIE.LES.Commons;
using SIE.LES.LinesideWarehouses;
using SIE.Packages.ItemLabels;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.LES.PrepareItems
{
    /// <summary>
    /// 导入备料模式维护-拉式
    /// </summary>
    [Services.Service(FallbackType = typeof(PrepareItemPullImportHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class PrepareItemPullImportHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "仓库编码","物料类型","物料编码","物料扩展属性","触发方式","最低安全水位","需求计算方式","最高存量","固定量"
        };

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        private Dictionary<string, Warehouse> dicWarehouse { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        private Dictionary<string, ItemCategory> dicItemCategory { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        private Dictionary<string, Item> dicItem { get; set; }

        /// <summary>
        /// 创建列的验证
        /// </summary>
        /// <returns>返回当前对象</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add(ColumnNameList[0], new ValidColumn(ImportDataType._String, true, VaildWarehouseCode));// 仓库编码
            this.ColumnValidList.Add(ColumnNameList[1], new ValidColumn(ImportDataType._String, false, VaildItemCategoryCode));// 物料类型编码         
            this.ColumnValidList.Add(ColumnNameList[2], new ValidColumn(ImportDataType._String, false, VaildItemCode));//物料编码
            this.ColumnValidList.Add(ColumnNameList[3], new ValidColumn(ImportDataType._String, false, true));//物料扩展属性
            this.ColumnValidList.Add(ColumnNameList[4], new ValidColumn(ImportDataType._String, true, true));//触发方式
            this.ColumnValidList.Add(ColumnNameList[5], new ValidColumn(ImportDataType._String, true, true));//最低安全水位
            this.ColumnValidList.Add(ColumnNameList[6], new ValidColumn(ImportDataType._Enum, true, true));//需求计算方式
            this.ColumnValidList.Add(ColumnNameList[7], new ValidColumn(ImportDataType._String, false, true));//最高存量
            this.ColumnValidList.Add(ColumnNameList[8], new ValidColumn(ImportDataType._String, false, true));//固定量
            return this;
        }

        /// <summary>
        /// 验证物料编码
        /// </summary>
        /// <param name="obj">物料编码</param>
        /// <param name="messageTip">信息提示</param>
        /// <param name="dr">当前行数据</param>
        /// <returns>是否验证通过</returns>
        private bool VaildItemCode(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            if (dicItem == null)
            {
                dicItem = new Dictionary<string, Item>();
            }
            string itemCode = obj.ToString();
            messageTip = string.Empty;

            if (!itemCode.IsNullOrEmpty() && !dicItem.ContainsKey(itemCode))
            {
                var item = RT.Service.Resolve<ItemController>().GetAllEnableItems(null, itemCode, ConsumeMode.Pull).FirstOrDefault();

                if (item == null)
                {
                    messageTip = "不存在于系统".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    isValid = false;
                    return isValid;
                }
                dicItem.Add(itemCode, item);
            }
            return isValid;
        }

        /// <summary>
        /// 验证仓库编码
        /// </summary>
        /// <param name="obj">仓库编码</param>
        /// <param name="messageTip">信息提示</param>
        /// <param name="dr">当前行数据</param>
        /// <returns>是否验证通过</returns>
        private bool VaildWarehouseCode(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            if (dicWarehouse == null)
            {
                dicWarehouse = new Dictionary<string, Warehouse>();
            }

            string warehouseCode = obj.ToString();
            string itemCategoryCode = dr[1].ToString();
            string itemCode = dr[2].ToString();
            string itemExtPropName = dr[3].ToString();
            string triggerType = dr[4].ToString();
            messageTip = string.Empty;

            string uniquenessData = warehouseCode + "-" + itemCategoryCode + "-" + itemCode + "-" + itemExtPropName + "-" + triggerType;

            if (!dicWarehouse.ContainsKey(uniquenessData))
            {
                var warehouse = RT.Service.Resolve<LinesideWarehouseController>().GeWarehouses(null, warehouseCode).FirstOrDefault();
                if (warehouse == null)
                {
                    messageTip = "不存在于产线线边仓".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    isValid = false;
                    return isValid;
                }
                dicWarehouse.Add(uniquenessData, warehouse);
            }
            else
            {
                messageTip = "+物料类型+物料编码+物料扩展属性+触发方式存在重复".L10N();
                AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                isValid = false;
            }
            return isValid;
        }

        /// <summary>
        /// 验证物料类型编码
        /// </summary>
        /// <param name="obj">物料类型编码</param>
        /// <param name="messageTip">信息提示</param>
        /// <param name="dr">当前行数据</param>
        /// <returns>是否验证通过</returns>
        private bool VaildItemCategoryCode(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            if (dicItemCategory == null)
            {
                dicItemCategory = new Dictionary<string, ItemCategory>();
            }
            string itemCategoryCode = obj.ToString();
            messageTip = string.Empty;

            if (!itemCategoryCode.IsNullOrEmpty() && !dicItemCategory.ContainsKey(itemCategoryCode))
            {
                var itemCategory = RT.Service.Resolve<ItemController>().GetItemSmallCategory(CategoryType.Item, null, itemCategoryCode, null).FirstOrDefault();

                if (itemCategory == null)
                {
                    messageTip = "不存在于系统".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    isValid = false;
                    return isValid;
                }
                dicItemCategory.Add(itemCategoryCode, itemCategory);
            }
            return isValid;
        }

        /// <summary>
        /// 给保存错误的数据行记录错误数据信息
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="columnName">列名</param>
        /// <param name="errorMsg">错误信息</param>
        private void AppendErrorMsg(DataRow row, string columnName, string errorMsg)
        {
            row[columnName] += errorMsg;
        }

        /// <summary>
        /// 业务数据处理
        /// </summary>
        /// <param name="drs">数据集合</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length == 0) return;
            var mainDataList = from g in drs
                               select new
                               {
                                   WarehouseCode = g.Field<string>(ColIndex("仓库编码")),
                                   ItemCategoryCode = g.Field<string>(ColIndex("物料类型")),
                                   ItemCode = g.Field<string>(ColIndex("物料编码")),
                                   ItemExtPropName = g.Field<string>(ColIndex("物料扩展属性")),
                                   TriggerType = g.Field<string>(ColIndex("触发方式")),
                                   LowestStage = g.Field<string>(ColIndex("最低安全水位")),
                                   DemandType = g.Field<string>(ColIndex("需求计算方式")),
                                   MaxStock = g.Field<string>(ColIndex("最高存量")),
                                   FixedQuantity = g.Field<string>(ColIndex("固定量")),
                                   DetailInfo = g
                               };
            List<string> itemCodeList = new List<string>();
            mainDataList.ForEach(data =>
            {
                itemCodeList.Add(data.ItemCode);
            });
            // 物料
            var itemList = RT.Service.Resolve<ItemLabelController>().GetItemByCode(itemCodeList);
            // 物料扩展属性子表
            var itemExtPropList = RT.Service.Resolve<ItemController>().GetItemPropertyValueList(itemCodeList);
            // 属性定义ids
            var definitionIds = itemExtPropList.Select(x => x.DefinitionId).ToList();
            // 属性列表
            var definitionList = RT.Service.Resolve<ItemController>().GetItemPropertyDefinitionList(definitionIds);
            var catalogTypeIds = definitionList.Where(p => p.PropertyType == ItemPropertyType.Catalog).Select(p => p.CatalogTypeId).ToList();
            // 快码明细
            var catalogList = RT.Service.Resolve<ItemController>().GetCatalogList(catalogTypeIds);
            // 扩展属性错误信息
            var itemExtPropErrorMsg = string.Empty;
            foreach (var mainDataItem in mainDataList)
            {
                try
                {
                    var vali = mainDataItem.WarehouseCode + "-" + mainDataItem.ItemCategoryCode + "-" + mainDataItem.ItemCode + "-" + mainDataItem.ItemExtPropName + "-" + mainDataItem.TriggerType;
                    var prepareItemPull = new PrepareItemPull();
                    prepareItemPull.WarehouseId = dicWarehouse != null && dicWarehouse.ContainsKey(vali) ? dicWarehouse[vali].Id : 0;
                    prepareItemPull.ItemCategoryId = dicItemCategory != null && dicItemCategory.ContainsKey(mainDataItem.ItemCategoryCode) ? dicItemCategory?[mainDataItem.ItemCategoryCode].Id : 0;
                    prepareItemPull.ItemId = dicItem?[mainDataItem.ItemCode]?.Id;
                    if (mainDataItem.TriggerType == TriggerMode.BelowSafe.ToLabel())
                    {
                        prepareItemPull.TriggerType = TriggerMode.BelowSafe;
                    }
                    else if (mainDataItem.TriggerType == TriggerMode.ManualModel.ToLabel())
                    {
                        prepareItemPull.TriggerType = TriggerMode.ManualModel;
                    }
                    if (!mainDataItem.LowestStage.IsNullOrEmpty())
                    {
                        Decimal.TryParse(mainDataItem.LowestStage, out decimal lowestStage);
                        prepareItemPull.LowestStage = lowestStage;
                    }
                    if (mainDataItem.ItemCode.IsNotEmpty())
                    {
                        var item = itemList.FirstOrDefault(p => p.Code == mainDataItem.ItemCode);
                        if (item.EnableExtendProperty)
                        {
                            // 物料扩展验证
                            prepareItemPull.ItemExtProp = ExtPropIsActive(itemExtPropList, definitionList, catalogList, item, mainDataItem.ItemExtPropName, out itemExtPropErrorMsg);
                            prepareItemPull.ItemExtPropName = mainDataItem.ItemExtPropName;
                        }
                        else
                        {
                            prepareItemPull.ItemExtProp = "";
                            prepareItemPull.ItemExtPropName = "";
                        }
                        if (itemExtPropErrorMsg.IsNotEmpty())
                        {
                            AppendErrorMsg(mainDataItem.DetailInfo, ImportDataHandle.MessageColumnName, itemExtPropErrorMsg);
                            continue;
                        }
                    }
                    if (prepareItemPull.ItemCategoryId.HasValue || prepareItemPull.ItemId.HasValue)
                    {
                        var pull = RT.Service.Resolve<PrepareItemController>().GetPrepareItemPull(prepareItemPull.WarehouseId, prepareItemPull.ItemCategoryId, prepareItemPull.ItemId, prepareItemPull.TriggerType, prepareItemPull.ItemExtPropName);
                        if (pull != null)
                        {
                            throw new ValidationException("仓库编码+物料类型+物料编码+物料扩展属性+触发方式存在重复;".L10N());
                        }
                    }


                    if (mainDataItem.DemandType == DemandMode.MaxStock.ToLabel())
                    {
                        prepareItemPull.DemandType = DemandMode.MaxStock;
                    }
                    else if (mainDataItem.DemandType == DemandMode.FixedQuantity.ToLabel())
                    {
                        prepareItemPull.DemandType = DemandMode.FixedQuantity;
                    }
                    if (!mainDataItem.MaxStock.IsNullOrEmpty())
                    {
                        Decimal.TryParse(mainDataItem.MaxStock, out decimal maxStock);
                        prepareItemPull.MaxStock = maxStock;
                    }
                    if (!mainDataItem.FixedQuantity.IsNullOrEmpty())
                    {
                        Decimal.TryParse(mainDataItem.FixedQuantity, out decimal fixedQuantity);
                        prepareItemPull.FixedQuantity = fixedQuantity;
                    }
                    prepareItemPull.PrepareItemType = PrepareItemType.Pull;

                    RF.Save(prepareItemPull);
                }
                catch (Exception ex)
                {
                    string strMsg = AppRuntime.Location.ConnectDataDirectly ? ex.Message : ex.InnerException.Message;
                    AppendErrorMsg(mainDataItem.DetailInfo, ImportDataHandle.MessageColumnName, strMsg);
                }
            }
        }

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        protected virtual int ColIndex(string columnName)
        {
            return ColumnNameList.IndexOf(columnName);
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            if (dicWarehouse != null)
            {
                dicWarehouse.Clear();
                dicWarehouse = null;
            }
            if (dicItemCategory != null)
            {
                dicItemCategory.Clear();
                dicItemCategory = null;
            }
            if (dicItem != null)
            {
                dicItem.Clear();
                dicItem = null;
            }
        }

        /// <summary>
        /// 物料扩展属性验证
        /// </summary>
        /// <param name="itemExtPropList">物料扩展属性列表</param>
        /// <param name="definitionList">物料属性定义</param>
        /// <param name="catalogList">快码列表</param>
        /// <param name="item">物料</param>
        /// <param name="itemExtPropName">物料扩展属性</param>
        /// <param name="itemExtPropErrorMsg">物料扩展属性错误信息</param>
        private string ExtPropIsActive(EntityList<ItemPropertyValue> itemExtPropList, EntityList<ItemPropertyDefinition> definitionList, EntityList<Catalog> catalogList, Items.Item item, string itemExtPropName, out string itemExtPropErrorMsg)
        {
            if (item.EnableExtendProperty && itemExtPropName.IsNullOrEmpty())
            {
                itemExtPropErrorMsg = "物料需启用扩展属性;";
                return string.Empty;
            }
            else
            {

                // 获取属性定义Ids
                var itemExtPropDefIds = itemExtPropList.Where(p => p.ItemId == item.Id).Select(p => p.DefinitionId).Distinct().ToList();
                var itemDefinitionList = definitionList.Where(p => itemExtPropDefIds.Contains(p.Id)).ToList();
                // 属性名称
                var definitionNames = itemDefinitionList.Select(p => p.Name).Distinct().ToList();
                // 导入数据的物料扩展属性名称
                var proNames = itemExtPropName.Split(';').ToList();
                if (definitionNames.Count != proNames.Count)
                {
                    itemExtPropErrorMsg = "物料扩展属性不全或多余;";
                    return string.Empty;
                }
                else
                {
                    // 物料拓展属性是否合格
                    var check = true;
                    foreach (var pro in proNames)
                    {
                        string key = string.Empty;
                        string value = string.Empty;
                        try
                        {
                            key = pro.Split(':')[0];
                            value = pro.Split(':')[1];
                        }
                        catch
                        {
                            check = false;
                            break;
                        }
                        var itemDefinition = itemDefinitionList.Find(p => p.Name == key);

                        if (itemDefinition != null)
                        {
                            if (itemDefinition.PropertyType == ItemPropertyType.Catalog)
                            {
                                var itemCata = catalogList.FirstOrDefault(p => p.Code == value && p.CatalogTypeId == itemDefinition.CatalogTypeId);
                                if (itemCata == null)
                                {
                                    check = false;
                                    break;

                                }
                            }
                            else
                            {
                                var itemExtPropValue = itemExtPropList.Where(p => p.ItemId == item.Id && p.DefinitionId == itemDefinition.Id && p.Value == value).ToList();
                                if (!itemExtPropValue.Any())
                                {
                                    check = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            check = false;
                            break;

                        }
                    }
                    if (check)
                    {
                        var exProp = string.Empty;
                        proNames.ForEach(proName =>
                        {
                            var keyId = itemDefinitionList.Find(p => p.Name == proName.Split(':')[0])?.Id.ToString();
                            if (exProp.IsNotEmpty())
                            {
                                exProp += ';';
                            }
                            exProp += keyId + ':' + proName.Split(':')[1];
                        });
                        itemExtPropErrorMsg = string.Empty;
                        return exProp;
                    }
                    else
                    {
                        itemExtPropErrorMsg = "物料扩展属性不合法;";
                        return string.Empty;
                    }
                }
            }
        }
    }
}
