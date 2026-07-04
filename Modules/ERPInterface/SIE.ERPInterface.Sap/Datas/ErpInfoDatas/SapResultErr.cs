using SapNwRfc;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Datas
{
    /// <summary>
    /// 返回结果
    /// </summary>
    [Serializable]
    public class SapResultErr<T> : SapResultBase where T : class
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        [SapName("ERR_DATA")]
        public T[] ERR_DATA { get; set; }
    }

    /// <summary>
    /// 返回错误信息类
    /// </summary>
    [Serializable]
    public class SapErrorData
    {
        /// <summary>
        /// 成功失败字段
        /// </summary>
        public string E_CODE { get; set; }

        /// <summary>
        /// Key单据号
        /// </summary>
        public string INF_KEY { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ERR_MSG { get; set; }

        /// <summary>
        /// 事务上传ID
        /// </summary>
        public double UploadTransactionId { get; set; }
    }

    /// <summary>
    /// 返回错误信息类
    /// </summary>
    [Serializable]
    public class SapMoreErrorData : SapErrorData
    {

        /// <summary>
        /// 物料凭证号
        /// </summary>
        public string MATERUAL_VOUCHER_CODE { get; set; }
        /// <summary>
        /// 凭证日期
        /// </summary>
        public string DOCUMENT_DATE { get; set; }

        /// <summary>
        /// 过账日期
        /// </summary>
        public string POSTING_DATE { get; set; }

    }
}
