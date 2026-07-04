using System;

namespace SIE.EventMessages.MES.Inspection.Models
{
    /// <summary>
    /// 不合格项目信息
    /// </summary>
    [Serializable]
    public class FailedBillDetailInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 缺陷代码描述
        /// </summary>
        public string DefectCodeDescription { get; set; }

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDescription { get; set; }

        /// <summary>
        /// 是否定量
        /// </summary>
        public bool IsQuantitative { get; set; }    

        /// <summary>
        /// 规格上限
        /// </summary>
        public string MaxValue { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public string MinValue { get; set; }

        /// <summary>
        /// 不合格值
        /// </summary>
        public string FailValues { get; set; }
    }
}
