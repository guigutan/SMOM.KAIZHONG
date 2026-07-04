using SIE.Barcodes.WipBatchs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports.Datas
{
    /// <summary>
    /// 标签信息
    /// </summary>
    [Serializable]
    public class PdaPrintInfo { 
    
        /// <summary>
        /// 标签号
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// 是否可疑品标签
        /// </summary>
        public bool IsSuspectProduct { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 蓝牙打印指令
        /// </summary>
        public string PrintCmd { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string DateTime { get; set; }

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? PrintTemplateId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
    }
}
