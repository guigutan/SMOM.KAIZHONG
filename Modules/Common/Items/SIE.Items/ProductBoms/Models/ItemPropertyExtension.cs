using SIE.Items.ProductBoms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Items.ProductBoms
{
    /// <summary>
    /// 静态扩展方法
    /// </summary>
    [Serializable]
    public static class ItemPropertyExtension
    {
        /// <summary>
        /// 字符串转为物料扩展属性列表
        /// 结构：物料扩展属性ID1:内容1,内容2;物料扩展属性ID2:内容3,内容4
        /// </summary>
        /// <param name="itemExtProp">物料扩展属性字符串</param>
        /// <param name="relationId">关联属性ID</param>
        /// <returns></returns>
        public static List<ItemPropertyInfo> ItemExtPropToList(this string itemExtProp, double relationId)
        {
            List<ItemPropertyInfo> propertyValues = new List<ItemPropertyInfo>();
            if (itemExtProp.IsNullOrEmpty())
            {
                return propertyValues;
            }

            string[] pvalues = itemExtProp.Split(';');
            foreach (string val in pvalues)
            {
                if (val.IsNullOrEmpty())
                {
                    continue;
                }

                double definitionId;
                string[] vals = val.Split(':');
                if (vals.Length == 2 && double.TryParse(vals[0], out definitionId))
                {
                    string[] arrvalue = vals[1].Split(',');
                    foreach (string v in arrvalue)
                    {
                        if (!v.IsNullOrEmpty())
                        {
                            ItemPropertyInfo propertyValue = new ItemPropertyInfo();
                            propertyValue.DefinitionId = definitionId;
                            propertyValue.RelationId = relationId;
                            propertyValue.Value = v;
                            propertyValues.Add(propertyValue);
                        }
                    }
                }
            }

            return propertyValues;
        }

        /// <summary>
        /// 两个扩展属性列表取交集的属性组数量是否等于指定属性组数量
        /// </summary>
        /// <param name="pvList1">扩展属性列表1</param>
        /// <param name="pvList2">扩展属性列表2</param>
        /// <param name="propertyGroupCount">属性组数量</param>
        /// <returns>返回是否等于指定属性组数量</returns>
        public static bool IntersectPropertyGroup(this List<ItemPropertyInfo> pvList1, List<ItemPropertyInfo> pvList2, int propertyGroupCount)
        {
            if (pvList1.IsNullOrEmpty() && pvList2.IsNullOrEmpty())
            {
                return true;
            }

            if ((pvList1.Count == 0 && pvList2.Count != 0) || (pvList1.Count != 0 && pvList2.Count == 0))
            {
                return false;
            }

            int samePropertyGroup = pvList1.Where(p => pvList2.Any(q => q.DefinitionId == p.DefinitionId && q.Value == p.Value && q.PropertyGroup == p.PropertyGroup))
                    .GroupBy(p => p.PropertyGroup).Count();

            return samePropertyGroup == propertyGroupCount;
        }

        /// <summary>
        /// 两个扩展属性列表取交集的属性组数量是否等于指定属性组数量
        /// </summary>
        /// <param name="pvList1">扩展属性列表1</param>
        /// <param name="pvList2">扩展属性列表2</param>
        /// <returns>返回是否等于指定属性组数量</returns>
        public static bool IntersectPropertyGroup(this List<ItemPropertyInfo> pvList1, List<ItemPropertyInfo> pvList2)
        {
            if (pvList1 == null)
            {
                pvList1 = new List<ItemPropertyInfo>();
            }
            if (pvList2 == null)
            {
                pvList2 = new List<ItemPropertyInfo>();
            }
            int propertyGroupCount = pvList1.GroupBy(p => p.PropertyGroup).Count();
            int propertyGroupCount2 = pvList2.GroupBy(p => p.PropertyGroup).Count();

            return pvList1.IntersectPropertyGroup(pvList2, new List<int> { propertyGroupCount, propertyGroupCount2 }.Max());
        }

        /// <summary>
        /// 物料扩展属性列表转字符串
        /// </summary>
        /// <param name="pvList1">物料扩展属性列表</param>
        /// <returns>返回物料扩展属性字符串</returns>
        public static string ToItemPropertyString(this List<ItemPropertyInfo> pvList1)
        {
            if (pvList1.IsNullOrEmpty())
            {
                pvList1 = new List<ItemPropertyInfo>();
            }
            StringBuilder sb = new StringBuilder();
            var tmpPvList = pvList1.OrderBy(p => p.PropertyGroup).ThenBy(p => p.DefinitionId).ThenBy(p => p.Value).ToList();
            foreach (var pv in tmpPvList)
            {
                sb.Append(pv.PropertyGroup);
                sb.Append(",");
                sb.Append(pv.DefinitionId);
                sb.Append(",");
                sb.Append(pv.Value);
                sb.Append("|");
            }

            return sb.ToString();
        }
    }
}