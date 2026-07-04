using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 报工返回数据
    /// </summary>
    [Serializable]
    public class ReportReturnData
    {
        /// <summary>
        /// 订单号 
        /// </summary>
        public string AUFNR { get; set; }

        /// <summary>
        /// 工作中心 
        /// </summary>
        public string ARBPL { get; set; }

        /// <summary>
        /// MOM系统唯一ID 
        /// </summary>
        public string ZUID { get; set; }

        /// <summary>
        /// 反馈标识S成功，E失败 
        /// </summary>
        public string ZFKBS { get; set; }

        /// <summary>
        /// 返回消息 
        /// </summary>
        public string ZFKXX { get; set; }

        /// <summary>
        /// 工序工厂 
        /// </summary>
        public string WERKS { get; set; }
    }

  
}
