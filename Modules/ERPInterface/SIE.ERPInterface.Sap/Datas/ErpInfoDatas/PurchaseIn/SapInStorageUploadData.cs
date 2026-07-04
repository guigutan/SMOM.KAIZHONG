using SIE.ERPInterface.Common.Datas;
using System;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.PurchaseIn
{

    /// <summary>
    /// 采购入库上传数据明细
    /// </summary>

    [Serializable]
    public class SapInStorageUploadDataDetail : SapItemParamBase
    {

        /// <summary>
        /// 移动类型
        /// </summary>
        public string BWART { get; set; }

        /// <summary>
        /// SRM行号
        /// </summary>
        public string SGTXT { get; set; }

        /// <summary>
        /// 采购凭证
        /// </summary>
        public string EBELN { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public string EBELP { get; set; }

        /// <summary>
        /// 库存地点
        /// </summary>
        public string LGORT { get; set; }

        /// <summary>
        /// 基本单位数量
        /// </summary>
        public decimal ERFMG { get; set; }

        /// <summary>
        /// 基本单位
        /// </summary>
        public string ERFME { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string CHARG { get; set; }

        /// <summary>
        /// 目标单位
        /// </summary>
        public string TARME { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string LIFNR { get; set; }

        /// <summary>
        /// 暂收凭证号
        /// </summary>
        public string LFBNR { get; set; }

        /// <summary>
        /// 暂收凭证行号
        /// </summary>
        public string LFPOS { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        
    }

    /// <summary>
    /// 采购入库上传数据
    /// </summary>
    [Serializable]
    public class SapInStorageUploadData<T> : SapOrderParamBase<T>
    {
        /// <summary>
        /// SRM单号
        /// </summary>
        public string ZSRMID { get; set; }

        /// <summary>
        /// 抬头文本
        /// </summary>
        public string BKTXT { get; set; }

    }

}
