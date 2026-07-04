using SIE.Common.Prints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 线边仓盘点
    /// </summary>
    [Serializable]
    [DisplayName("线边仓盘点")]
    public class LesStockCountPrintable : BillPrintable<LesStockCount>
    {
        /// <summary>
        /// 根据实体类型获取属性
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns>对应type的属性</returns>
        public override IEnumerable<string> GetPropertys(Type type = null)
        {
            var propertys = base.GetPropertys(type).ToList();
            if (type == typeof(LesStockCount))
            {
                propertys.Add("Operator_Code");
                propertys.Add("Operator_Name");
                propertys.Add("R_TableName");
                propertys.Add("R_CreateBy");
                propertys.Add("R_UpdateBy");
            }

            if (type == typeof(LesStockCountDetail))
            {
                propertys.Add("Item_Code");
                propertys.Add("Item_Name");
                propertys.Add("Item_SpecificationModel");
                propertys.Add("Item_Unit");
                propertys.Add("Warehouse_Code");
                propertys.Add("Warehouse_Name");
                propertys.Add("StorageLocation_Code");
                propertys.Add("StorageLocation_Name");
                propertys.Add("LotCode");
                propertys.Add("CountByCode");
                propertys.Add("CountByName");
            }
            return propertys;
        }

        /// <summary>
        /// 转换数据
        /// </summary>
        /// <param name="data">实体对象</param>
        /// <returns>转换后的数据</returns>
        public override string ConverterData(object data)
        {
            var content = base.ConverterData(data);
            if (data is LesStockCount)
            {
                var bill = data as LesStockCount;
                if (bill != null)
                {
                    string operator_Code = string.Empty;
                    string Operator_Name = string.Empty;
                    if (bill.Operator != null)
                    {
                        operator_Code = bill.Operator.Code;
                        Operator_Name = bill.Operator.Name;
                    }
                    content += operator_Code + Separator + Operator_Name + Separator;
                    var meta = Domain.RF.Find<LesStockCount>().EntityMeta;
                    var tableName = meta.TableMeta.TableName;
                    content += tableName + Separator
                        + bill.CreateBy + Separator
                        + bill.UpdateBy + Separator;
                }
            }

            if (data is LesStockCountDetail)
            {
                var detail = data as LesStockCountDetail;
                if (detail != null)
                {
                    string countByCode = string.Empty;
                    string countByName = string.Empty;
                    string reCountByCode = string.Empty;
                    string reCountByName = string.Empty;
                    if (detail.CountBy != null)
                    {
                        countByCode = detail.CountBy.Code;
                        countByName = detail.CountBy.Name;
                    }
                    content += detail.Item?.Code + Separator + detail.Item?.Name + Separator +
                            detail.Item?.SpecificationModel + Separator + detail.Item?.Unit?.Name + Separator +
                            detail.Warehouse?.Code + Separator + detail.Warehouse?.Name + Separator +
                            detail.StorageLocation?.Code + Separator + detail.StorageLocation?.Name + Separator +
                            detail.Lot?.Code + Separator +
                            countByCode + Separator + countByName + Separator +
                            reCountByCode + Separator + reCountByName + Separator;
                }
            }
            return content;
        }
    }
}
