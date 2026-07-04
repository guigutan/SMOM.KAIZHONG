using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    [Serializable]
    public class WipDefectResponsibility
    {
        /// <summary>
        /// 数据库状态
        /// </summary>
        public PersistenceStatus PersistenceStatus { get; set; }

        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id
        {
            get; set;
        }
        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        public double WipProductDefectId
        {
            get;set;
        }

        /// <summary>
        /// 缺陷责任Id
        /// </summary>
        public double DefectResponsibilityId
        {
            get; set;
        }
        /// <summary>
        /// 缺陷责任编码
        /// </summary>
        public string ResponseCode
        {
            get; set;
        }
        
        /// <summary>
        /// 缺陷责任名称
        /// </summary>
        public string ResponseDesc
        {
            get; set;
        }
        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string DefectLocation
        {
            get; set;
        }
    }
}
