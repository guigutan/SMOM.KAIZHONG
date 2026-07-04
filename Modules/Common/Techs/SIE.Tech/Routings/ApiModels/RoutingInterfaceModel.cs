using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Tech.Routings.ApiModels
{
    /// <summary>
    /// 工艺路线接口模型
    /// </summary>
    [Serializable]
    public class RoutingInterfaceModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoutingInterfaceModel()
        {
            routingProcessDetais = new List<RoutingProcessDetai>();
        }
        /// <summary>
        /// 工艺路线主信息
        /// </summary>
        public RoutingSummaries routingSummaries
        {
            get; set;
        }

        /// <summary>
        /// 工艺路线工序明细信息
        /// </summary>
        public List<RoutingProcessDetai> routingProcessDetais { get; set; }
    }
}
