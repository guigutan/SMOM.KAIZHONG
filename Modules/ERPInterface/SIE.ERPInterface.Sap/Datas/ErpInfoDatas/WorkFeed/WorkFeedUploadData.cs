using SIE.ERPInterface.Common.Datas;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.WorkFeed
{
    /// <summary>
    /// 工单发料上传数据
    /// </summary>
    [Serializable]
    public class WorkFeedUploadData: SapItemParamBase
    {
        /// <summary>
        /// 行号
        /// </summary>
        public string BILL_DTL_NO { get; set; }

        /// <summary>
        /// （工厂）库存组织
        /// </summary>
        public string WERKS { get; set; }

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
        /// 工单号
        /// </summary>
        public string AUFNR { get; set; }

        /// <summary>
        /// 生产批次
        /// </summary>
        public string CHARG { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string BWART { get; set; }
    }

    /// <summary>
    /// 工单发料单头数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SapOrderParamWorkFeed<T> : SapOrderParamBase<T>
    {
        /// <summary>
        /// WMS单号
        /// </summary>
        public string BILL_NO { get; set; }
    }
}
