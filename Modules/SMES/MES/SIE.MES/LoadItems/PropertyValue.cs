using System;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 物料属性
    /// </summary>
    [Serializable]
    public class PropertyValue
    {
        /// <summary>
        /// 属性定义
        /// </summary>
        public double DefinitionId { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
    }
}
