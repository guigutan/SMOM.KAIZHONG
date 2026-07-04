using SIE.Domain;
using SIE.EventMessages.WMS.Traces;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Traces
{
    /// <summary>
    /// 反向追溯-Mes关键件采集信息
    /// </summary>
    [Serializable]
    public class MesProcessCollectKeyItemInfo
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public List<MesProcessCollectKeyItemItemInfo> Data { get; set; } = new List<MesProcessCollectKeyItemItemInfo>();
    }

    /// <summary>
    /// 反向追溯-Mes关键件采集详细信息
    /// </summary>
    [Serializable]
    public class MesProcessCollectKeyItemItemInfo
    {
        /// <summary>
        /// 物料Id
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
        /// 来源条码
        /// </summary>
        public string SourceCode { get; set; }

        /// <summary>
        /// 用料数
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

    }
}
