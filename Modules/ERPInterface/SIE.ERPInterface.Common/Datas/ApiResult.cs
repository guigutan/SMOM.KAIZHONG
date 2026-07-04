using SIE.EventMessages;
using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// API接口处理结果
    /// </summary>
    [Serializable]
    public class ApiResult
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
        public List<ErpErrorData> ErpErrorDatas { get; set; } = new List<ErpErrorData>();
    }
}
