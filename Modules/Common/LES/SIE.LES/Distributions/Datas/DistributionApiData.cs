using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.Distributions.Datas
{
    /// <summary>
    /// 配送管理扫描识别数据
    /// </summary>
    public class ScanData
    {
        /// <summary>
        /// 扫描类型 1-容器编码 2-配送单号 3-标签 4-发运单 5-物料编码
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 扫描的内容
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string  Lpn { get; set; }

        /// <summary>
        /// 配送单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 配送单数据
        /// </summary>
        public List<DistributionNoList> DistributionNoList { get; set; }
    }

    /// <summary>
    /// 配送单数据
    /// </summary>
    public class DistributionNoList
    {
        /// <summary>
        /// 配送单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string Lpn { get; set; }

        /// <summary>
        /// 单据ID
        /// </summary>
        public double BillID { get; set; }

        /// <summary>
        /// 目标产线编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 目标产线名称
        /// </summary>
        public string ProductName { get; set; }
    }

    /// <summary>
    /// 扫描数据
    /// </summary>
    public class DistributionScanData
    {
        /// <summary>
        /// 配送单信息
        /// </summary>
        public List<DistributionDetailData> DistributionList { get; set; }
    }

    /// <summary>
    /// 配送明细
    /// </summary>
    public class DistributionDetailList
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
    }

    /// <summary>
    /// 扫描明细数据
    /// </summary>
    public class DistributionDetailData: DistributionNoList
    {
        /// <summary>
        /// 配送明细信息
        /// </summary>
        public List<DistributionDetailList> DistributionDetailList { get; set; }
    }

   
}
