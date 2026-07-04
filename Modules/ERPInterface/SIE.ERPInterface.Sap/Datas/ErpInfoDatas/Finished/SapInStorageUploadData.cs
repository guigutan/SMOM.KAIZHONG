using SIE.ERPInterface.Common.Datas;
using System;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Finished
{
    /// <summary>
    /// 成品入库上传数据明细
    /// </summary>

    [Serializable]
    public class SapInStorageUploadDataDetail : SapItemParamBase
    {
        /// <summary>
        /// 移动类型
        /// </summary>
        public string BWART { get; set; }

        /// <summary>
        /// WMS行号
        /// </summary>
        public string BILL_DTL_NO { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 库存地点
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
        /// 生产订单
        /// </summary>
        public string AUFNR { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string CHARG { get; set; }
    }

    /// <summary>
    /// 成品入库上传数据
    /// </summary>
    [Serializable]
    public class SapInStorageUploadData<T> : SapOrderParamBase<T>
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string BILL_NO { get; set; }

    }

}
