using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 可疑品阈值下载
    /// </summary>
    [Serializable]
    public class ThresholdData
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 目标值
        /// </summary>
        public string ThresholdValue { get; set; }

        /// <summary>
        /// 预警值
        /// </summary>
        public string AlertValue { get; set; }
    }
}
