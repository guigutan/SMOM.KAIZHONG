using SIE.Core.Enums;
using SIE.ERPInterface.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas
{
    #region EBS参数
    /// <summary>
    /// Ebs报文参数
    /// </summary>
    [Serializable]
    public class EbsParameter
    {
        /// <summary>
        /// WebService URI
        /// </summary>
        public string DownloadUri { get; set; }

        /// <summary>
        /// WebService URI
        /// </summary>
        public string UploadUri { get; set; }

        /// <summary>
        /// 业务实体ID,ERP固定值81
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// ERP库存组织Id
        /// </summary>
        public string InvOrgId { get; set; }

        /// <summary>
        /// 授权码，通过postman可以获取
        /// </summary>
        public string Authorization { get; set; }

        /// <summary>
        /// 接口编码
        /// </summary>
        public string InterfaceCode { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string InterfaceName { get; set; }

        /// <summary>
        /// 下载参数
        /// </summary>
        public EbsDownParameter DownParameter { get; set; } = new EbsDownParameter();

        /// <summary>
        /// 上传的报文
        /// </summary>
        public string UploadStr { get; set; }

        /// <summary>
        /// 上传的类型
        /// </summary>
        public JobType UploadJobType { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public OrderType OrderType { get; set; }
    }

    /// <summary>
    /// 下载参数
    /// </summary>
    [Serializable]
    public class EbsDownParameter
    {
        /// <summary>
        /// 接口编码
        /// </summary>
        public int PageSize { get; set; } = 1000;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { get; set; } = 1;

        /// <summary>
        /// 最后拉取时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; } = DateTime.Parse("1900-01-01 00:00:01");

        /// <summary>
        /// ERP分组Id,大量数据的时候用，可自定义，也可使用返回的XV_GROUP_ID
        /// </summary>
        public string PN_GROUP_ID { get; set; } = "";

        /// <summary>
        /// 自定义参数报文
        /// </summary>
        public string ParaStr { get; set; }
    }
    #endregion

    
}
