using SIE.Common;
using System;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 检验项目
    /// </summary>
    [Serializable]
    public class InspectionItem
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public double InspectItemId { get; set; }

        /// <summary>
        /// 项目测试值
        /// </summary>
        public decimal? InspectionValue { get; set; }

        /// <summary>
        /// 项目判定结果
        /// </summary>
        public ResultType ItemResult { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}