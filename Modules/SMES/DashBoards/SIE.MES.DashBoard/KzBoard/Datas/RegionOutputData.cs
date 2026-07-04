using Irony.Parsing;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzBoard.Datas
{
    /// <summary>
    /// 区域生产产量看板
    /// </summary>
    [Serializable]
    public class RegionOutputData
    {

        /// <summary>
        /// 当班计划
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 累计数量
        /// </summary>
        public decimal CumulativePlan { get; set; }

        /// <summary>
        /// 累计实际
        /// </summary>
        public decimal CumulativeActive { get; set; }

        /// <summary>
        /// 差异数量
        /// </summary>
        public decimal DifferenceQty { get; set; }

        /// <summary>
        /// 连续安全生产天数
        /// </summary>
        public decimal SafetyProductionDays { get; set; }

        /// <summary>
        /// 小时产量
        /// </summary>
        public List<HourOutputData> gridData1 { get; set; } = new List<HourOutputData>();

        /// <summary>
        /// 生产产量
        /// </summary>
        public List<OutputData> gridData2 { get; set; } = new List<OutputData>();
    }

    /// <summary>
    /// 小时产量
    /// </summary>
    [Serializable]
    public class HourOutputData
    { 
        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 计划
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 实际
        /// </summary>
        public decimal ActiveQty { get; set; }

        /// <summary>
        /// 差异
        /// </summary>
        public decimal DifferenceQty { get; set; }
    }

    /// <summary>
    /// 生产产量
    /// </summary>
    [Serializable]
    public class OutputData
    { 
        /// <summary>
        /// 生产资源
        /// </summary>
        public string WipResource { get; set; }

        /// <summary>
        /// 生产资源名称
        /// </summary>
        public string WipResourceName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// 计划
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 实际
        /// </summary>
        public decimal ActiveQty { get; set; }

        /// <summary>
        /// 差异
        /// </summary>
        public decimal DifferenceQty { get; set; }
    }
}
