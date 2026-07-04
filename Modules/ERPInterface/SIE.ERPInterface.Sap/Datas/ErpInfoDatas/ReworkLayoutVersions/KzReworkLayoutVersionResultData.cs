using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.ReworkLayoutVersions
{
    /// <summary>
    /// 返工信息返工数据
    /// </summary>
    [Serializable]
    public class KzReworkLayoutVersionResultData
    {
        /// <summary>
        /// 生产订单
        /// </summary>
        public string AUFNR { get; set; }

        /// <summary>
        /// 卸货点
        /// </summary>
        public string ABLAD { get; set; }

        /// <summary>
        /// 反馈标识(S代表成功，E代表失败)
        /// </summary>
        public string ZFKBS { get; set; }

        /// <summary>
        /// 反馈信息
        /// </summary>
        public string ZFKXX { get; set; }
    }
}
