using SIE.EventMessages.Common.Traces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Traces
{
    /// <summary>
    /// 采集记录追溯查询实体
    /// </summary>
    [Serializable]
    public class TraceInfoForProcssKeyItemCriteria
    {
        /// <summary>
        /// 生产通用报表Id
        /// </summary>
        public double WipProductVersionId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 产品扩展属性
        /// </summary>
        public string ProductExtProp { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string ProductSn { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }
      

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料来源条码
        /// </summary>
        public string ItemSourceCode { get; set; }

        /// <summary>
        /// 分页信息
        /// </summary>
        public PagingInfo PagingInfo { get; set; }
    }
}
