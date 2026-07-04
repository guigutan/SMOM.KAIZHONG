using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 检具维护
    /// </summary>
    [Serializable]
    public class CheckerUpholdData
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
        /// 检具名称
        /// </summary>
        public string CheckerName { get; set; }

        /// <summary>
        /// 有效日期(可空)
        /// </summary>
        public string EffectiveDate { get; set; }

        /// <summary>
        /// 检具类型
        /// </summary>
        public string CheckerType { get; set; }

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNo { get; set; }
    }
}
