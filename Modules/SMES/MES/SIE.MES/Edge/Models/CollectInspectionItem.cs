using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 采集检验项目结果
    /// </summary>
    [Serializable]
    public class CollectInspectionItem
    {
        /// <summary>
        /// 检验项目Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 检验标识
        /// </summary>

        public Defects.InspectionItems.CheckTag CheckTag { get; set; }

        /// <summary>
        /// 规范上限
        /// </summary>
        public decimal? LimitMax { get; set; }

        /// <summary>
        /// 规范下限
        /// </summary>
        public decimal? LimitLow { get; set; }

        /// <summary>
        /// 测试值
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// 检验结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
