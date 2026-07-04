using SIE.ERPInterface.Common.Datas;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.OutPurchase
{
    /// <summary>
    /// 委外出库上传数据
    /// </summary>
    [Serializable]
    public class UploadOutPurchaseData : SapItemParamBase
    {
        /// <summary>
        /// 生产批次
        /// </summary>
        public string CHARG { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string BWART { get; set; }

        /// <summary>
        /// 发货仓库编码
        /// </summary>
        public string LGORT { get; set; }

        /// <summary>
        /// 数量（主）
        /// </summary>
        public decimal ERFMG { get; set; }

        /// <summary>
        /// 主单位编码
        /// </summary>
        public string ERFME { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string BILL_DTL_NO { get; set; }

        /// <summary>
        /// 采购订单
        /// </summary>
        public string EBELN { get; set; }

        /// <summary>
        /// 采购订单行号
        /// </summary>
        public string EBELP { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string LIFNR { get; set; }

        /// <summary>
        /// 工厂(库存组织)
        /// </summary>
        public string WERKS { get; set; }
    }

    /// <summary>
    /// 委外退料数据
    /// </summary>
    [Serializable]
    public class UploadOutPurchaseInData: SapItemParamBase
    {
        /// <summary>
        /// 行号
        /// </summary>
        public string BILL_DTL_NO { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string BWART { get; set; }

        /// <summary>
        /// 采购订单
        /// </summary>
        public string EBELN { get; set; }

        /// <summary>
        /// 采购订单行号
        /// </summary>
        public string EBELP { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal ERFMG { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string ERFME { get; set; }

        /// <summary>
        /// 库存地点(入库仓库编码)
        /// </summary>
        public string LGORT { get; set; }

        /// <summary>
        /// 生产批次
        /// </summary>
        public string CHARG { get; set; }

        /// <summary>
        /// 库存组织
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string LIFNR { get; set; }
    }

    /// <summary>
    /// 委外出库单头数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SapOrderParamOutPur<T> : SapOrderParamBase<T>
    {
        /// <summary>
        /// wms单号
        /// </summary>
        public string BILL_NO { get; set; }
    }
}
