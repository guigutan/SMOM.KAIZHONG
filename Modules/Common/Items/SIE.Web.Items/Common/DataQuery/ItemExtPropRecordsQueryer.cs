using SIE.Domain;
using SIE.Items;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Items.Common.DataQuery
{
    /// <summary>
    /// 物料扩展属性
    /// </summary>
    public class ItemExtPropRecordsQueryer : DataQueryer
    {
        /// <summary>
        /// 解析物料属性ID
        /// </summary>
        /// <param name="selectPropValues"></param>
        /// <returns></returns>
        private Dictionary<double, string> GetDefinitionId(string selectPropValues)
        {
            Dictionary<double, string> results = new Dictionary<double, string>();
            var values = selectPropValues.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var value in values)
            {
                if (!value.IsNullOrWhiteSpace())
                {
                    var definitionId = Convert.ToDouble(value.Split(':')[0]);
                    var propertyValue = value.Split(':')[1];
                    results.Add(definitionId, propertyValue);
                }
            }
            return results;
        }

        public List<ItemPropertyValue> GetItemPropertys(double itemId)
        {
            var propertyValues = RT.Service.Resolve<ItemController>().GetItemPropertys(itemId);
            return propertyValues.ToList();
        }


        /// <summary>
        /// 获取物料属性值
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="selectPropValues">已选物料扩展属性</param>
        /// <param name="productBomId">产品BOM ID（如果大于0，则筛选出该BOM明细内出现过的扩展属性）</param>
        /// <returns>属性值</returns>
        public List<ItemExtPropValueData> GetItemExtPropRecordsValue(double itemId, string selectPropValues, double productBomId = 0)
        {
            EntityList<ItemPropertyValue> propertyValues;
            if (productBomId > 0)
            {
                propertyValues = RT.Service.Resolve<ItemController>().GetItemPropertysWithBomIs(itemId, productBomId);
            }
            else
            {
                propertyValues = RT.Service.Resolve<ItemController>().GetItemPropertys(itemId);
            }
            List<ItemExtPropValueData> rst = new List<ItemExtPropValueData>();
            if (propertyValues.Count == 0)
            {
                return rst;
            }
            var selectDefinionIds = GetDefinitionId(selectPropValues ?? "");
            propertyValues.OrderBy(p => p.DefinitionId).GroupBy(p => p.DefinitionId).ForEach(p =>
            {
                ItemExtPropValueData item = new ItemExtPropValueData();
                item.Name = p.FirstOrDefault().DefinitionName;
                item.DefinitionId = p.Key;
                p.ToList().ForEach(g =>
                {
                    PropertyValues values = new PropertyValues() { Value = g.Value };
                    if (selectDefinionIds.Any(p => p.Key == g.DefinitionId && p.Value == g.Value))
                    {
                        values.IsChecked = true;
                    }
                    item.Values.Add(values);
                });
                rst.Add(item);
            });
            return rst;
        }

        /// <summary>
        /// 根据id获取物料的类型的枚举值
        /// </summary>
        public int GetItemTypeById(double itemId)
        {
            ItemType tmp = RT.Service.Resolve<ItemController>().GetType(itemId);
            return ((int)tmp);
        }
    }

    /// <summary>
    /// 物料属性值数据
    /// </summary>
    public class ItemExtPropValueData
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 属性Id
        /// </summary>
        public double DefinitionId { get; set; }

        /// <summary>
        /// 属性值集合
        /// </summary>
        public List<PropertyValues> Values { get; set; } = new List<PropertyValues>();
    }

    /// <summary>
    /// 属性值集合
    /// </summary>
    public class PropertyValues
    {
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 是否已选
        /// </summary>
        public bool IsChecked { get; set; }
    }
}
