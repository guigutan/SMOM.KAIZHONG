using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.Routings.RoutingBoms.DataQueryers
{
    /// <summary>
    /// 工艺路线版本、工段等信息
    /// </summary>
    [Serializable]
    public class RoutingVersionInfo
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public double? RoutingVersionId { get; set; }
        /// <summary>
        /// 工艺路线版本名称
        /// </summary>
        public string RoutingVersionName { get; set; }
        /// <summary>
        /// 工段ID
        /// </summary>
        public double? ProccessSegmentId { get; set; }
        /// <summary>
        /// 工段编码
        /// </summary>
        public string ProccessSegmentCode { get; set; }
    }
}
