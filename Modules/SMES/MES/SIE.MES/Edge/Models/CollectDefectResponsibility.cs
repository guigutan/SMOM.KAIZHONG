using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 采集缺陷责任
    /// </summary>
    [Serializable]
    public class CollectDefectResponsibility
    {

        /// <summary>
        /// 缺陷责任Id
        /// </summary>
        public double DefectResponsibilityId
        {
            get;
            set;
        }

        /// <summary>
        /// 缺陷责任编码
        /// </summary>
        public string ResponseCode
        {
            get;
            set;
        }
        /// <summary>
        /// 缺陷责任名称
        /// </summary>
        public string ResponseDesc
        {
            get;
            set;
        }

        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string DefectLocation
        {
            get;
            set;
        }
    }

}
