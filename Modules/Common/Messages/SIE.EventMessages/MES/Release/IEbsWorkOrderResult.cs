using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Release
{
    /// <summary>
    /// 处理结果
    /// </summary>
    [Serializable]
    public class IEbsWorkOrderResult
    {
        /// <summary>
        /// 数据量
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 成功数量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailCount { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public List<DetailDataInfo> ErpErrorDatas { get; set; } = new List<DetailDataInfo>();
    }

    /// <summary>
    /// 明细信息
    /// </summary>
    [Serializable]
    public class DetailDataInfo
    {
        /// <summary>
        /// ERP通信的中间表Key（子错误存子的key）
        /// </summary>
        public string Infkey { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 是否是子错误	
        /// </summary>
        public bool IsChild { get; set; }

        /// <summary>
        /// 是否成功写入,ERP要求成功写入的数据也要回传
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
