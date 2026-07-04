using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// SOAP报文参数
    /// </summary>
    [Serializable]
    public class SoapParameter
    {
        /// <summary>
        /// WebService URI
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 职责信息
        /// </summary>
        public string Responsibility { get; set; }

        /// <summary>
        /// 职责应用短码
        /// </summary>
        public string RespApplication { get; set; }

        /// <summary>
        /// 安全组
        /// </summary>
        public string SecurityGroup { get; set; }

        /// <summary>
        /// 语种
        /// </summary>
        public string NLSLanguage { get; set; }

        /// <summary>
        /// 业务实体ID
        /// </summary>
        public int Org_Id { get; set; }

        /// <summary>
        /// 调用接口名称
        /// </summary>
        public string P_SERVICE_NAME { get; set; }

        /// <summary>
        /// 调用者系统标识
        /// </summary>
        public string P_ORIG_SYSTEM { get; set; }

        /// <summary>
        /// 调用流水ID
        /// </summary>
        public double P_BATCH_ID { get; set; }

        /// <summary>
        /// 下载参数
        /// </summary>
        public SoapDownloadParameter SoapDownloadParameter { get; set; } = new SoapDownloadParameter();

        /// <summary>
        /// 上传参数
        /// </summary>
        public List<SoapUploadParameterDtl> SoapUploadParameterDtlList { get; } = new List<SoapUploadParameterDtl>();
    }

    /// <summary>
    /// SOAP报文下载参数
    /// </summary>
    [Serializable]
    public class SoapDownloadParameter
    {
        /// <summary>
        /// 调用接口标签
        /// </summary>
        public string Code_Label { get; set; }

        /// <summary>
        /// 实体主键
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 最后更新时间(到)
        /// </summary>
        public string LAST_UPDATE_DATE_TO { get; set; }

        /// <summary>
        /// 最后更新时间(从)
        /// </summary>
        public string LAST_UPDATE_DATE_FROM { get; set; }
    }

    /// <summary>
    /// SOAP报文上传参数
    /// </summary>
    [Serializable]
    public class SoapUploadParameterDtl
    {
        /// <summary>
        /// 行号
        /// </summary>
        public string LINE_ID { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WIP_ENTITY_NAME { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string MOVE_QUANTITY { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string MOVE_TYPE { get; set; }

        /// <summary>
        /// 事务上传ID
        /// </summary>
        public double UploadTransactionId { get; set; }
    }
}
