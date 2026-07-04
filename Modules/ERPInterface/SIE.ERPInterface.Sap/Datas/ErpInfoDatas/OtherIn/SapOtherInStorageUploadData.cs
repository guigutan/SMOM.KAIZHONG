using SIE.ERPInterface.Common.Datas;
using System;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.OtherIn
{

    /// <summary>
    /// 其他入库上传数据明细
    /// </summary>

    [Serializable]
    public class SapOtherInStorageUploadDataDetail : SapItemParamBase
    {
        /// <summary>
        /// WMS行号
        /// </summary>
        public string BILL_DTL_NO { get; set; }

        /// <summary>
        /// 其他出库物料凭证行号
        /// </summary>
        public string ZEILE { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 移动类型
        /// </summary>
        public string BWART { get; set; }

        /// <summary>
        /// 收货仓库
        /// </summary>
        public string LGORT { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal ERFMG { get; set; }

        /// <summary>
        /// 基本单位
        /// </summary>
        public string ERFME { get; set; }


        /// <summary>
        /// 特殊库存
        /// </summary>
        public string SOBKZ { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string KUNNR { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string LIFNR { get; set; }

        /// <summary>
        /// 成本中心
        /// </summary>
        public string KOSTL { get; set; }

        /// <summary>
        /// WBS项目号
        /// </summary>
        public string PS_POSNR { get; set; }

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string KDAUF { get; set; }

        /// <summary>
        /// 销售订单行号
        /// </summary>
        public string KDPOS { get; set; }

        /// <summary>
        /// 主资产号
        /// </summary>
        public string ANLN1 { get; set; }

        /// <summary>
        /// 网络
        /// </summary>
        public string NPLNR { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string AUFNR { get; set; }

        /// <summary>
        /// 项目文本
        /// </summary>
        public string SGTXT { get; set; }

        /// <summary>
        /// 发出批次
        /// </summary>
        public string CHARG { get; set; }

        /// <summary>
        /// 接收批次
        /// </summary>
        public string UMCHA { get; set; }

        /// <summary>
        /// 过账金额
        /// </summary>
        public string EXBWR { get; set; }

        /// <summary>
        /// 收货方
        /// </summary>
        public string WEMPF { get; set; }


    }

    /// <summary>
    /// 其他入库上传数据
    /// </summary>
    [Serializable]
    public class SapOtherInStorageUploadData<T> : SapOrderParamBase<T>
    {
        /// <summary>
        /// WMS单号
        /// </summary>
        public string BILL_NO { get; set; }

        /// <summary>
        /// 其他出库物料凭证号
        /// </summary>
        public string MBLNR { get; set; }

    }

}
