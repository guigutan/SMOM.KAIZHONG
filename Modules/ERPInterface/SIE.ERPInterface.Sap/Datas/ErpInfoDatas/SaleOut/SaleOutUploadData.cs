using SIE.ERPInterface.Common.Datas;
using System;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.SaleOut
{
    /// <summary>
    /// 销售出库上传数据明细
    /// </summary>
    [Serializable]
    public class SapSaleOutUploadDetail : SapItemParamBase
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 交货单行号
        /// </summary>
        public string POSNR { get; set; }

        /// <summary>
        /// 发货仓库
        /// </summary>
        public string LGORT { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal MENGE { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string MEINS { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string CHARG { get; set; }

        /// <summary>
        /// 辅助单位
        /// </summary>
        public string VRKM1 { get; set; }
    }

    /// <summary>
    /// 销售出库上传数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SapOrderParamSale<T> : SapOrderParamBase<T>
    {
        /// <summary>
        /// 交货单号
        /// </summary>
        public string VBELN { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string KUNNR { get; set; }
    }
}
