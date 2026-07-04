using System;

namespace SIE.Items.ProductBoms.Models
{
    /// <summary>
    /// 物料扩展属性对象
    /// </summary>
    [Serializable]
    public class ItemPropertyInfo
    {
        /// <summary>
        /// 关联 ID （可能来源： 组合替代表ID 或 制程单BOM明细ID）
        /// </summary>
        public double RelationId { get; set; }

        /// <summary>
        /// 关联 ID （String类型的ID）
        /// </summary>
        public string DetailId { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 定义ID
        /// </summary>
        public double DefinitionId { get; set; }

        /// <summary>
        /// 属性定义名称
        /// </summary>
        public string DefinitionName { get; set; }

        /// <summary>
        /// 属性组
        /// </summary>
        public string PropertyGroup { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemPropertyInfo()
        {
            PropertyGroup = string.Empty;
        }

        /// <summary>
        /// 复制一个新的对象
        /// </summary>
        /// <returns>返回新的对象</returns>
        public ItemPropertyInfo Copy()
        {
            ItemPropertyInfo newProperty = new ItemPropertyInfo();
            newProperty.RelationId = this.RelationId;
            newProperty.DetailId = this.DetailId;
            newProperty.Value = this.Value;
            newProperty.DefinitionId = this.DefinitionId;
            newProperty.DefinitionName = this.DefinitionName;
            newProperty.PropertyGroup = this.PropertyGroup;

            return newProperty;
        }
    }
}