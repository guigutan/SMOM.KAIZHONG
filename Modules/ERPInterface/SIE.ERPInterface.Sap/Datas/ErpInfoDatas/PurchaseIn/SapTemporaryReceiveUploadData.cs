using SIE.ERPInterface.Common.Datas;
using System;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.PurchaseIn
{

    /// <summary>
    /// 采购送货单暂收上传数据明细
    /// </summary>

    [Serializable]
    public class SapTemporaryReceiveUploadDataDetail : SapItemParamBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string WERKS { get; set; }

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
        /// 采购单位
        /// </summary>
        public string TARME { get; set; }
    }

    /// <summary>
    /// 采购送货单暂收上传数据
    /// </summary>
    [Serializable]
    public class SapTemporaryReceiveUploadData<T> : SapOrderParamBase<T>
    {
        /// <summary>
        /// SRM单号
        /// </summary>
        public string ZSRMID { get; set; }

    }
   
}
