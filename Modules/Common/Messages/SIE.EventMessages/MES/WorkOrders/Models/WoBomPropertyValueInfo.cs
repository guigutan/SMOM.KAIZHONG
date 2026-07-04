using System;

namespace SIE.EventMessages.MES.WorkOrders.Models
{
    /// <summary>
    /// 工单BOM物料扩展属性
    /// </summary>
    [Serializable]
    public class WoBomPropertyValueInfo
    {
        /// <summary>
        /// 工单BOM ID
        /// </summary>
        public double WoBomId { get; set; }

        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        public double DefinitionId { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string DefinitionName { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
    }
}
