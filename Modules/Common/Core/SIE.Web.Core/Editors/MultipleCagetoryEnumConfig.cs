using SIE.Utils;
using SIE.Web.ClientMetaModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SIE.Web.Core.Editors
{
    /// <summary>
    /// 多分类枚举筛选编辑器
    /// </summary>
    public class MultipleCagetoryEnumConfig : EnumBoxConfig
    {
        /// <summary>
        /// 显示枚举值集合
        /// </summary>
        private List<Enum> ValuesList { get; set; } = new List<Enum>();


        /// <summary>
        /// 分类集合
        /// </summary>
        public List<string> CategoryList { get; } = new List<string>();

        /// <summary>
        /// 过滤分类枚举
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public override List<EnumViewModel> FilterEnum(List<EnumViewModel> models)
        {

            if (CategoryList.Count > 0)
            {
                foreach (Enum item in Enum.GetValues(base.EnumType))
                {
                    Type type = item.GetType();
                    FieldInfo fieldInfo = type.GetField(item.ToString());
                    if (fieldInfo != null && fieldInfo.IsDefined(typeof(CategoryAttribute), true))
                    {
                        CategoryAttribute categoryAttribute = (CategoryAttribute)fieldInfo.GetCustomAttribute(typeof(CategoryAttribute), true);
                        foreach (var categoryValue in CategoryList)
                        {
                            if (categoryAttribute.Category == categoryValue)
                            {
                                ValuesList.Add(item);
                            }
                        }
                    }
                }

                models = models.Where(p => ValuesList.Contains(p.EnumValue)).ToList();
            }

            if (FilterCategoery.IsNotEmpty())
            {
                models = models.Where(p => p.Category == FilterCategoery).ToList();
            }
            return models;
        }
    }
}
