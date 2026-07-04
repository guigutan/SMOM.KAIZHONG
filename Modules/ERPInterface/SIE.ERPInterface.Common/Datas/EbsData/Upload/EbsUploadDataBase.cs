using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 上传数据基类
    /// </summary>
    [Serializable]
    public class EbsUploadDataBase
    {
        /// <summary>
        /// 来源编号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 来源行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 事务交易Id
        /// </summary>
        public double TranId { get; set; }

        /// <summary>
        /// 业务实体
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 库存组织
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// 订单行ID
        /// </summary>
        public string BillLineErpKey { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string UnitCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 子库
        /// </summary>
        public string ErpWarehouseCode { get; set; }

        /// <summary>
        /// 货位
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// 生产批号
        /// </summary>
        public string ProductBatch { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        public string TransactionDate { get; set; }

        /// <summary>
        /// 请求报文
        /// </summary>
        public string RequestStr { get; set; }       

        /// <summary>
        /// 上传事务Id
        /// </summary>
        public double UploadTranId { get; set; }
    }

}
