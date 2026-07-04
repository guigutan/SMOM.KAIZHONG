using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// 返工工艺路线版本
    /// </summary>
    [Serializable]
    public class PdaReworkLayoutVersionInfo
    {
        /// <summary>
        /// 返工工艺路线版本Id
        /// </summary>
        public double VersionId { get; set; }

        /// <summary>
        /// 生产版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 版本描述
        /// </summary>
        public string Desc { get; set; }
    }
}
