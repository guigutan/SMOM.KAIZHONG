using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ProductionAchievementRateData
    {
        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }
        /// <summary>
        /// 厂部名称
        /// </summary>
        public string PlantName { get; set; }
        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName { get; set; }


        /// <summary>
        /// 物料编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 计划产量（万）
        /// </summary>
        public decimal PlanQty { get; set; }
        /// <summary>
        /// 实际产量（万）
        /// </summary>
        public decimal ActualQty { get; set; }

        /// <summary>
        ///排产开始时间
        /// </summary>
        public DateTime? PlanBeginTime { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 生产达成率
        /// </summary>
        public decimal ProductionAchievement { get; set; }
    }

   


}
