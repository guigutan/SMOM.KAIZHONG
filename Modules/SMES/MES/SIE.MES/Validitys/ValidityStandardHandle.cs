using Microsoft.Extensions.FileSystemGlobbing.Internal;
using SIE.Common.Catalogs;
using SIE.Common.ImportHelper;
using SIE.Core.Labels;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MES.Validitys.Helpers;
using SIE.Packages.ItemLabels;
using SIE.Resources.Enterprises;
using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SIE.MES.Validitys
{
    /// <summary>
    /// 物料标签导入
    /// </summary>
    [Services.Service(FallbackType = typeof(ValidityStandardHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ValidityStandardHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "物料编码","物料扩展属性","可使用时长寿命(H)","生效日期","失效日期"
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
            var importDataList = from g in drs
                                 select new
                                 {
                                     ItemCode = g.Field<string>(ColIndex("物料编码")).Trim(),
                                     ItemExtPropName = g.Field<string>(ColIndex("物料扩展属性")).Trim(),
                                     LongLived = g.Field<string>(ColIndex("可使用时长寿命(H)")).Trim(),
                                     Effective = g.Field<string>(ColIndex("生效日期")).Trim(),
                                     Expiration = g.Field<string>(ColIndex("失效日期")).Trim(),
                                     DetailInfo = g,
                                 };
            // 物料编码    
            List<string> itemCodeList = new List<string>();
            // Tuple(标签号, 物料编码)
            List<Tuple<string, string>> labelItemCodeList = new List<Tuple<string, string>>();
            importDataList.ForEach(data =>
            {
                itemCodeList.Add(data.ItemCode);
            });
            itemCodeList = itemCodeList.Distinct().ToList();
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

            EntityList<ValidityStandard> saveValidityStandards = new EntityList<ValidityStandard>();
            var importDataRows = importDataList.ToList();
            for (int i = 0; i < importDataRows.Count; i++)
            {
                // 是否通过
                bool pass = true;
                // 错误信息
                string errorMsg = string.Empty;

                // 物料扩展属性
                var itemExtProp = string.Empty;
                // 生效时间
                DateTime effective = new DateTime();
                // 失效时间
                DateTime? expiration = null;
                decimal longLived = 0;

                if (i == 0 && importDataRows[0].ItemCode.IsNullOrEmpty())
                {
                    ImportExtension.BatchAppendText(new List<DataRow>() { importDataRows[i].DetailInfo }, ImportDataHandle.MessageColumnName, "数据物料编码不能为空！");
                    continue;
                }

                var item = itemList.FirstOrDefault(p => p.Code == importDataRows[i].ItemCode);
                // 验证实体
                ValidateRow validateRow = new ValidateRow
                {
                    ItemCode = importDataRows[i].ItemCode,
                    LongLived = importDataRows[i].LongLived,
                    Effective = importDataRows[i].Effective,
                    Expiration = importDataRows[i].Expiration,
                    ItemExtPropName = importDataRows[i].ItemExtPropName,
                };
                // 验证主表字段
                FieldVerification(validateRow, item, itemExtPropList, definitionList, catalogList, ref pass, ref errorMsg, ref itemExtProp, ref effective, ref expiration, ref longLived);

                if (!pass)
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] = errorMsg;
                }
                else
                {
                    ValidityStandard validityStandard = new ValidityStandard()
                    {
                        ItemId = item.Id,
                        ItemExtPropName = itemExtProp,
                        Effective = effective,
                        Expiration = expiration,
                        LongLived = longLived
                    };
                    saveValidityStandards.Add(validityStandard);
                }
            }
            ValidityHelper validityHelper = new ValidityHelper(saveValidityStandards);
            validityHelper.SaveCommand();
            RF.BatchInsert(saveValidityStandards);
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
            EntityList<Catalog> catalogList,
            ref bool pass, ref string errorMsg,
            ref string itemExtProp,
            ref DateTime effective, ref DateTime? expiration, ref decimal longLived)
        {
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
            if (datarow.LongLived.IsNullOrEmpty())
            {
                errorMsg += "可使用时长寿命(H)不允许为空";
                pass = false;
            }
            else
            {
                var isNum = decimal.TryParse(datarow.LongLived, out longLived);
                if (!isNum)
                {
                    errorMsg += "可使用时长寿命(H)输入必须为数字;";
                    pass = false;
                }
                if (isNum && longLived <= 0)
                {
                    errorMsg += "可使用时长寿命(H)输入必须为正数;";
                    pass = false;
                }
            }


            // 验证生效时间不能为空
            if (datarow.Effective.IsNullOrEmpty())
            {
                errorMsg += "生效日期不能为空;";
                pass = false;
            }
            else
            {
                // 验证日期
                // 时间格式验证
                if (DateTime.TryParse(datarow.Effective, out DateTime dateTime))
                {
                    effective = dateTime;
                }
                else if (Double.TryParse(datarow.Effective, out double timedouble))
                {
                    effective = DateTime.FromOADate(timedouble);
                }
                else
                {
                    errorMsg += "生效日期格式不合法;";
                    pass = false;
                }
            }

            if (datarow.Expiration.IsNotEmpty())
            {
                // 验证日期
                // 时间格式验证
                if (DateTime.TryParse(datarow.Expiration, out DateTime dateTime))
                {
                    expiration = dateTime;
                }
                else if (Double.TryParse(datarow.Expiration, out double timedouble))
                {
                    expiration = DateTime.FromOADate(timedouble);
                }
                else
                {
                    errorMsg += "失效日期格式不合法;";
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
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 可使用时长寿命
        /// </summary>
        public string LongLived { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        public string Effective { get; set; }

        /// <summary>
        /// 失效日期
        /// </summary>
        public string Expiration { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }
    }
}
