using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ApiModels
{
    /// <summary>
    /// 前端命令转实体
    /// </summary>
    [Serializable]
    public class TreeCmdInfo
    {
        /// <summary>
        /// 项目号需求设计Id
        /// </summary>
        public double ProductDesignId { get; set; }

        /// <summary>
        /// 产品Ids
        /// </summary>
        public List<double> ProductIds { get; set; } = new List<double>();
    }
}
