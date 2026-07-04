using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘工序信息
    /// </summary>
    [Serializable]
    public class EdgeProcess
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
        /// 工序类型
        /// </summary>
        public int? ProcessType { get; set; }

        /// <summary>
        /// 缺陷信息
        /// </summary>
        public EdgeDefectInfo DefectInfo { get; set; }

        /// <summary>
        /// 采集步骤
        /// </summary>
        public List<EdgeCollectStep> EdgeCollectSteps { get; set; } = new List<EdgeCollectStep>();
    }
}
