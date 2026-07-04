using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Routings.RoutingBoms.ApiModels
{
    /// <summary>
    /// 工序Bom明细信息
    /// </summary>
    [Serializable]
    public class RtBomDtlInfo
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double MaterialId { get; set; }

        /// <summary>
        /// 单位用量
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 工艺路线工序Id
        /// </summary>
        public double RoutingProcessId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 产品工艺路线Id
        /// </summary>
        public double RoutingId { get; set; }

        /// <summary>
        /// 版本Id
        /// </summary>
        public double RoutingVersionId { get; set; }
    }
}
