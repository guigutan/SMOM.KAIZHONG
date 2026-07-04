using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 缺陷责任类
    /// </summary>
    [Serializable]
    public  class EdgeDefectResponsibility
    {
        /// <summary>
        /// 缺陷责任Id
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 分类Id
        /// </summary>
        public double CategoryId { get; set; }
    }
}
