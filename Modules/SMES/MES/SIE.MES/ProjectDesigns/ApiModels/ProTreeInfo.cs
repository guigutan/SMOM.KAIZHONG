using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ApiModels
{
    /// <summary>
    /// 工艺资料信息
    /// </summary>
    [Serializable]
    public class ProTreeInfo
    {
        /// <summary>
        /// 项目号需求设计Id
        /// </summary>
        public double ProjectDesignDetailId { get; set; }

        /// <summary>
        /// 首层Id
        /// </summary>
        public double ProductId { get; set; }
    }
}
