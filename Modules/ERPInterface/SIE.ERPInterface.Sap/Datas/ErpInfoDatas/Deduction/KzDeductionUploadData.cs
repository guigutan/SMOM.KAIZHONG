using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Deduction
{
    /// <summary>
    /// 上传数据
    /// </summary>
    [Serializable]
    public class KzDeductionUploadData
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string AUFNR { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string PLANT { get; set; }

        /// <summary>
        /// 过账日期
        /// </summary>
        public string BUDAT { get; set; }

        /// <summary>
        /// 物料号
        /// </summary>
        public string MATNR { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string MEINS { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string MENGE { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string CHARG { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string LGORT { get; set; }

        /// <summary>
        /// 移动类型
        /// </summary>
        public string BWART { get; set; }

        /// <summary>
        /// 移动原因
        /// </summary>
        public string GRUND { get; set; }
    }
}
