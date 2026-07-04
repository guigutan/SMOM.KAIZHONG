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
    public class AndonAnomalyData
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
        /// 安灯大类
        /// </summary>
        public string AndonClass { get; set; }

        /// <summary>
        /// 安灯类型
        /// </summary>
        public string AnDengType { get; set; }

        /// <summary>
        /// 安灯次数
        /// </summary>
        public int AnDengCount { get; set; }

        /// <summary>
        /// 及时响应次数
        /// </summary>
        public int OnTimeResponseCount { get; set; }

        /// <summary>
        /// 及时处理次数
        /// </summary>
        public int OnTimeProcessCount { get; set; }

        /// <summary>
        /// 及时响应率
        /// </summary>
        public decimal OnTimeResponseRate { get; set; }

        /// <summary>
        /// 异常响应时间（小时）
        /// </summary>
        public decimal ExceptionResponseTime { get; set; }

        /// <summary>
        /// 异常处理时间（小时）
        /// </summary>
        public decimal ExceptionProcessTime { get; set; }

        /// <summary>
        /// 及时处理率
        /// </summary>
        public decimal OnTimeProcessRate { get; set; }
    }
}
