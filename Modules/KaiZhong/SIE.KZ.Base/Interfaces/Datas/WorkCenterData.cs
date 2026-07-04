using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 工作中心数据
    /// </summary>
    [Serializable]
    public class WorkCenterData
    {

        /// <summary>
        /// 工作中心类别
        /// </summary>
        public string VERWE { get; set; }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string ARBPL { get; set; }

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string KTEXT { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 工作中心负责人
        /// </summary>
        public string VERAN { get; set; }

        /// <summary>
        /// 状态(X:禁用;空为启用s)
        /// </summary>
        public string LVORM { get; set; }
    }
}
