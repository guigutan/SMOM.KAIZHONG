using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// ERP通信返回错误信息类
    /// </summary>
    [Serializable]
    public class ErpErrorData
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
