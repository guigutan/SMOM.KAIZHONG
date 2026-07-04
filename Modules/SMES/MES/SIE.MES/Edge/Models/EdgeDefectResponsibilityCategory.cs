using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 缺陷责任分类
    /// </summary>
   [Serializable]
    public class EdgeDefectResponsibilityCategory
    {
        /// <summary>
        /// 缺陷责任分类Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description{ get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public double? TreePId { get; set; }
    }
}
