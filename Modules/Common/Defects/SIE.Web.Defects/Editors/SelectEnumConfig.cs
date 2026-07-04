using SIE.Utils;
using SIE.Web.ClientMetaModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Defects.Editors
{
    /// <summary>
    /// 枚举筛选编辑器
    /// </summary>
    public class SelectEnumConfig : EnumBoxConfig
    {
        /// <summary>
        /// 显示枚举值集合
        /// </summary>
        public List<Enum> ValuesList { get; set; } = new List<Enum>();

        /// <summary>
        /// 过滤分类枚举
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public override List<EnumViewModel> FilterEnum(List<EnumViewModel> models)
        {
            if (ValuesList.Count > 0)
            {
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
