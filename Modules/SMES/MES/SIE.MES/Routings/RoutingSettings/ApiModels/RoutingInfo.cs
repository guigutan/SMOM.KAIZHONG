using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Routings.RoutingSettings.ApiModels
{
    /// <summary>
    /// 工序清单信息
    /// </summary>
    [Serializable]
    public class DefaultRoutingProcessInfo
    {
        /// <summary>
        /// 工序清单Id
        /// </summary>
        public double RoutingProcessId { get; set; }

        /// <summary>
        /// 默认版本Id
        /// </summary>
        public double DefaultVersionId { get; set; }

        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public double RoutingId { get; set; }
    }
}
