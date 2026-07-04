using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Purchases.FixtureReceives.Model
{
    /// <summary>
    /// 工治具编码信息
    /// </summary>
    [Serializable]
    public class FixtureEncodeInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 管控方式
        /// </summary>
        public int ManageMode { get; set; }
        
        /// <summary>
        /// 是否免检
        /// </summary>
        public bool ExemptionInspect { get; set; }

       /// <summary>
       /// 单位Id
       /// </summary>
        public double UnitId { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
       public string UnitName { get; set; }

    }
}
