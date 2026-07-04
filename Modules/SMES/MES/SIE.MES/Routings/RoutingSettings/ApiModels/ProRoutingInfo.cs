using SIE.Core.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Routings.RoutingSettings.ApiModels
{
    /// <summary>
    /// 产品工艺路线设置信息
    /// </summary>
    [Serializable]
    public class ProRoutingInfo
    {
        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType OrderType { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public double? RoutingId { get; set; }

        /// <summary>
        /// 版本Id
        /// </summary>
        public double? RoutingVersionId { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionName { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
