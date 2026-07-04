using Microsoft.Extensions.FileSystemGlobbing.Internal;
using SIE.Common.Catalogs;
using SIE.Common.ImportHelper;
using SIE.Core.Labels;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.Resources.Enterprises;
using SIE.Utils;
using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 物料标签导入
    /// </summary>
    [Services.Service(FallbackType = typeof(ItemLabelHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ItemLabelHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "行号","标签号","物料编码","物料扩展属性","可用数量","批次号","供应商编码","条码信息来源","工厂编码","生产日期","库存地"//,"明细-工单号","明细-数量",
        };

        #region 基础数据
        /// <summary>
        /// 行号
        /// </summary>
        private List<string> LineNoList { get; set; }
        #endregion


        /// <summary>
        /// 列标准验证
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建导入数据列验证
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (LineNoList != null)
            {
                LineNoList.Clear();
                LineNoList = null;
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
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }



        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            // Method intentionally left empty.
            var importDataList = from g in drs
                                 select new
                                 {
                                     LineNo = g.Field<string>(ColIndex("行号")).Trim(),
                                     Label = g.Field<string>(ColIndex("标签号")).Trim(),
                                     ItemCode = g.Field<string>(ColIndex("物料编码")).Trim(),
                                     ItemExtPropName = g.Field<string>(ColIndex("物料扩展属性")).Trim(),
                                     Qty = g.Field<string>(ColIndex("可用数量")).Trim(),
                                     Lot = g.Field<string>(ColIndex("批次号")).Trim(),
                                     SupplierCode = g.Field<string>(ColIndex("供应商编码")).Trim(),
                                     SourceType = g.Field<string>(ColIndex("条码信息来源")).Trim(),
                                     FactoryCode = g.Field<string>(ColIndex("工厂编码")).Trim(),
                                     ProductionDate = g.Field<string>(ColIndex("生产日期")).Trim(),
                                     //DetailWoNo = g.Field<string>(ColIndex("明细-工单号")).Trim(),
                                     //DetailQty = g.Field<string>(ColIndex("明细-数量")).Trim(),
                                     Lgort = g.Field<string>(ColIndex("库存地")).Trim(),
                                     DetailInfo = g,
                                 };
            // 物料编码 工厂编码 工单号 供应商编码 标签号
            List<string> itemCodeList = new List<string>();
            List<string> factoryCodeList = new List<string>();
            List<string> woNoList = new List<string>();
            List<string> supplierCodeList = new List<string>();
            List<string> labelList = new List<string>();
            // Tuple(标签号, 物料编码)
            List<Tuple<string, string>> labelItemCodeList = new List<Tuple<string, string>>();
            importDataList.ForEach(data =>
            {
                labelList.Add(data.Label);
                itemCodeList.Add(data.ItemCode);
                factoryCodeList.Add(data.FactoryCode);
                //woNoList.Add(data.DetailWoNo);
                supplierCodeList.Add(data.SupplierCode);
                labelItemCodeList.Add(Tuple.Create(data.Label, data.ItemCode));               
            });
            itemCodeList = itemCodeList.Distinct().ToList();
            factoryCodeList = factoryCodeList.Distinct().ToList();
            woNoList = woNoList.Distinct().ToList();
            supplierCodeList = supplierCodeList.Distinct().ToList();
            // 物料
            var itemList = RT.Service.Resolve<ItemLabelController>().GetItemByCode(itemCodeList);
            // 工厂
            var factoryList = RT.Service.Resolve<ItemLabelController>().FactoryIsExists(factoryCodeList);
            // 物料标签
            var itemLabelList = RT.Service.Resolve<ItemLabelController>().GetItemLabelList(itemCodeList);
            var itemLabelIds = itemLabelList.Select(p => p.Id).ToList();
            // 物料标签投入工单
            var itemLabelWoOrderList = RT.Service.Resolve<ItemLabelController>().GetItemLabelWorkOrders(itemLabelIds);
            // 库存
            var itemStockBaseList = RT.Service.Resolve<ItemLabelController>().GetItemStockDataBaseList(itemCodeList);
            // 工单(非关闭、非完工)
            var woOrderList = RT.Service.Resolve<ItemLabelController>().GetWorkOrderList(woNoList);
            // 供应商
            var supplierList = RT.Service.Resolve<ItemLabelController>().GetSupplierList(supplierCodeList);
            // 物料扩展属性子表
            var itemExtPropList = RT.Service.Resolve<ItemController>().GetItemPropertyValueList(itemCodeList);
            // 属性定义ids
            var definitionIds = itemExtPropList.Select(x => x.DefinitionId).ToList();
            // 属性列表
            var definitionList = RT.Service.Resolve<ItemController>().GetItemPropertyDefinitionList(definitionIds);
            var catalogTypeIds = definitionList.Where(p => p.PropertyType == ItemPropertyType.Catalog).Select(p => p.CatalogTypeId).ToList();
            // 快码明细
            var catalogList = RT.Service.Resolve<ItemController>().GetCatalogList(catalogTypeIds);


            // 通过验证的物料标签主表
            EntityList<ItemLabel> saveItemLabels = new EntityList<ItemLabel>();
            // 通过验证的物料标签关联工单子表
            EntityList<ItemLabelWorkOrder> saveItemLabelWoOrders = new EntityList<ItemLabelWorkOrder>();
            var importDataRows = importDataList.ToList();
            // 记录写入的物料标签
            ItemLabel itemLabel = new ItemLabel();

            for (int i = 0; i < importDataRows.Count; i++)
            {
                // 是否通过
                bool pass = true;
                // 错误信息
                string errorMsg = string.Empty;
                // 是否需要创建新的主表
                bool needNewParent = true;
                // 是否序列号管控
                bool? isSerialNumber = null;
                // 是否批次管控
                bool? isBatch = null;
                // 物料扩展属性
                var itemExtProp = string.Empty;
                // 生产时间
                DateTime productTime = new DateTime();
                // 批次
                string lot = string.Empty;


                if (i == 0 && importDataRows[0].Label.IsNullOrEmpty())
                {
                    ImportExtension.BatchAppendText(new List<DataRow>() { importDataRows[i].DetailInfo }, ImportDataHandle.MessageColumnName, "首行数据物料标签不能为空！");
                    continue;
                }

                var item = itemList.FirstOrDefault(p => p.Code == importDataRows[i].ItemCode);
                var factory = factoryList.FirstOrDefault(p => p.Code == importDataRows[i].FactoryCode);
                var labelIsRepeat = RT.Service.Resolve<ItemLabelController>().SerItemLabelRepeat(labelItemCodeList, item, itemStockBaseList, itemLabelList, importDataRows[i].Label, out isSerialNumber, out isBatch);
                //var woOrder = woOrderList.FirstOrDefault(p => p.No == importDataRows[i].DetailWoNo);
                var supplier = supplierList.FirstOrDefault(p => p.Code == importDataRows[i].SupplierCode);

                if (importDataRows[i].Label.IsNullOrEmpty() && importDataRows[i].ItemCode.IsNullOrEmpty()) // 物料标签和物料编码都为空，视作上一行子表
                {
                    needNewParent = false;
                }
                // 验证实体
                ValidateRow validateRow = new ValidateRow
                {
                    LineNo = importDataRows[i].LineNo,
                    Qty = importDataRows[i].Qty,
                    FactoryCode = importDataRows[i].FactoryCode,
                    SupplierCode = importDataRows[i].SupplierCode,
                    ProductionDate = importDataRows[i].ProductionDate,
                    //DetailWoNo = importDataRows[i].DetailWoNo,
                    //DetailQty = importDataRows[i].DetailQty,
                    ItemExtPropName = importDataRows[i].ItemExtPropName,
                    Lot = importDataRows[i].Lot,
                    SourceType = importDataRows[i].SourceType,
                };
                if (needNewParent)
                {
                    // 验证主表字段
                    ParentFieldVerification(validateRow, item, labelIsRepeat, isBatch, factory, supplier, itemExtPropList, definitionList, catalogList, ref pass, ref errorMsg, ref itemExtProp, ref productTime, ref lot);
                }

                // 验证子表字段
                //ChildFieldVerification(validateRow, woOrder, ref pass, ref errorMsg);
                if (pass)
                {
                    if (needNewParent)
                    {
                        itemLabel = new ItemLabel
                        {
                            Label = importDataRows[i].Label,
                            ItemId = item.Id,
                            Item = item,
                            ItemExtProp = itemExtProp,
                            ItemExtPropName = itemExtProp.IsNotEmpty() ? importDataRows[i].ItemExtPropName : string.Empty,
                            Qty = decimal.Parse(importDataRows[i].Qty),
                            Lot = lot,
                            FactoryId = factory?.Id,
                            Factory = factory,
                            SupplierId = supplier?.Id,
                            Supplier = supplier,
                            IsSerialNumber = isSerialNumber,
                            SourceType = importDataRows[i].SourceType.Trim().IsNullOrEmpty()? LabelSource.Import:(LabelSource)EnumViewModel.LabelToEnum(importDataRows[i].SourceType, typeof(LabelSource)),
                            ProductionDate = productTime,
                            ItemLabelState = ItemLabelState.Receive,
                            Lgort = importDataRows[i].Lgort,
                        };
                        var dataBaseItemLabel = itemLabelList.FirstOrDefault(p => p.Label == itemLabel.Label &&
                        p.ItemId == itemLabel.ItemId && p.ItemExtProp == itemLabel.ItemExtProp && p.Lot == itemLabel.Lot &&
                        p.FactoryId == itemLabel.FactoryId && p.SupplierId == itemLabel.SupplierId && p.IsSerialNumber == itemLabel.IsSerialNumber &&
                        p.ProductionDate == itemLabel.ProductionDate);
                        if (dataBaseItemLabel != null)
                        {
                            itemLabel = dataBaseItemLabel;
                            itemLabel.Qty += decimal.Parse(importDataRows[i].Qty);
                        }
                        if (itemLabel.InitialQty == null || itemLabel.InitialQty == 0)
                            itemLabel.InitialQty = itemLabel.Qty;
                        saveItemLabels.Add(itemLabel);
                    }
                    //if (validateRow.DetailWoNo.IsNotEmpty() && validateRow.DetailQty.IsNotEmpty())
                    //{
                    //    ItemLabelWorkOrder itemLabelWorkOrder = new ItemLabelWorkOrder
                    //    {
                    //        ItemLabelId = itemLabel.Id,
                    //        ItemLabel = itemLabel,
                    //        WorkOrderId = woOrder.Id,
                    //        WorkOrder = woOrder,
                    //        Qty = decimal.Parse(importDataRows[i].DetailQty),
                    //    };
                    //    var dataBaseItemLabelWoOrder = itemLabelWoOrderList.FirstOrDefault(p => p.ItemLabelId == itemLabel.Id && p.WorkOrderId == itemLabelWorkOrder.WorkOrderId);
                    //    if (dataBaseItemLabelWoOrder != null)
                    //    {
                    //        itemLabelWorkOrder = dataBaseItemLabelWoOrder;
                    //        itemLabelWorkOrder.Qty += decimal.Parse(importDataRows[i].DetailQty);
                    //    }
                    //    saveItemLabelWoOrders.Add(itemLabelWorkOrder);
                    //}
                }
                else // 数据验证不通过
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] = errorMsg;
                }
            }
            MergeAndSaveItemLabels(saveItemLabels, saveItemLabelWoOrders);
        }

        /// <summary>
        /// 合并条码并保存
        /// </summary>
        /// <param name="saveItemLabels"></param>
        /// <param name="saveItemLabelWoOrders"></param>
        private void MergeAndSaveItemLabels(EntityList<ItemLabel> saveItemLabels, EntityList<ItemLabelWorkOrder> saveItemLabelWoOrders)
        {
            
            // 导入表内和数据库合并
            using (var tran = DB.TransactionScope(PackageEntityDataProvider.ConnectionStringName))
            {
                // 批量保存
                RF.Save(saveItemLabels);
                //saveItemLabelWoOrders.ForEach(i =>
                //{
                //    i.ItemLabelId = i.ItemLabel.Id;
                //});
                //RF.Save(saveItemLabelWoOrders);
                tran.Complete();
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
                                var itemExtPropValue = itemExtPropList.First(p => p.ItemId == item.Id && p.DefinitionId == itemDefinition.Id&&p.Value== value)?.Value;
                                if (value != itemExtPropValue)
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

        /// <summary>
        /// 验证主表字段
        /// </summary>
        /// <param name="datarow">数据行</param>
        /// <param name="item">物料</param>
        /// <param name="labelIsRepeat">标签管控是否重复</param>
        /// <param name="isBatch">是否批次管控</param>
        /// <param name="factory">工厂</param>
        /// <param name="supplier">供应商</param>
        /// <param name="itemExtPropList">物料扩展属性列表</param>
        /// <param name="definitionList">物料属性定义</param>
        /// <param name="catalogList">快码列表</param>
        /// <param name="pass">是否通过</param>
        /// <param name="errorMsg">错误信息</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <param name="productTime">生产日期</param>
        /// <param name="lot">批次号</param>
        private void ParentFieldVerification(ValidateRow datarow, Item item, bool labelIsRepeat, bool? isBatch, Enterprise factory, Supplier supplier, EntityList<ItemPropertyValue> itemExtPropList, EntityList<ItemPropertyDefinition> definitionList, EntityList<Catalog> catalogList, ref bool pass, ref string errorMsg, ref string itemExtProp, ref DateTime productTime, ref string lot)
        {

            if (datarow.LineNo.IsNullOrEmpty())
            {
                errorMsg += "行号不为空;";
                pass = false;
            }
            // 验证物料是否存在
            if (item == null)
            {
                errorMsg += "物料不存在;";
                pass = false;
            }
            else
            {
                // 物料扩展属性
                if (!item.EnableExtendProperty && datarow.ItemExtPropName.IsNotEmpty())
                {
                    errorMsg += "物料未启用扩展属性;";
                    pass = false;
                }
                if (item.EnableExtendProperty)
                {
                    // 物料扩展验证
                    string itemExtPropErrorMsg = string.Empty;
                    itemExtProp = ExtPropIsActive(itemExtPropList, definitionList, catalogList, item, datarow.ItemExtPropName, out itemExtPropErrorMsg);
                    if (itemExtPropErrorMsg.IsNotEmpty())
                    {
                        errorMsg += itemExtPropErrorMsg;
                        pass = false;
                    }
                }
            }
            // 验证序列号管控
            if (labelIsRepeat)
            {
                errorMsg += "序列号管控物料标签重复;";
                pass = false;
            }
            if (datarow.Lot.Trim().IsNullOrEmpty())
            {
                errorMsg += "批次号不能为空;";
                pass = false;
            }
            else
            {
                lot = datarow.Lot;
            }
            //if (datarow.Lot.IsNullOrEmpty())
            //{
            //    lot = "LotDefault";
            //}
            //else
            //{
            //    lot = datarow.Lot;
            //}

            // 批次号验证
            //if (isBatch == true)
            //{
            //    if (datarow.Lot.IsNullOrEmpty())
            //    {
            //        errorMsg += "物料批次管控批次号不能为空;";
            //        pass = false;
            //    }
            //    else
            //    {
            //        lot = datarow.Lot;
            //    }
            //}
            //else
            //{
            //    lot = "LotDefault";
            //}
            if (datarow.Qty.IsNullOrEmpty())
            {
                errorMsg += "可用数量不为空;";
                pass = false;
            }
            else
            {
                decimal qty;
                var isNum = decimal.TryParse(datarow.Qty, out qty);
                if (!isNum)
                {
                    errorMsg += "可用数量输入必须为数字;";
                    pass = false;
                }
                if (isNum && qty <= 0)
                {
                    errorMsg += "可用数量输入必须为正数;";
                    pass = false;
                }
            }
            if (datarow.FactoryCode.IsNullOrEmpty())
            {
                errorMsg += "工厂编码不为空;";
                pass = false;
            }
            else
            {
                // 验证工厂是否存在
                if (factory == null)
                {
                    errorMsg += "工厂不存在;";
                    pass = false;
                }
            }
            // 验证供应商是否存在
            if (datarow.SupplierCode.IsNotEmpty() && supplier == null)
            {
                errorMsg += "供应商不存在;";
                pass = false;
            }
            // 验证日期
            // 时间格式验证
            if (DateTime.TryParse(datarow.ProductionDate, out DateTime dateTime))
            {
                productTime = dateTime;
            }
            else if (Double.TryParse(datarow.ProductionDate, out double timedouble))
            {
                productTime = DateTime.FromOADate(timedouble);
            }
            else
            {
                errorMsg += "生产日期格式不合法;";
                pass = false;
            }
            //验证条码来源类型
            if (datarow.SourceType != null)
            {
                try
                {
                    var SourceType = (LabelSource?)EnumViewModel.LabelToEnum(datarow.SourceType, typeof(LabelSource));
                    if (SourceType == null)
                    {
                        errorMsg += "条码来源信息输入错误";
                        pass = false;
                    }
                }
                catch (Exception ex)
                {
                    errorMsg += "条码来源信息输入错误";
                    pass = false;
                }
            }
        }

        /// <summary>
        /// 验证子表字段
        /// </summary>
        /// <param name="datarow">数据行</param>
        /// <param name="woOrder">工单</param>
        /// <param name="pass">是否通过</param>
        /// <param name="errorMsg">错误信息</param>
        private void ChildFieldVerification(ValidateRow datarow, Core.WorkOrders.WorkOrder woOrder, ref bool pass, ref string errorMsg)
        {
            // 验证工单是否存在
            if (datarow.DetailWoNo.IsNotEmpty())
            {
                if (woOrder == null)
                {
                    errorMsg += "工单不存在(工单状态为完工或关闭);";
                    pass = false;
                }
                else
                {
                    if (datarow.DetailQty.IsNotEmpty())
                    {
                        decimal detailQty;
                        var isNum = decimal.TryParse(datarow.DetailQty, out detailQty);
                        if (!isNum)
                        {
                            errorMsg += "可用数量输入必须为数字;";
                            pass = false;
                        }
                        if (isNum && detailQty <= 0)
                        {
                            errorMsg += "可用数量输入必须为正数;";
                            pass = false;
                        }
                    }
                    else
                    {
                        errorMsg += "明细数量不能为空;";
                        pass = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 验证实体
    /// </summary>
    [Serializable]
    public class ValidateRow
    {
        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Qty { get; set; }

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public string ProductionDate { get; set; }

        /// <summary>
        /// 明细工单
        /// </summary>
        public string DetailWoNo { get; set; }

        /// <summary>
        /// 明细数量
        /// </summary>
        public string DetailQty { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// 条码来源信息
        /// </summary>
        public string SourceType { get; set; }
    }
}
