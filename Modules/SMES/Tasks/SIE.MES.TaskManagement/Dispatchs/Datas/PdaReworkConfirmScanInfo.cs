using Renci.SshNet.Security.Cryptography.Ciphers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// 返工确认扫码信息
    /// </summary>
    [Serializable]
    public class PdaReworkConfirmScanInfo
    {
        
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 批次标签Id
        /// </summary>
        public double WipBatchId { get; set; }

        /// <summary>
        /// 工序标签号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal SnQty { get; set; }
    }
}
