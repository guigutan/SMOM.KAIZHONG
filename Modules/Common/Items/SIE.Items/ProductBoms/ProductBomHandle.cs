using SIE.Common;
using SIE.Common.Catalogs;
using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Items.ProductBoms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Items
{
    /// <summary>
    /// 产品BOM导入
    /// </summary>
    [Services.Service(FallbackType = typeof(ProductBomHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ProductBomHandle : IDisposable, IBusinessImport
    {

        private const string bomCode = "BOM编码";
        private const string bomName = "BOM名称";
        private const string productCode = "产品编码";
        private const string itemExtValues = "扩展属性值";
        private const string isDefault = "是否默认";
        private const string sourceType = "数据来源类型";
        private const string version = "版本号";
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            bomCode.L10N(),bomName.L10N(),productCode.L10N(),itemExtValues.L10N(),isDefault.L10N(),sourceType.L10N(),version.L10N()
        };

        #region 基础数据


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
            if (ColumnValidList != null)
            {
                ColumnValidList.Clear();
                ColumnValidList = null;
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
        #endregion


        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            var importDataList = from g in drs// "BOM编码","BOM名称","产品编码","扩展属性值","是否默认","数据来源类型"
                                 select new
                                 {
                                     BomCode = g.Field<string>(ColIndex(bomCode.L10N())).Trim(),
                                     BomName = g.Field<string>(ColIndex(bomName.L10N())).Trim(),
                                     ProductCode = g.Field<string>(ColIndex(productCode.L10N())).Trim(),
                                     ItemExtValues = g.Field<string>(ColIndex(itemExtValues.L10N())).Trim(),
                                     IsDefault = g.Field<string>(ColIndex(isDefault.L10N())).Trim(),
                                     SourceType = g.Field<string>(ColIndex(sourceType.L10N())).Trim(),
                                     Vision = g.Field<string>(ColIndex(version.L10N())).Trim(),
                                     DetailInfo = g,
                                 };
            // BOM编码    
            List<string> bomCodeList = new List<string>();
            List<string> itemCodeList = new List<string>();
            importDataList.ForEach(data =>
            {
                bomCodeList.Add(data.BomCode);
                itemCodeList.Add(data.ProductCode);
            });

            bomCodeList = bomCodeList.Distinct().ToList();
            var dbBomCodeList = RT.Service.Resolve<ProductBomController>().GetProductBomByCodes(bomCodeList);
            var itemList = RT.Service.Resolve<ItemController>().GetItemsIdByCode(itemCodeList);
            // 物料扩展属性子表
            var itemExtPropList = RT.Service.Resolve<ItemController>().GetItemPropertyValueList(itemCodeList);
            // 属性定义ids
            var definitionIds = itemExtPropList.Select(x => x.DefinitionId).ToList();
            //属性列表
            var definitionList = RT.Service.Resolve<ItemController>().GetItemPropertyDefinitionList(definitionIds);
            var catalogTypeIds = definitionList.Where(p => p.PropertyType == ItemPropertyType.Catalog).Select(p => p.CatalogTypeId).ToList();
            // 快码明细
            var catalogList = RT.Service.Resolve<ItemController>().GetCatalogList(catalogTypeIds);

            EntityList<ProductBom> saveProductBoms = new EntityList<ProductBom>();
            var importDataRows = importDataList.ToList();
            for (int i = 0; i < importDataRows.Count; i++)
            {
                // 是否通过
                bool pass = true;
                // 错误信息
                string errorMsg = string.Empty;

                // 物料扩展属性
                var itemExtProp = string.Empty;
                var IsDefault = false;
                SourceType sourceType = SourceType.Internal;
                if (importDataRows[i].BomCode.IsNullOrEmpty())
                {
                    ImportExtension.BatchAppendText(new List<DataRow>() { importDataRows[i].DetailInfo }, ImportDataHandle.MessageColumnName, "数据[产品BOM]不能为空！".L10N());
                    continue;
                }

                var item = itemList.FirstOrDefault(p => p.Code == importDataRows[i].ProductCode);
                // 验证实体
                ValidateRow validateRow = new ValidateRow
                {
                    BomCode = importDataRows[i].BomCode,
                    BomName = importDataRows[i].BomName,
                    ProductCode = importDataRows[i].ProductCode,
                    IsDefault = importDataRows[i].IsDefault,
                    ItemExtValues = importDataRows[i].ItemExtValues,
                    SourceType = importDataRows[i].SourceType,
                    Version = importDataRows[i].Vision
                };
                // 验证主表字段
                FieldVerification(validateRow, item, itemExtPropList, definitionList, catalogList, dbBomCodeList, ref pass, ref errorMsg, ref itemExtProp, ref IsDefault, ref sourceType);

                if (!pass)
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] = errorMsg;
                }
                else
                {
                    ProductBom validityStandard = new ProductBom()
                    {
                        ProductId = item.Id,
                        ItemExtPropName = itemExtProp,
                        SourceType = sourceType,
                        Code = validateRow.BomCode,
                        Name = validateRow.BomName,
                        IsDefault = IsDefault,
                        Version = validateRow.Version,
                    };
                    saveProductBoms.Add(validityStandard);
                }
            }
            RF.Save(saveProductBoms);
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
        private string ExtPropIsActive(EntityList<ItemPropertyValue> itemExtPropList, EntityList<ItemPropertyDefinition> definitionList,
            EntityList<Catalog> catalogList, Item item,
            string itemExtPropName, out string itemExtPropErrorMsg)
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
                                var itemExtPropValue = itemExtPropList.First(p => p.ItemId == item.Id && p.DefinitionId == itemDefinition.Id && p.Value == value)?.Value;
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


        private void FieldVerification(ValidateRow datarow, Item item, EntityList<ItemPropertyValue> itemExtPropList, EntityList<ItemPropertyDefinition> definitionList,
            EntityList<Catalog> catalogList, EntityList<ProductBom> dbBomCodeList,
            ref bool pass, ref string errorMsg,
            ref string itemExtProp,
            ref bool IsDefault, ref SourceType sourceType)
        {
            if (datarow.ProductCode.IsNullOrEmpty())
            {
                errorMsg += "产品编码不能为空".L10N();
                pass = false;
            }
            if (datarow.BomName.IsNullOrEmpty())
            {
                errorMsg += "产品BOM名称不能为空".L10N();
                pass = false;
            }

            if (dbBomCodeList.Any(m => m.Code == datarow.BomCode))
            {
                errorMsg += "产品BOM系统已存在，请勿重复导入".L10N();
                pass = false;
            }
            // 验证物料是否存在
            if (item == null)
            {
                errorMsg += "产品[{0}]不存在".L10nFormat(datarow.ProductCode);
                pass = false;
            }
            else
            {
                // 物料扩展属性
                if (!item.EnableExtendProperty && datarow.ItemExtValues.IsNotEmpty())
                {
                    errorMsg += "物料未启用扩展属性".L10N();
                    pass = false;
                }
                if (item.EnableExtendProperty)
                {
                    // 物料扩展验证
                    string itemExtPropErrorMsg = string.Empty;
                    itemExtProp = ExtPropIsActive(itemExtPropList, definitionList, catalogList, item, datarow.ItemExtValues, out itemExtPropErrorMsg);
                    if (itemExtPropErrorMsg.IsNotEmpty())
                    {
                        errorMsg += itemExtPropErrorMsg;
                        pass = false;
                    }
                }
            }


            // 验证生效时间不能为空
            if (datarow.BomName.IsNullOrEmpty())
            {
                errorMsg += "产品BOM名称不能为空;".L10N();
                pass = false;
            }


            if (datarow.IsDefault.IsNotEmpty())
            {
                // 验证日期
                // 时间格式验证
                if (bool.TryParse(datarow.IsDefault, out bool isDefault))
                {
                    IsDefault = isDefault;
                }
                else
                {
                    errorMsg += "是否默认格式不合法;";
                    pass = false;
                }
            }
            if (datarow.SourceType.IsNotEmpty())
            {
                // 验证日期
                // 时间格式验证
                if (SourceType.TryParse(datarow.SourceType, out SourceType inputSourceType))
                {
                    sourceType = inputSourceType;
                }
                else
                {
                    errorMsg += "来源类型格式不合法;";
                    pass = false;
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
        /// 产品BOM编码
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        /// 产品BOM名称
        /// </summary>
        public string BomName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtValues { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public string IsDefault { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
    }
}
