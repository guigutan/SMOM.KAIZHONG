using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘 生产资源信息
    /// </summary>
    [Serializable]
    public class EdgeResource
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

        public EdgeShiftInfo EdgeShiftInfo{ get; set; }
}
}
