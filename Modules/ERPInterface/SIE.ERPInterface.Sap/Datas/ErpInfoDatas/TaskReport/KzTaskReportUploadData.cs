using SIE.ERPInterface.Common.Datas;
using System;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.SaleReturn
{
    

    /// <summary>
    /// 凯中报工上传数据
    /// </summary>
    [Serializable]
    public class KzTaskReportUploadData
    {

        /// <summary>
        /// 工单号
        /// </summary>
        public string AUFNR { get; set; }


        /// <summary>
        /// 工序工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 操作/活动编号 (工序流水号)
        /// </summary>
        public string VORNR { get; set; }


        /// <summary>
        /// 工序文本编码（工序编码）
        /// </summary>
        public string KTSCH { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string ARBPL { get; set; }

        

        /// <summary>
        /// 待确认数量（良品数量）
        /// </summary>
        public decimal YIELD { get; set; }


        /// <summary>
        /// 特殊良品数量-可疑品数量 (返工数)
        /// </summary>
        public decimal ZTQTY { get; set; }

        /// <summary>
        /// 废品数量（不良品数量）
        /// </summary>
        public decimal SCRAP { get; set; }

        /// <summary>
        /// 过账日期（报工日期）
        /// </summary>
        public string BUDAT { get; set; }


        /// <summary>
        /// MES系统唯一ID
        /// </summary>
        public string ZUID { get; set; }


    }
}
