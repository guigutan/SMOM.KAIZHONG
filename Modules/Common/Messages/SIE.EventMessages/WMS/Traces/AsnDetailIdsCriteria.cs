using SIE.EventMessages.Common.Traces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.WMS.Traces
{
    /// <summary>
    /// WmsAsn单明细信息查询实体
    /// </summary>
    [Serializable]
    public class AsnDetailIdsCriteria
    {
        /// <summary>
        /// 追溯类型
        /// </summary>
        public TraceType TraceType { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        public string ItemLot { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }
    }
}
