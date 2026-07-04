using System.Collections.Generic;

namespace SIE.Inventory.Commom
{
    /// <summary>
    /// 扩展属性数据
    /// </summary>
    public class ExtensionData
    {
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 属性id
        /// </summary>
        public double id { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public List<string> valList { get; set; }
    }
}
