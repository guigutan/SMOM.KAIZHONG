using System;

namespace SIE.Resources.WipResources.Models
{
    /// <summary>
    /// 资源、日历方案信息
    /// </summary>
    [Serializable]
    public class ResourceSchemeInfo
    {
        /// <summary>
        /// 生产资源ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 日历方案ID
        /// </summary>
        public double SchemeId { get; set; }
    }
}