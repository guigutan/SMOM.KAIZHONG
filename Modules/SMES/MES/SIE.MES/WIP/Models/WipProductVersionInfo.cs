using SIE.MES.WIP.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WIP.Models
{
    /// <summary>
    /// (批次)生产采集记录信息
    /// </summary>
    [Serializable]
    public class WipProductVersionInfo
    {
        /// <summary>
        /// 父Id
        /// </summary>
        public double ParentId { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? WipResourceId { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public string WipResourceCode { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId {  get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName {  get; set; }

        /// <summary>
        /// 采集状态
        /// </summary>
        public WipProductProcessState State { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OperateTime { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 是否批次
        /// </summary>
        public bool IsBatch { get; set; }

        /// <summary>
        /// 入站时间
        /// </summary>
        public DateTime? InTime { get; set; }

        /// <summary>
        /// 出站时间
        /// </summary>
        public DateTime? OutTime { get; set; }
    }
}
