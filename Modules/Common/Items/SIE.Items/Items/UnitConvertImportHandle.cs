using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Items
{
    /// <summary>
    /// 导入单位转换
    /// </summary>
    [Services.Service(FallbackType = typeof(UnitConvertImportHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class UnitConvertImportHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "物料编码","主单位","辅助单位","分子","分母","默认辅助单位"
        };

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 辅助单位
        /// </summary>
        private Dictionary<string, Unit> dicUnit { get; set; }

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
            this.ColumnValidList.Add(ColumnNameList[0], new ValidColumn(ImportDataType._String, true, VaildItemCode));// 物料编码
            this.ColumnValidList.Add(ColumnNameList[1], new ValidColumn(ImportDataType._String, true, true));// 主单位         
            this.ColumnValidList.Add(ColumnNameList[2], new ValidColumn(ImportDataType._String, true, VaildUnitName));//辅助单位
            this.ColumnValidList.Add(ColumnNameList[3], new ValidColumn(ImportDataType._String, true, true));//分子
            this.ColumnValidList.Add(ColumnNameList[4], new ValidColumn(ImportDataType._String, true, true));//分母
            this.ColumnValidList.Add(ColumnNameList[5], new ValidColumn(ImportDataType._String, true, true));//默认辅助单位
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
            string unitName = dr[1].ToString();
            messageTip = string.Empty;
            string itemData = itemCode + "-" + unitName;

            if (!dicItem.ContainsKey(itemData))
            {
                var item = RT.Service.Resolve<ItemController>().GetAllEnableItems(null, itemCode, unitName).FirstOrDefault();

                if (item == null)
                {
                    messageTip = "不存在于系统或此物料下不存在主单位".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    isValid = false;
                    return isValid;
                }
                dicItem.Add(itemData, item);
            }
            return isValid;
        }

        /// <summary>
        /// 验证辅助单位
        /// </summary>
        /// <param name="obj">辅助单位名称</param>
        /// <param name="messageTip">信息提示</param>
        /// <param name="dr">当前行数据</param>
        /// <returns>是否验证通过</returns>
        private bool VaildUnitName(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            if (dicUnit == null)
            {
                dicUnit = new Dictionary<string, Unit>();
            }

            string unitName = obj.ToString();
            string itemCode = dr[0].ToString();
            string itemUnitName = dr[1].ToString();
            messageTip = string.Empty;

            string unitData = itemCode + "-" + itemUnitName + "-" + unitName;

            if (!dicUnit.ContainsKey(unitData))
            {
                var unit = RT.Service.Resolve<ItemController>().GetUnit(unitName);
                if (unit == null)
                {
                    messageTip = "不存在于系统".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    isValid = false;
                    return isValid;
                }
                dicUnit.Add(unitData, unit);
            }
            else
            {
                messageTip = "+物料编码存在重复".L10N();
                AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                isValid = false;
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
                                   ItemCode = g.Field<string>(ColIndex("物料编码")),
                                   ItemUnitName = g.Field<string>(ColIndex("主单位")),
                                   UnitName = g.Field<string>(ColIndex("辅助单位")),
                                   Numerator = g.Field<string>(ColIndex("分子")),
                                   Denominator = g.Field<string>(ColIndex("分母")),
                                   IsDefault = g.Field<string>(ColIndex("默认辅助单位")),
                                   DetailInfo = g
                               };
            foreach (var mainDataItem in mainDataList)
            {
                try
                {
                    string itemData = mainDataItem.ItemCode + "-" + mainDataItem.ItemUnitName;
                    var vali = mainDataItem.ItemCode + "-" + mainDataItem.ItemUnitName + "-" + mainDataItem.UnitName;
                    var unitConvert = new UnitConvert();
                    unitConvert.ItemId = dicItem[itemData].Id;
                    unitConvert.MainUnitId = dicItem[itemData].UnitId.Value;
                    unitConvert.UnitId = dicUnit[vali].Id;
                    if (!mainDataItem.Numerator.IsNullOrEmpty())
                    {
                        int.TryParse(mainDataItem.Numerator, out int numerator);
                        unitConvert.Numerator = numerator;
                    }
                    if (!mainDataItem.Denominator.IsNullOrEmpty())
                    {
                        int.TryParse(mainDataItem.Denominator, out int denominator);
                        unitConvert.Denominator = denominator;
                    }
                    if (mainDataItem.IsDefault == "是".L10N())
                    {
                        RT.Service.Resolve<ItemUnitController>().UpdateDefaultUnit(unitConvert.ItemId.Value, unitConvert.UnitId);
                        unitConvert.IsDefault = true;
                    }
                    else
                    {
                        unitConvert.IsDefault = RT.Service.Resolve<ItemUnitController>().UpdateDefaultItemUnit(unitConvert.ItemId.Value, unitConvert.UnitId);
                    }
                    if (mainDataItem.ItemUnitName == mainDataItem.UnitName)
                    {
                        throw new ValidationException("主单位与辅助单位不能相同".L10N());
                    }
                    if (RT.Service.Resolve<ItemUnitController>().GetItemUnits(unitConvert))
                    {
                        throw new ValidationException("存在相同的主单位和辅助单位数据".L10N());
                    }
                 
                    RF.Save(unitConvert);
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
            if (dicUnit != null)
            {
                dicUnit.Clear();
                dicUnit = null;
            }
            if (dicItem != null)
            {
                dicItem.Clear();
                dicItem = null;
            }
        }
    }
}
