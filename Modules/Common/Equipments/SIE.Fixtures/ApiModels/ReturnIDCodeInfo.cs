using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 归还ID编码信息
    /// </summary>
    [Serializable]
    public class ReturnIDCodeInfo : ResultDataInfo
    {
        /// <summary>
        /// ID编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 治具编码
        /// </summary>
        public string EncodeCode { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType { get; set; }

        /// <summary>
        /// 可归还数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 管理方式
        /// </summary>
        public string ManageMode { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产线ID
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName { get; set; }
    }
}
