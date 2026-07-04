using System;

namespace SIE.Wpf.ESop
{
    /// <summary>
    /// 工作单元信息
    /// </summary>
    [Serializable]
    public partial class ESopWorkcell
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public double UserId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 显示点Id
        /// </summary>
        public double DisplayPointId { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public double? StationId { get; set; }
    }
}