using System;

namespace SIE.EventMessages.PDCA
{
    /// <summary>
    /// 系统推送不合格检验单的缺陷信息
    /// </summary>
    [Serializable]
    public class DefectInfo
    {
        /// <summary>
        /// 缺陷
        /// </summary>
        public double DefectId { get; set; }

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public string DefectLevel { get; set; }

        /// <summary>
        /// 质量类型
        /// </summary>
        public string QualityType { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        public string DefectDescription { get; set; }

        /// <summary>
        /// 缺陷编码
        /// </summary>
        public string DefectCode { get; set; }
    }
}