using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 缺陷分类
    /// </summary>
    [Serializable]
    public class EdgeDefectCategory
    {
        /// <summary>
        /// 缺陷分类Id
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 缺陷分类编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 缺陷分类描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 父Id
        /// </summary>
        public double? TreePId { get; set; }
    }
}
