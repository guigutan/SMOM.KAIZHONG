using SIE.Inventory.Commom;
using System;

namespace SIE.Inventory.TransactionProcessing
{
    /// <summary>
    /// 物料扩展属性数据
    /// </summary>
    [Serializable]
    public class BaseItemExtPropData
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 外键Id
        /// </summary>
        public double FId { get; set; }

        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        public double DefinitionId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ItemExtPropFunctionType Type { get; set; }
    }
}
