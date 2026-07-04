using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.AndonAverageTime.Datas
{
    /// <summary>
    /// 
    /// </summary>
    public class AverageData
    {
        /// <summary>
        /// 响应人
        /// </summary>
        public string Responser { get; set; }
        /// <summary>
        /// 响应时长
        /// </summary>
        public decimal ResponseDuration { get; set; }
        /// <summary>
        /// 响应时长非0次数
        /// </summary>
        public decimal ResponseDurationNotNullCount { get; set; }




        /// <summary>
        /// 处理人
        /// </summary>
        public string Handler { get; set; }
        /// <summary>
        /// 处理时长
        /// </summary>
        public decimal HandlerDuration { get; set; }
        /// <summary>
        /// 处理时长非0次数
        /// </summary>
        public decimal HandlerDurationNotNullCount { get; set; }






        /// <summary>
        /// 验收人
        /// </summary>
        public string Checker { get; set; }
        /// <summary>
        /// 验收时长
        /// </summary>
        public decimal CheckerDuration { get; set; }
        /// <summary>
        /// 验收时长非0次数
        /// </summary>

        public decimal CheckerDurationNotNullCount { get; set; }
    }

    /// <summary>
    /// 安灯操作日志数据
    /// </summary>
    public class OperateLogData
    {
        /// <summary>
        /// 安灯编码ID
        /// </summary>
        public decimal AndonManageId { get; set; }
        /// <summary>
        /// 安灯类型ID
        /// </summary>
        public decimal OperateType { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public decimal OperateBy { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OperateTime { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class AndonAverageTimeData
    {
        /// <summary>
        /// 安灯编码ID
        /// </summary>
        public decimal AndonManageId { get; set; }


        /// <summary>
        ///  响应时长（秒）
        /// </summary>
        public double? ResponseDurationSec { get; set; }
        /// <summary>
        /// 响应人（最后一个响应操作的操作人ID）
        /// </summary>
        public decimal? ResponseBy { get; set; }



        /// <summary>
        /// 处理时长（秒）
        /// </summary>
        public double? HandleDurationSec { get; set; }
        /// <summary>
        /// 处理人（最后一个处理完成操作的操作人ID）
        /// </summary>
        public decimal? HandleBy { get; set; }



        /// <summary>
        /// 验收时长（秒）
        /// </summary>
        public double? CheckDurationSec { get; set; }        
        /// <summary>
        ///  验收人（最后一个验收操作的操作人ID）
        /// </summary>
        public decimal? CheckBy { get; set; }     

    }

}