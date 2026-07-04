using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Traces.Models
{
    /// <summary>
    /// 产品信息
    /// </summary>
    [Serializable]
    public class RecursiveProductInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TreeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double SnId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }
        /// <summary>
        /// 产品Sn
        /// </summary>
        public string ProductSn { get; set; }
        /// <summary>
        /// 产品扩展属性
        /// </summary>
        public string ProductExtPropName { get; set; }
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        public double ItemId { get; set; }

        public string ItemSourceCode { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
