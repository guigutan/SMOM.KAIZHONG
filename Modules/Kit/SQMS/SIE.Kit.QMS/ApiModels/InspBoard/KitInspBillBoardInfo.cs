using SIE.Common;
using SIE.QMS.Common;
using System;

namespace SIE.Kit.QMS.ApiModels.InspBoard
{
    /// <summary>
    /// 检验单信息-看板
    /// </summary>
    [Serializable]
    public class KitInspBillBoardInfo
    {
        /// <summary>
        /// 单据ID  
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 检验单号  
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 物料编码  
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称  
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 报检数量  
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 检验状态  
        /// </summary>
        public InspectionStatus InspectionStatus { get; set; }

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? InspectionResult { get; set; }

        /// <summary>
        /// 检验状态-显示 
        /// </summary>
        public string InspectionStatusLabel { get; set; }

        /// <summary>
        /// 制单时间  
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 超时标识  
        /// </summary>
        public bool IsTimeOut { get; set; }

        /// <summary>
        /// 检验员
        /// </summary>
        public string InspectorName { get; set; }

    }
}
