using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 缺陷代码
    /// </summary>
    [Serializable]
    public class EdgeDefect
    {
        /// <summary>
        /// 缺陷代码Id
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 缺陷代码编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 缺陷代码描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 缺陷分类Id
        /// </summary>
        public double CategoryId { get; set; }
    }
}
