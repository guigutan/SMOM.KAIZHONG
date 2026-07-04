using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP.Entity
{
    /// <summary>
    /// 缺陷分类项目
    /// </summary>
   [Serializable]
    public class DefectCategoryItem
    {
        /// <summary>
        /// 缺陷分类
        /// </summary>
        public DefectCategory Category { get; set; }

        /// <summary>
        /// 缺陷分类集合
        /// </summary>
        public List<DefectCategoryItem> Children { get; } = new List<DefectCategoryItem>();

        /// <summary>
        /// 缺陷分类项目
        /// </summary>
        public DefectCategoryItem Parent { get; set; }

        /// <summary>
        /// 比较缺陷分类ID是否一致
        /// </summary>
        /// <param name="obj">需要比较的对象</param>
        /// <returns>返回是否与传入的对象的缺陷分类ID一致</returns>
        public override bool Equals(object obj)
        {
            var item = obj as DefectCategoryItem;
            if (item == null) return false;
            return Category?.Id == item.Category?.Id;
        }

        /// <summary>
        /// 获取缺陷分类的哈希编码
        /// </summary>
        /// <returns>返回缺陷分类的哈希编码</returns>
        public override int GetHashCode()
        {
            if (Category == null) return 0;
            return Category.GetHashCode();
        }
    }
}
