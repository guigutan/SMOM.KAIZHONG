using SIE.Core.Enums;
using System;

namespace SIE.Resources.ProcessTechs
{
    /// <summary>
    /// 工艺信息
    /// </summary>
    [Serializable]
    public class ProcessTechInfo
    {
        /// <summary>
        /// 制程工艺ID
        /// </summary>
        public double ProcessTechId { get; set; }

        /// <summary>
        /// 工艺编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工艺名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 制程工艺类型ID
        /// </summary>
        public double ProcessTechTypeId { get; set; }

        /// <summary>
        /// 标准工时(s/单位)
        /// </summary>
        public decimal? SAM { get; set; }

        /// <summary>
        /// 转款时间
        /// </summary>
        public decimal? TransferTime { get; set; }

        /// <summary>
        /// 工作时长(天)
        /// </summary>
        public decimal WorkingHours { get; set; }

        /// <summary>
        /// 是否瓶颈工序
        /// </summary>
        public bool IsBottleneck { get; set; }

        /// <summary>
        /// 是否排产
        /// </summary>
        public bool IsScheduling { get; set; }

        /// <summary>
        /// 版面类型
        /// </summary>
        public Deck? DeckType { get; set; }

        /// <summary>
        /// 外协时长（天）
        /// </summary>
        public decimal? OutAssistDay { get; set; }
    }
}