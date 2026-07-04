using SIE.Defects.InspectionItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘机型检验项目
    /// </summary>
    public class EdgeInspectionItemInfo
    {
        /// <summary>
        /// 缺陷代码
        /// </summary>
        public List<EdgeInspectionItem> InspectionItems { get; set; } = new List<EdgeInspectionItem>();

        /// <summary>
        /// 上一次更新
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }

        /// <summary>
        /// 下一次更新
        /// </summary>
        public DateTime NextRefreshTime { get; set; }
    }

    /// <summary>
    /// 机型检验项目
    /// </summary>
    [Serializable]
    public class EdgeInspectionItem
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 机型Id
        /// </summary>
        public double ProductModelId { get; set; }

        /// <summary>
        /// 检验项目
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 检验标识
        /// </summary>
        public CheckTag CheckTag { get; set; }

        /// <summary>
        /// 规格下限判断符号
        /// </summary>
        public CompareType? LimitLowCompare { get; set; }

        /// <summary>
        /// 规格上限判断符号
        /// </summary>
        public CompareType? LimitMaxCompare { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LimitLow { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? LimitMax { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }
    }
}
