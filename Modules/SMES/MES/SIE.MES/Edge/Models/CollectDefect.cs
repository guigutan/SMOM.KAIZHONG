using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 采集缺陷信息
    /// </summary>
    [Serializable]
    public class CollectDefect: Defects.DefectData
    {
        /// <summary>
        /// 已维修
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// 维修措施
        /// </summary>
        public List<CollectDefectMeasure>DefectMeasure{ get; set; }

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public List<CollectDefectResponsibility> DefectResponsibility { get; set; }
    }
}
