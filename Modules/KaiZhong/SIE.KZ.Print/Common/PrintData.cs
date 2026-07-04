using SIE.Barcodes.WipBatchs;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.KZ.Print.Common
{
    /// <summary>
    /// 打印实体
    /// </summary>
    [Serializable]
    public class PrintData
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public int InvOrgId { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrinterName { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 打印模板ID
        /// </summary>
        public double PrintTemplateId { get; set; }
        /// <summary>
        /// 打印日志Id
        /// </summary>
        public double PrintLogId { get; set; }
    }

    /// <summary>
    /// 批次标签打印实体
    /// </summary>
    [Serializable]
    public class WipBatchData : PrintData
    {
        /// <summary>
        /// 待打印的数据
        /// </summary>
        public List<PrintInfo> Data { get; set; }

    }

    [Serializable]
    public class PrintInfo 
    {
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
        /// 蓝牙打印指令
        /// </summary>
        public string PrintCmd { get; set; }
        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? PrintTemplateId { get; set; }

    }
}
