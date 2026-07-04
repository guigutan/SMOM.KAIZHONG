using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 产品与产线的关系
    /// </summary>
    [Serializable]
    public class ProductLineData
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 产线/机台编码
        /// </summary>
        public string WipResourceCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode { get; set; }
    }
}
