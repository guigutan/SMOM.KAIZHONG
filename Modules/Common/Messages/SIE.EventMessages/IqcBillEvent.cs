using SIE.Common;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// 来料检验单信息
    /// </summary>
    [Serializable]
    public class IqcBillEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IqcBillEvent()
        {
            IqcBillEventList = new List<IqcEvent>();
        }

        /// <summary>
        /// 来料检验单信息列表
        /// </summary>
        public List<IqcEvent> IqcBillEventList { get; set; }
    }

    /// <summary>
    /// 来料检验单信息
    /// </summary>
    [Serializable]
    public class IqcEvent
    {
        /// <summary>
        /// 接收记录Id
        /// </summary> 
        public double? InspectionId { get; set; }

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? Result { get; set; }

        /// <summary>
        /// 检验员
        /// </summary>
        public double? InspectionBy { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime? InspectionDate { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal? PassQty { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal? FailQty { get; set; }

        /// <summary>
        /// 不合格处理方式
        /// </summary>
        public ProcessMode? ProcessMode { get; set; }

        /// <summary>
        /// 不合格审核人员
        /// </summary>
        public double? AuditBy { get; set; }

        /// <summary>
        /// 不合格审核时间
        /// </summary>
        public DateTime? AuditDate { get; set; }

        /// <summary>
        /// 条码信息
        /// </summary>
        public List<RidModel> Rid { get; set; } = new List<RidModel>();
    }

    /// <summary>
    /// RID数据
    /// </summary>
    [Serializable]
    public class RidModel
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Rid { get; set; }

        /// <summary>
        /// 是否破坏性检验
        /// </summary>
        public bool IsBrokenInsp { get; set; }

        /// <summary>
        /// 检验结果 
        /// </summary>
        public InspectionResult? InspResult { get; set; }
    }
}
