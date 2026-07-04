using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.APIModel
{
    /// <summary>
    /// 安灯统计信息
    /// </summary>
    [Serializable]
    public class AndonManageGroupByData
    {
        /// <summary>
        /// 车间ID
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 工厂代码
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 车间代码
        /// </summary>
        public string WorkShopCode { get; set; }

        /// <summary>
        /// 安灯大类
        /// </summary>
        public  string AndonBigType { get; set; }

        /// <summary>
        /// 安灯类型
        /// </summary>
        public string AndonType { get; set; }

        /// <summary>
        /// 安灯次数
        /// </summary>
        public int AndonNum { get; set; }

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
