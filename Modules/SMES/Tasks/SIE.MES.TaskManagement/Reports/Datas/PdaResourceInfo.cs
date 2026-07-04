using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports.Datas
{
    /// <summary>
    /// 资源信息
    /// </summary>
    [Serializable]
    public class PdaResourceInfo
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }



    }
}
