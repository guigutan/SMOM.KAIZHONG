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
    /// 过程追溯-产品检验记录追溯
    /// </summary>
    [Serializable]
    public class TraceInfoForProductInspect
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public List<TraceItemInfoForProductInspect> Data { get; set; } = new List<TraceItemInfoForProductInspect>();
    }

    /// <summary>
    /// 过程追溯-关联产品追溯详细信息
    /// </summary>
    [Serializable]
    public class TraceItemInfoForProductInspect
    {

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 规范下限
        /// </summary>
        public decimal? LimitLow { get; set; }

        /// <summary>
        /// 规范上限
        /// </summary>
        public decimal? LimitMax { get; set; }

        /// <summary>
        /// 测试值
        /// </summary>
        public decimal? InspectionValue { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 检验结果
        /// </summary>
        public string Result { get; set; }


    }
}
