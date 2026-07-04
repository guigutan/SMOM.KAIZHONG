using System;

namespace SIE.EventMessages
{
    /// <summary>
    /// 物料属性
    /// </summary>
    [Serializable]
    public class ItemPropertyData
    {
        /// <summary>
        /// 属性Id
        /// </summary>
        public double DefinitionId { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string DefinitionName { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string DefinitionValue { get; set; }
    }
}
