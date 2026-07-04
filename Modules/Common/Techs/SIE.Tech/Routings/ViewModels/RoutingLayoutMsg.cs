using System;

namespace SIE.Tech.Routings.ViewModels
{
    /// <summary>
    /// 工艺路线信息
    /// </summary>
    [Serializable]
    public class RoutingLayoutMsg
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public double RoutingId { get; set; }

        /// <summary>
        /// 工艺路线版本ID
        /// </summary>
        public double RoutingVersionId { get; set; }

        /// <summary>
        /// 工艺路线布局
        /// </summary>
        public string Layout { get; set; }

        /// <summary>
        /// 版本名字
        /// </summary>
        public string VersionName { get; set; }
    }
}
