using SIE.Common;
using SIE.Common.Algorithm;
using SIE.Common.Catalogs;
using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Items.ProductBoms;
using SIE.Resources.ProcessSegments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Items
{
    /// <summary>
    /// 产品BOM明细导入
    /// </summary>
    [Services.Service(FallbackType = typeof(ProductBomDetailHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ProductBomDetailHandle : IDisposable, IBusinessImport
    {
        private const string bomCode = "产品BOM编码";
        private const string itemCode = "物料编码";
        private const string unitConsumption = "单位耗用量";
        private const string lossrate = "损耗率";
        private const string itemExtValues = "扩展属性值";
        private const string recoil = "是否反冲物料";
        private const string remark = "备注";
        private const string section = "工段";
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            bomCode.L10N(),itemCode.L10N(),unitConsumption.L10N(),lossrate.L10N(),itemExtValues.L10N(),recoil.L10N(),section.L10N(),remark.L10N()
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
                                     BomCode = g.Field<string>(ColIndex(bomCode.L10N())).Trim(),
                                     ItemCode = g.Field<string>(ColIndex(itemCode.L10N())).Trim(),
                                     UnitConsumption = g.Field<string>(ColIndex(unitConsumption.L10N())).Trim(),
                                     Lossrate = g.Field<string>(ColIndex(lossrate.L10N())).Trim(),
                                     ItemExtValues = g.Field<string>(ColIndex(itemExtValues.L10N())).Trim(),
                                     Recoil = g.Field<string>(ColIndex(recoil.L10N())).Trim(),
                                     Remark = g.Field<string>(ColIndex(remark.L10N())).Trim(),
                                     Section = g.Field<string>(ColIndex(section.L10N())).Trim(),
                                     DetailInfo = g,
                                 };
            // BOM编码    
            List<string> bomCodeList = new List<string>();
            List<string> itemCodeList = new List<string>();
            List<string> processSegmentList = new List<string>();
            importDataList.ForEach(data =>
            {
                bomCodeList.Add(data.BomCode);
                itemCodeList.Add(data.ItemCode);
                processSegmentList.Add(data.Section);
            });

            bomCodeList = bomCodeList.Distinct().ToList();
            var dbBomCodeList = RT.Service.Resolve<ProductBomController>().GetProductBomByCodes(bomCodeList);//主表数据

            var dbProcessSegmentList = RT.Service.Resolve<ProcessSegmentController>().GetProcessSegmentByCodes(processSegmentList);//主表数据

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

            EntityList<ProductBomDetail> saveProductBomDetails = new EntityList<ProductBomDetail>();
            var importDataRows = importDataList.ToList();
            for (int i = 0; i < importDataRows.Count; i++)
            {
                // 是否通过
                bool pass = true;
                // 错误信息
                string errorMsg = string.Empty;

                // 物料扩展属性
                var itemExtProp = string.Empty;

                bool IsRecoil = false;
                decimal Lossrate = 0m;
                decimal UnitConsumpt = 0m;
                double? SegmentId = null;


                if (importDataRows[i].BomCode.IsNullOrEmpty())
                {
                    ImportExtension.BatchAppendText(new List<DataRow>() { importDataRows[i].DetailInfo }, ImportDataHandle.MessageColumnName, "数据[产品BOM编码]不能为空！".L10N());
                    continue;
                }
                var parentBom = dbBomCodeList.FirstOrDefault(p => p.Code == importDataRows[i].BomCode);
                if (parentBom == null)
                {
                    ImportExtension.BatchAppendText(new List<DataRow>() { importDataRows[i].DetailInfo }, ImportDataHandle.MessageColumnName, "数据[产品BOM编码]在系统中不存在，请检查！".L10N());
                    continue;
                }

                var item = itemList.FirstOrDefault(p => p.Code == importDataRows[i].ItemCode);
                // 验证实体
                ValidateDetailRow validateRow = new ValidateDetailRow
                {
                    BomCode = importDataRows[i].BomCode,
                    ItemCode = importDataRows[i].ItemCode,
                    Lossrate = importDataRows[i].Lossrate,
                    Recoil = importDataRows[i].Recoil,
                    ItemExtValues = importDataRows[i].ItemExtValues,
                     Section = importDataRows[i].Section,
                    UnitConsumpt = importDataRows[i].UnitConsumption,
                    Remark = importDataRows[i].Remark,
                };
                // 验证主表字段
                FieldVerification(validateRow, item, itemExtPropList, definitionList, catalogList, dbBomCodeList, dbProcessSegmentList, ref pass, ref errorMsg, ref itemExtProp, ref IsRecoil, ref Lossrate, ref UnitConsumpt, ref SegmentId);

                if (!pass)
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] = errorMsg;
                }
                else
                {
                    ProductBomDetail productBomDetail = new ProductBomDetail()
                    {
                        ItemId = item.Id,
                        ItemExtPropName = itemExtProp,
                        IsRecoilItem = IsRecoil,
                        LossRate = Lossrate,
                        ProcessSegmentId = SegmentId,
                        ProductBomId = parentBom.Id,
                        UnitQty = UnitConsumpt,
                        Remark = validateRow.Remark,
                    };
                    saveProductBomDetails.Add(productBomDetail);
                }
            }
            RF.Save(saveProductBomDetails);
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


        private void FieldVerification(ValidateDetailRow datarow, Item item, EntityList<ItemPropertyValue> itemExtPropList, EntityList<ItemPropertyDefinition> definitionList,
            EntityList<Catalog> catalogList, EntityList<ProductBom> dbBomCodeList, EntityList<ProcessSegment> dbProcessSegmentList,
            ref bool pass, ref string errorMsg,
            ref string itemExtProp,
            ref bool IsRecoil, ref decimal Lossrate, ref decimal UnitConsumpt, ref double? SegmentId)
        {
            if (datarow.ItemCode.IsNullOrEmpty())
            {
                errorMsg += "物料编码不能为空".L10N();
                pass = false;
            }
            if (datarow.BomCode.IsNullOrEmpty())
            {
                errorMsg += "产品BOM编码不能为空".L10N();
                pass = false;
            }

            if (!dbBomCodeList.Any(m => m.Code == datarow.BomCode))
            {
                errorMsg += "产品BOM在系统不存在，无法导入".L10N();
                pass = false;
            }
            // 验证物料是否存在
            if (item == null)
            {
                errorMsg += "物料[{0}]不存在".L10nFormat(datarow.ItemCode);
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
            if (datarow.UnitConsumpt.IsNullOrEmpty())
            {
                errorMsg += "单位耗用量不能为空;".L10N();
                pass = false;
            }
            else
            {
                if (decimal.TryParse(datarow.UnitConsumpt, out decimal unitConsumpt))
                {
                    UnitConsumpt = unitConsumpt;
                }
                else
                {
                    errorMsg += "单位耗用量只能输入数字;".L10N();
                    pass = false;
                }
            }


            if (datarow.Recoil.IsNotEmpty())
            {
                if (bool.TryParse(datarow.Recoil, out bool isRecoil))
                {
                    IsRecoil = isRecoil;
                }
                else
                {
                    errorMsg += "是否反冲物料格式不合法;".L10N();
                    pass = false;
                }
            }
            if (datarow.Lossrate.IsNotEmpty())
            {
                if (decimal.TryParse(datarow.Lossrate, out decimal lossrate))
                {
                    Lossrate = lossrate;
                }
                else
                {
                    errorMsg += "耗损率格式不合法;".L10N();
                    pass = false;
                }
            }
            if (datarow.Section.IsNotEmpty())
            {
                var segment = dbProcessSegmentList.FirstOrDefault(m => m.Code == datarow.Section);

                if (segment == null)
                {
                    errorMsg += "工段[{0}]系统不存在,请检查;".L10nFormat(datarow.Section);
                    pass = false;
                }
                else
                {
                    SegmentId = segment.Id;
                }
            }
        }
    }

    /// <summary>
    /// 验证实体
    /// </summary>
    [Serializable]
    public class ValidateDetailRow
    {

        /// <summary>
        /// 产品BOM编码
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public string UnitConsumpt { get; set; }

        /// <summary>
        /// 损耗率
        /// </summary>
        public string Lossrate { get; set; }

        /// <summary>
        /// 是否反冲物料
        /// </summary>
        public string Recoil { get; set; }

        /// <summary>
        /// 工段
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtValues { get; set; }
    }
}
