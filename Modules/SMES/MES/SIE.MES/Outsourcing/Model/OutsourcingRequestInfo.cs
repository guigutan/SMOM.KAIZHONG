using System;

namespace SIE.MES.Outsourcing.Model
{
    /// <summary>
    /// 委外需求单信息
    /// </summary>
    [Serializable]
    public class OutsourcingRequestInfo
    {
        /// <summary>
        /// 需求单Id
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 委外需求单号
        /// </summary>
        public string RequestNo { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 起始工序
        /// </summary>
        public string BeginProcess { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal RequestQty { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        public decimal OutboundQty { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal InboundQty { get; set; }

        /// <summary>
        /// 结束工序
        /// </summary>
        public string EndProcess { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProduceCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProduceName { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 工序委外需求单状态
        /// </summary>
        public int OutsourcingState { get; set; }

        /// <summary>
        /// 工序委外需求单状态
        /// </summary>
        public string OutsourcingStateDisplay { get; set; }
    }

    /// <summary>
    /// 委外出库入库明细信息
    /// </summary>
    [Serializable]
    public class RequestDetailInfo
    {

        /// <summary>
        /// 生产条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 来源Id
        /// </summary>
        public double SourceId { get; set; }

        /// <summary>
        /// 出库Id
        /// </summary>
        public double? OutboundId { get; set; }
    }
}
