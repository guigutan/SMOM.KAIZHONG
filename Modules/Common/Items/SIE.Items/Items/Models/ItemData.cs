using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料扩展属性
    /// </summary>
    [Serializable]
    public class ItemExtendPropData
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名
        /// </summary>
        public string ItemExtPropName { get; set; }

    }

    /// <summary>
    /// 物料对应编码
    /// </summary>
    [Serializable]
    public class ItemAndCode
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }
    }

    /// <summary>
    /// 物料对应单位
    /// </summary>
    [Serializable]
    public class ItemAndUnit
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public double UnitId { get; set; }
    }

}
