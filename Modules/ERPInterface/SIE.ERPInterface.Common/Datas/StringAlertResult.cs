using SIE.Common.Alert;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 推送结果(字符串)
    /// </summary>
    [Serializable]
    public class StringAlertResult : AlertResultBase
    {
        /// <summary>
        /// 推送信息
        /// </summary>
        [Label("推送信息")]
        public string Message { get; set; }
    }
}
