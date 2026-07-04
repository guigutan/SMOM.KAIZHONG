using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘层工位信息
    /// </summary>
    [Serializable]
    public class EdgeStation
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        /// 关联工序Ids
        /// </summary>
        public List<string> ProcessIds { get; set; }
    }
}
