using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.PackingQcs.Data
{
    /// <summary>
    /// 资源
    /// </summary>
    public class PackingResource
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }
    }
}
