using SIE.Defects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 边缘缺陷信息
    /// </summary>
    public class EdgeDefectInfo
    {
        /// <summary>
        /// 缺陷代码
        /// </summary>
        public List<EdgeDefect> Defects { get; set; } = new List<EdgeDefect>();

        /// <summary>
        /// 缺陷分类
        /// </summary>
        public List<EdgeDefectCategory> DefectCategories { get; set; } = new List<EdgeDefectCategory>();

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public List<EdgeDefectResponsibility> DefectResponsibilities { get; set; } = new List<EdgeDefectResponsibility>();


        /// <summary>
        /// 缺陷责任分类
        /// </summary>
        public List<EdgeDefectResponsibilityCategory> DefectResponsibilityCategories { get; set; } = new List<EdgeDefectResponsibilityCategory>();

        /// <summary>
        /// 维修措施
        /// </summary>

        public List<EdgeRepairMeasure> RepairMeasures { get; set; } = new List<EdgeRepairMeasure>();

        /// <summary>
        /// 上一次更新
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }

        /// <summary>
        /// 下一次更新
        /// </summary>
        public DateTime NextRefreshTime { get; set; }
    }
}
