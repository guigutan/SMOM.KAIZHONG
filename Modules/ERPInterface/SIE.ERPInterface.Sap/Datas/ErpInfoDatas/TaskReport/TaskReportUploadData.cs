using SIE.ERPInterface.Common.Datas;
using System;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.SaleReturn
{
    

    /// <summary>
    /// 报工上传数据
    /// </summary>
    [Serializable]
    public class TaskReportUploadData
    {

        /// <summary>
        /// 工单号
        /// </summary>
        public string AUFNR { get; set; }

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal LMNGA { get; set; }
        

        /// <summary>
        /// 工序编码
        /// </summary>
        public string VORNR { get; set; }

        /// <summary>
        /// （实际完成时间-实际开始时间  ）H  * （投入人数
        /// </summary>
        public string LAR01 { get; set; }


        /// <summary>
        /// （实际完成时间-实际开始时间  ）H  * （投入人数
        /// </summary>
        public string VGW01 { get; set; }


        /// <summary>
        /// （实际完成时间-实际开始时间  ）H  
        /// </summary>
        public string LAR02 { get; set; }

        /// <summary>
        /// （实际完成时间-实际开始时间  ）H  
        /// </summary>
        public string LAR03 { get; set; }

        /// <summary>
        /// （实际完成时间-实际开始时间  ）H  * （投入人数）
        /// </summary>
        public string LAR04 { get; set; }
    }
}
