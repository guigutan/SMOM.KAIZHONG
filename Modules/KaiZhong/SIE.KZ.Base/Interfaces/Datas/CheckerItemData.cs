using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 检具与产品的关系
    /// </summary>
    [Serializable]
    public class CheckerItemData
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 检具编码
        /// </summary>
        public string CheckerCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 工序编码(可空)
        /// </summary>
        public string ProcessCode { get; set; }

        /// <summary>
        /// 图号(可空)
        /// </summary>
        public string DrawingNo { get; set; }
    }
}
