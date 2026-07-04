using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    /// <summary>
    /// 缺陷责任分类项目
    /// </summary>
    public class ResponsibilityCategoryItem
    {
        /// <summary>
        /// 缺陷责任分类
        /// </summary>
        public DefectResponsibilityCategory Category { get; set; }

        /// <summary>
        /// 缺陷责任分类项目 集合
        /// </summary>
        public List<ResponsibilityCategoryItem> Children { get; } = new List<ResponsibilityCategoryItem>();

        /// <summary>
        /// 缺陷责任分类项目
        /// </summary>
        public ResponsibilityCategoryItem Parent { get; set; }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            var item = obj as ResponsibilityCategoryItem;
            if (item == null) return false;
            return Category?.Id == item.Category?.Id;
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            if (Category == null) return 0;
            return Category.GetHashCode();
        }
    }
}
