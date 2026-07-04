using SIE.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialPreparations.ApiModels
{
    /// <summary>
    /// 工单BOM信息
    /// </summary>
    [Serializable]
    public class WoBomInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 消耗方式
        /// </summary>
        public ConsumeMode ConsumeMode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性名
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 需求量
        /// </summary>
        public decimal BomNeedQty { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WoId { get; set; }
    }
}
