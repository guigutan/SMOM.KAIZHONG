using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// ASN明细数据
    /// </summary>
    [Serializable]
    public class EbsToSmomAsnDetailData: EbsOrderBaseData
    {        
        /// <summary>
        /// 订单类型(单据大类)
        /// </summary>
        public int OrderType { get; set; }
       
        /// 交货日期
        /// </summary>
        public DateTime? DeliveryDate { get; set; }
       
        /// <summary>
        /// 交接人
        /// </summary>
        public string Connecter { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 货主编码
        /// </summary>
        public string ShipperCode { get; set; }

        /// <summary>
        /// 明细行状态
        /// </summary>
        public int LineState { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string ErpLineNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 预期数量
        /// </summary>
        public decimal ExpectQty { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string UnitCode { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? LotAtt01 { get; set; }

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? LotAtt02 { get; set; }

        /// <summary>
        /// 生产批次
        /// </summary>
        public string ProductBatch { get; set; }
            
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 明细来源Id
        /// </summary>
        public string ErpSplitFromDetailId { get; set; }

        /// <summary>
        /// 拆分来源行的状态，0有效 1失效
        /// </summary>
        public int? ErpSplitFromState { get; set; }

        /// <summary>
        /// ERP拆分行号
        /// </summary>
        public string ErpSplitLineNo { get; set; }
    }

}
