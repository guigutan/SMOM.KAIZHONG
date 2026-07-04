using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// SAPBase
    /// </summary>
    [Serializable]
    public class SapBase
    {
        /// <summary>
        /// SAP返回状态，E错误 S成功
        /// </summary>
        public string MSGTY { get; set; }

        /// <summary>
        /// SAP返回错误信息
        /// </summary>
        public string MSGTX { get; set; }
    }

    /// <summary>
    /// 接口结果
    /// </summary>
    [Serializable]
    public class SapResult
    {
        /// <summary>
        /// 接口是否访问成功
        /// </summary>
        public bool IsSuccess { get; set; } = true;
        /// <summary>
        /// 请求报文
        /// </summary>
        public string RequestStr { get; set; }
        /// <summary>
        /// 接收报文
        /// </summary>
        public string ResponseStr { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime ResponseDate { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string InterfaceName { get; set; }

        /// <summary>
        /// SAP返回报文序列后的数据
        /// </summary>
        public SapResultData SapResultData { get; set; } = new SapResultData();
        /// <summary>
        /// SAP上传返回数据
        /// </summary>
        public SapUploadResultData SapUploadResultData { get; set; } = new SapUploadResultData();

    }
    /// <summary>
    /// SAP上传返回报文序列后的数据
    /// </summary>
    [Serializable]
    public class SapUploadResultData {
        /// <summary>
        /// S成功，E失败
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
    }
    /// <summary>
    /// SAP返回报文序列后的数据
    /// </summary>
    [Serializable]
    public class SapResultData : SapBase
    {
        /// <summary>
        /// 数据处理结果
        /// </summary>
        public List<SapDetailData> RETURN { get; set; }

        /// <summary>
        /// 本次请求的KEY,GUID值
        /// </summary>
        public string DATAKEY { get; set; }

    }

    /// <summary>
    /// 返回明细数据
    /// </summary>
    [Serializable]
    public class SapDetailData : SapBase
    {         
        /// <summary>
        /// 握手的Key，行Key返回的是单据Key-行Key,如果单据报错就只会返回单据Key
        /// </summary>
        public string EXDOCKEY { get; set; }

        /// <summary>
        /// SAP返回凭证（凭证号-凭证行号）
        /// </summary>
        public string SAPKEY { get; set; }        
    }
}
