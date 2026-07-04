using SIE.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘工艺路线工序信息
    /// </summary>
    public class EdgeRouteProcess
    {

        /// <summary>
        /// 工序ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 源工序Id
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// 是否可选
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// 边缘工序物料清单 
        /// </summary>
        public IList<EdgeBom> Boms { get; } = new List<EdgeBom>();


        /// <summary>
        /// 下级工序 
        /// </summary>
        public Dictionary<ResultType, List<string>> Next { get; } = new Dictionary<ResultType, List<string>>();


        /// <summary>
        /// 工序标记
        /// </summary>
        public int Sign { get; set; }


        /// <summary>
        /// 已过站次数
        /// </summary>
        public int PassNum { get; set; }

        /// <summary>
        /// 最大过站次数
        /// </summary>
        public int? MaxPassNum { get; set; }


        /// <summary>
        /// 是否重复
        /// </summary>
        public bool Repeat { get; set; }

        /// <summary>
        /// 是否创建SKU
        /// </summary>
        public bool CreateSku { get; set; }
    }
}
